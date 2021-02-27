using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Web.Http;
using TSICS.Helper;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers.Api.V1
{
    public class ArticleSearchController : ApiController
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
        public IHttpActionResult Post(string id, ApiJsonDraw apiDraw)
        {
            ApiJsonStatus apiJsonStatusM = Factory.Create<ApiJsonStatus>("ApiJsonStatus", ClassType.clsTypeDataModel);
            //var draw = Request.Content.ReadAsStringAsync();
            apiJsonStatusM.code = 200;
            apiJsonStatusM.message = "Post ok " + apiDraw.draw;

            var start = 3 * (apiDraw.draw - 1);
            //if (start == 0)
            //    start = 1;

            List<ArticleListJson> aljModel = new List<ArticleListJson>();

            var alModel = _articleBs.GetListApprovedByCategory(start, id);

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
                ajModel.type = "Artikel";
                ajModel.date = item.CreatedAt?.ToString("dd MMM yyyy");

                aljModel.Add(ajModel);
            }

            return Ok(new { status = apiJsonStatusM, data = aljModel });
        }
    }
}
