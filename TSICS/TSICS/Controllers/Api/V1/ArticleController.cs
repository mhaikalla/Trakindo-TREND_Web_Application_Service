using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;
using TSICS.Helper;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers.Api.V1
{
    public class ArticleController : ApiController
    {
       private readonly ArticleBusinessService _articleBs = Factory.Create<ArticleBusinessService>("Article", ClassType.clsTypeBusinessService);

       public IHttpActionResult Get(int id)
        {
            ApiJsonStatus apiJsonStatusM = Factory.Create<ApiJsonStatus>("ApiJsonStatus", ClassType.clsTypeDataModel);

            apiJsonStatusM.code = 200;
            apiJsonStatusM.message = "ok";

            List<ArticleListJson> aljModel = new List<ArticleListJson>();

            

            return Ok(new { status = apiJsonStatusM, data = aljModel });
        }
        public IHttpActionResult Post(ApiJsonDraw apiDraw, String level,String category, String keyword)
        {
            ApiJsonStatus apiJsonStatusM = Factory.Create<ApiJsonStatus>("ApiJsonStatus", ClassType.clsTypeDataModel);
            
            apiJsonStatusM.code = 200;
            apiJsonStatusM.message = "Post ok " + apiDraw.draw;

            var start = 6 * (apiDraw.draw - 1);

            List<ArticleListJson> aljModel = new List<ArticleListJson>();

            var alModel = _articleBs.GetListApproved(start, level, category, keyword);

            foreach (var item in alModel)
            {
                ArticleListJson ajModel = Factory.Create<ArticleListJson>("ArticleListJson", ClassType.clsTypeDataModel);
                ArticleListJsonImg ajiModel = Factory.Create<ArticleListJsonImg>("ArticleListJsonImg", ClassType.clsTypeDataModel);

                string attachmentsPath = WebConfigure.GetDomain() + "/Upload/Article/Header/";
                ajiModel.src = attachmentsPath + item.HeaderImage;
                ajiModel.label = item.Category1;
                ajiModel.alt = item.Category1;
                ajModel.img = ajiModel;
                ajModel.link = WebConfigure.GetDomain() + "/Library/Detail/" + item.ArticleId;
                ajModel.group_url = WebConfigure.GetDomain() + "/Library?category=" + item.Category1Id;
                ajModel.title = item.Title;
                ajModel.text = Common.GetShortDescription(item.Description);
                ajModel.type = "Article";
                ajModel.date = item.CreatedAt?.ToString("dd MMM yyyy");

                aljModel.Add(ajModel);
            }
            return Ok(new { status = apiJsonStatusM, data = aljModel });
        }
        [HttpPost]
        [Route("api/Article/Category/{id}")]
        public IHttpActionResult PostCategory(ApiJsonDraw apiDraw, string level, string id)
        {
            ApiJsonStatus apiJsonStatusM = Factory.Create<ApiJsonStatus>("ApiJsonStatus", ClassType.clsTypeDataModel);

            apiJsonStatusM.code = 200;
            apiJsonStatusM.message = "Post ok" + apiDraw.draw;

            var start = 3 * (apiDraw.draw - 1);

            List<ArticleListJson> aljModel = new List<ArticleListJson>();

            var alModel = _articleBs.GetListCategory(start, level, id);

            foreach (var item in alModel)
            {
                
                ArticleListJson ajModel =
                    Factory.Create<ArticleListJson>("ArticleListJson", ClassType.clsTypeDataModel);
                ArticleListJsonImg ajiModel =
                    Factory.Create<ArticleListJsonImg>("ArticleListJsonImg", ClassType.clsTypeDataModel);

                string attachmentsPath = WebConfigure.GetDomain() + "/Upload/Article/Header/";
                ajiModel.src = attachmentsPath + item.HeaderImage;
                ajiModel.label = item.Category1;
                ajiModel.alt = item.Category1;
                ajModel.img = ajiModel;
                ajModel.link = WebConfigure.GetDomain() + "/Library/Detail/" + item.ArticleId;
                ajModel.group_url = WebConfigure.GetDomain() + "/Library?category=" + item.Category1Id;
                ajModel.title = item.Title;
                ajModel.text = Common.GetShortDescription(item.Description);
                ajModel.type = "Artikel";
                ajModel.date = item.CreatedAt?.ToString("dd MMM yyyy");

                aljModel.Add(ajModel);
            }
            return Ok(new { status = apiJsonStatusM, data = aljModel });
        }

        public IHttpActionResult Post(ApiJsonDraw apiDraw, String level, String keyword)
        {
            ApiJsonStatus apiJsonStatusM = Factory.Create<ApiJsonStatus>("ApiJsonStatus", ClassType.clsTypeDataModel);
            
            apiJsonStatusM.code = 200;
            apiJsonStatusM.message = "Post ok " + apiDraw.draw;

            var start = 3 * (apiDraw.draw - 1);

            List<ArticleListJson> aljModel = new List<ArticleListJson>();

            var alModel = _articleBs.GetSearchData(start, keyword, level);
            
            foreach (var item in alModel)
            {
                ArticleListJson ajModel = Factory.Create<ArticleListJson>("ArticleListJson", ClassType.clsTypeDataModel);
                ArticleListJsonImg ajiModel = Factory.Create<ArticleListJsonImg>("ArticleListJsonImg", ClassType.clsTypeDataModel);

                string attachmentsPath = WebConfigure.GetDomain() + "/Upload/Article/Header/";
                ajiModel.src = attachmentsPath + item.HeaderImage;
                ajiModel.label = item.Category1;
                ajiModel.alt = item.Category1;
                ajModel.img = ajiModel;
                ajModel.link = WebConfigure.GetDomain() + "/Library/Detail/" + item.ArticleId;
                ajModel.title = item.Title;
                ajModel.text = Common.GetShortDescription(item.Description);
                ajModel.type = "Article";
                ajModel.date = item.CreatedAt?.ToString("dd MMM yyyy");
                aljModel.Add(ajModel);
            }

            return Ok(new { status = apiJsonStatusM, data = aljModel });
        }

        [HttpPost]
        [Route("api/article/uploadimagecontent")]
        public IHttpActionResult UploadImageContent()
        {
            ArticleImageUpload uploadResponse = new ArticleImageUpload();
            try
            {
                var postedFile = HttpContext.Current.Request.Files[0];
                string dateString = DateTime.Now.ToString("yyyyMMddHmmss");
                string fileName = dateString + "-" + postedFile.FileName;
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/Article/Content/"), fileName);
                postedFile.SaveAs(path);

                uploadResponse.uploaded = 1;
                uploadResponse.fileName = fileName;
                uploadResponse.url = WebConfigure.GetDomain() + "/Upload/Article/Content/" + fileName;
                return Ok(uploadResponse);
            }
            catch (Exception e)
            {
                return Ok(uploadResponse.error = e.InnerException?.ToString());
            }
        }
    }
}
