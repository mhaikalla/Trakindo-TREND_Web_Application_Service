using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Com.Trakindo.TSICS.Data.Model;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.Framework;
using PagedList;
using TSICS.Helper;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace TSICS.Areas.Admin.Controllers
{

    public class ArticlesController : Controller
    {
        private readonly ArticleBusinessService _articleService = Factory.Create<ArticleBusinessService>("Article", ClassType.clsTypeBusinessService);
        private readonly ArticleCategoryBusinessService _articleCategoryBs = Factory.Create<ArticleCategoryBusinessService>("ArticleCategory", ClassType.clsTypeBusinessService);
        private readonly ArticleFileBusinessService _articleFileBs = Factory.Create<ArticleFileBusinessService>("ArticleFile", ClassType.clsTypeBusinessService);
        private readonly ArticleTagsBusinessService _articleTagsService = Factory.Create<ArticleTagsBusinessService>("ArticleTags", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _UserBService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        // GET: Admin/Articles
        public ActionResult Index(int? page)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            int pageSize = 999999999;
            int pageNumber = (page ?? 1);

            var listArtikel = _articleService.GetList();
            List<User> userData = new List<User>();
            if (listArtikel != null)
            {
                foreach (var item in listArtikel)
                {
                    User userdata = _UserBService.GetDetail(item.CreatedBy);
                    if (userdata != null)
                    {
                        User userobject = new User()
                        {
                            UserId = item.ArticleId,
                            Name = userdata.Name
                        };
                        userData.Add(userobject);
                    }
                    else
                    {
                        User userobject = new User()
                        {
                            UserId = item.ArticleId,
                            Name = null
                        };
                        userData.Add(userobject);
                    }
                }
            }
            ViewBag.UserData = userData;
            return View(listArtikel.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Articles/Details/5
        public ActionResult Details(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            Article articleObject = _articleService.Detail(id);

            SetDropdownArtikelCategory(articleObject);

            var tags = _articleTagsService.GetTagsByArticle(id);
            ViewBag.Tags = tags;
            var attachments = _articleFileBs.GetListByArtikelId(id);
            List<TicketAttachmentsAPI> listAttachment = new List<TicketAttachmentsAPI>();
            string attachmentsPath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Article/Attachments/");
            if (attachments != null)
            {
                string[] imgExt = { ".jpg", ".png", ".jpeg", ".gif", ".bmp", ".tif", ".tiff" };
                foreach (var attachment in attachments)
                {
                    if (imgExt.Contains(Path.GetExtension(attachment.Name)))
                    {
                        listAttachment.Add(new TicketAttachmentsAPI
                        {
                            Id = attachment.ArticleFileId,
                            Name = Common.ImageToBase64(attachmentsPath + attachment.Name),
                            Level = attachment.LevelUser,
                            Type = Path.GetExtension(attachment.Name)
                        });
                    }
                    else
                    {
                        listAttachment.Add(new TicketAttachmentsAPI
                        {
                            Id = attachment.ArticleFileId,
                            Name = attachment.Name,
                            Level = attachment.LevelUser,
                            Type = Path.GetExtension(attachment.Name)
                        });
                    }
                }
            }
            ViewBag.Attachments = listAttachment;
            ViewBag.HeaderImg = WebConfigure.GetDomain() + "/Upload/Article/Header/" + articleObject.HeaderImage;
            if (articleObject == null)
            {
                return HttpNotFound();
            }
            return View(articleObject);
        }

        // GET: Admin/Articles/Create
        public ActionResult Create()
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ViewBag.EmptyImg = WebConfigure.GetDomain() + "/assets/images/repository/empty-image.jpg";
            ViewBag.ListCategory1 = _articleCategoryBs.GetListParent(0);

            return View();
        }

        // POST: Admin/Articles/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Article artikelObject, FormCollection collection)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            var artikelModel = ParsingCreate(artikelObject);

            artikelModel.Status = collection["type"].Contains("draft") ? 2 : 1;

            String token = Common.AccessToken() + DateTime.Now.ToString("ddMMyyyy");
            token = token.Replace("/", "");
            token = token.Replace("+", "");
            artikelModel.Token = token;
            artikelModel = _articleService.Add(artikelModel);

            // Check if user put some tags
            if (collection.AllKeys.Contains("tag-input[]"))
            {
                //insert articleTags
                string tags = collection["tag-input[]"];
                InsertArticleTags(tags, artikelModel.ArticleId);
            }

            //upload
            if (Request.Files.Count > 0)
            {
                for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                {
                    if (Request.Files.GetKey(i) == "header")
                    {
                        var file = Request.Files.Get(i);
                        string response = Common.ValidateFileUpload(file);

                        if (response.Equals("true"))
                        {
                            if (file != null)
                            {
                                var fileName = artikelModel.ArticleId + Path.GetExtension(file.FileName);

                                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Article/Header/"), fileName);

                                file.SaveAs(path);

                                artikelModel.HeaderImage = fileName;
                            }

                            _articleService.Edit(artikelModel);
                        }
                    }
                }

                if (Request.Files.AllKeys.Contains("attachment[]"))
                {
                    ArticleFile artikelFile = Factory.Create<ArticleFile>("ArticleFile", ClassType.clsTypeDataModel);
                    int attachmentIndex = 1;
                    string[] fileLevel = collection["file_level[]"].Split(',');
                    for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                    {
                        if (Request.Files.GetKey(i) == "attachment[]")
                        {
                            var file = Request.Files[i];
                            string response = Common.ValidateFileUpload(file);
                            string dateString = DateTime.Now.ToString("yyyyMMddHmmss");
                            if (response.Equals("true"))
                            {
                                artikelFile.ArticleId = artikelModel.ArticleId;
                                artikelFile.Name = Common.UploadFileArticle(file, Path.GetFileNameWithoutExtension(file.FileName) + "-" + dateString + "-" + attachmentIndex);
                                artikelFile.LevelUser = fileLevel[i - 1];
                                artikelFile.CreatedAt = DateTime.Now;

                                _articleFileBs.Add(artikelFile);
                                attachmentIndex += 1;
                            }
                        }
                    }
                }
            }
            if (collection["type"].Contains("draft"))
            {
                return RedirectToAction("Draft");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Articles/Edit/5
        public ActionResult Edit(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ViewBag.Domain = WebConfigure.GetDomain() + "/Upload/Article/Attachments/";
            Article articleObject = _articleService.Detail(id);

            SetDropdownArtikelCategory(articleObject);

            var tags = _articleTagsService.GetTagsByArticle(id);
            ViewBag.Tags = tags;
            //Attachment
            var attachments = _articleFileBs.GetListByArtikelId(id);
            List<TicketAttachmentsAPI> listAttachment = new List<TicketAttachmentsAPI>();
            string attachmentsPath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Article/Attachments/");
            if (attachments != null)
            {
                string[] imgExt = { ".jpg", ".png", ".jpeg", ".gif", ".bmp", ".tif", ".tiff" };
                foreach (var attachment in attachments)
                {
                    if (imgExt.Contains(Path.GetExtension(attachment.Name)))
                    {
                        listAttachment.Add(new TicketAttachmentsAPI
                        {
                            Id = attachment.ArticleFileId,
                            Name = Common.ImageToBase64(attachmentsPath + attachment.Name),
                            Level = attachment.LevelUser,
                            Type = Path.GetExtension(attachment.Name),
                            NameWithOutBase64 = attachment.Name
                        });
                    }
                    else
                    {
                        listAttachment.Add(new TicketAttachmentsAPI
                        {
                            Id = attachment.ArticleFileId,
                            Name = attachment.Name,
                            Level = attachment.LevelUser,
                            Type = Path.GetExtension(attachment.Name),
                            NameWithOutBase64 = attachment.Name
                        });
                    }
                }
            }
            ViewBag.Attachments = listAttachment;
            ViewBag.HeaderImg = WebConfigure.GetDomain() + "/Upload/Article/Header/" + articleObject.HeaderImage;
            ViewBag.DefaultImg = WebConfigure.GetDomain() + "/Upload/attachment-default.png";
            ViewBag.EmptyImg = WebConfigure.GetDomain() + "/assets/images/repository/empty-image.jpg";
            if (articleObject == null)
            {
                return HttpNotFound();
            }
            return View(articleObject);
        }

        // POST: Admin/Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Article artikelObject, FormCollection collection)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            SetDropdownArtikelCategory(artikelObject);

            var artikelModel = ParsingEdit(artikelObject, collection);

            artikelModel = _articleService.Edit(artikelModel);

            if (collection.AllKeys.Contains("tag-input[]"))
            {
                string tags = collection["tag-input[]"];
                UpdateArticleTags(tags, artikelModel.ArticleId);
            }
            //upload
            if (Request.Files.Count > 0)
            {
                for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                {
                    if (Request.Files.GetKey(i) == "header")
                    {
                        var file = Request.Files.Get(i);
                        string response = Common.ValidateFileUpload(file);

                        if (response.Equals("true"))
                        {
                            if (file != null)
                            {
                                var fileName = artikelModel.ArticleId + Path.GetExtension(file.FileName);

                                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Article/Header/"), fileName);

                                file.SaveAs(path);

                                artikelModel.HeaderImage = fileName;
                            }

                            _articleService.Edit(artikelModel);
                        }
                    }
                }

                if (Request.Files.AllKeys.Contains("attachment[]"))
                {
                    ArticleFile artikelFile = Factory.Create<ArticleFile>("ArticleFile", ClassType.clsTypeDataModel);
                    int attachmentIndex = 1;
                    string[] fileLevel = collection["file_level[]"].Split(',');
                    for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                    {
                        if (Request.Files.GetKey(i) == "attachment[]")
                        {
                            var file = Request.Files[i];
                            string response = Common.ValidateFileUpload(file);
                            string dateString = DateTime.Now.ToString("yyyyMMddHmmss");
                            if (response.Equals("true"))
                            {
                                artikelFile.ArticleId = artikelModel.ArticleId;
                                artikelFile.Name = Common.UploadFileArticle(file, Path.GetFileNameWithoutExtension(file.FileName) + "-" + dateString + "-" + attachmentIndex);
                                artikelFile.LevelUser = fileLevel[i - 1];
                                artikelFile.CreatedAt = DateTime.Now;

                                _articleFileBs.Add(artikelFile);
                                attachmentIndex += 1;
                            }
                        }
                    }
                }
            }
            if (artikelObject.Status == 9 && artikelModel.Status == 1)
            {
                Email.SendMailSubmitedArticle(artikelModel, _UserBService.GetDetail(artikelObject.CreatedBy));
            }
            if (collection["type"].Contains("draft")) {
                return RedirectToAction("Draft");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Articles/Delete/5
        public ActionResult Delete(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            Article articleObject = _articleService.Detail(id);

            SetDropdownArtikelCategory(articleObject);

            var tags = _articleTagsService.GetTagsByArticle(id);
            ViewBag.Tags = tags;
            var attachments = _articleFileBs.GetListByArtikelId(id);
            List<TicketAttachmentsAPI> listAttachment = new List<TicketAttachmentsAPI>();
            string attachmentsPath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Article/");
            if (attachments != null)
            {
                string[] imgExt = { ".jpg", ".png", ".jpeg", ".gif", ".bmp", ".tif", ".tiff" };
                foreach (var attachment in attachments)
                {
                    if (imgExt.Contains(Path.GetExtension(attachment.Name)))
                    {
                        listAttachment.Add(new TicketAttachmentsAPI
                        {
                            Id = attachment.ArticleFileId,
                            Name = Common.ImageToBase64(attachmentsPath + attachment.Name),
                            Type = Path.GetExtension(attachment.Name)
                        });
                    }
                    else
                    {
                        listAttachment.Add(new TicketAttachmentsAPI
                        {
                            Id = attachment.ArticleFileId,
                            Name = attachment.Name,
                            Type = Path.GetExtension(attachment.Name)
                        });
                    }
                }
            }
            ViewBag.Attachments = listAttachment;
            ViewBag.HeaderImg = WebConfigure.GetDomain() + "/Upload/Article/Header/" + articleObject.HeaderImage;
            if (articleObject == null)
            {
                return HttpNotFound();
            }
            return View(articleObject);
        }

        // POST: Admin/Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            Article article = _articleService.GetDetail(id);
            article.Status = 4; //Status Deleted
            _articleService.Edit(article);
            return RedirectToAction("Index");
        }

        public ActionResult Draft(int? page)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            int pageSize = 999999999;
            int pageNumber = (page ?? 1);

            var listArtikel = _articleService.GetListDraft();
            List<User> userData = new List<User>();
            if (listArtikel != null)
            {
                foreach (var item in listArtikel)
                {
                    User userdata = _UserBService.GetDetail(item.CreatedBy);
                    if (userdata != null)
                    {
                        User userobject = new User()
                        {
                            UserId = item.ArticleId,
                            Name = userdata.Name
                        };
                        userData.Add(userobject);
                    }
                    else
                    {
                        User userobject = new User()
                        {
                            UserId = item.ArticleId,
                            Name = null
                        };
                        userData.Add(userobject);
                    }
                }
            }
            ViewBag.UserData = userData;
            return View(listArtikel.ToPagedList(pageNumber, pageSize));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetChild(int id)
        {
            return Json(new SelectList(_articleCategoryBs.GetListParent(id), "ArticleCategoryId", "Name"));
        }

        private Article ParsingEdit(Article artikel, FormCollection collect)
        {
            var articleOld = _articleService.Detail(artikel.ArticleId);

            var articleReturn = articleOld;
            articleReturn.Position = 0;
            articleReturn.Title = artikel.Title;
            articleReturn.Description = artikel.Description;
            articleReturn.Status = collect["type"].Contains("draft") ? 2 : Convert.ToInt32(collect["Status"]);
            articleReturn.LevelUser = artikel.LevelUser;

            articleReturn.Category1Id = artikel.Category1Id;
            if(artikel.Category1Id != 0) { 
                articleReturn.Category1 = _articleCategoryBs.GetDetail(artikel.Category1Id).Name ?? "-";
            }
            if (artikel.Category2Id != 0)
            {
                articleReturn.Category2Id = artikel.Category2Id;
                articleReturn.Category2 = _articleCategoryBs.GetDetail(artikel.Category2Id).Name ?? "-";
            }
            if (artikel.Category3Id != 0)
            {
                articleReturn.Category3Id = artikel.Category3Id;
                articleReturn.Category3 = _articleCategoryBs.GetDetail(artikel.Category3Id).Name ?? "-";
            }
            if (artikel.Category4Id != 0)
            {
                articleReturn.Category4Id = artikel.Category4Id;
                articleReturn.Category4 = _articleCategoryBs.GetDetail(artikel.Category4Id).Name ?? "-";
            }
            if (artikel.Category5Id != 0)
            {
                articleReturn.Category5Id = artikel.Category5Id;
                articleReturn.Category5 = _articleCategoryBs.GetDetail(artikel.Category5Id).Name ?? "-";
            }
            if (artikel.Category6Id != 0)
            {
                articleReturn.Category6Id = artikel.Category6Id;
                articleReturn.Category6 = _articleCategoryBs.GetDetail(artikel.Category6Id).Name ?? "-";
            }
            if (artikel.Category7Id != 0)
            {
                articleReturn.Category7Id = artikel.Category7Id;
                articleReturn.Category7 = _articleCategoryBs.GetDetail(artikel.Category7Id).Name ?? "-";
            }

            articleReturn.Aproved1At = DateTime.Now;
            articleReturn.AprovedAdminAt = DateTime.Now;
            articleReturn.AprovedAdminBy = Convert.ToInt32(Session["useridbackend"]);

            return articleReturn;
        }

        private Article ParsingCreate(Article artikel)
        {
            Article articleReturn = Factory.Create<Article>("Article", ClassType.clsTypeDataModel);
            articleReturn.Position = 0;
            articleReturn.ArticleId = artikel.ArticleId;
            articleReturn.Title = artikel.Title;
            articleReturn.Description = artikel.Description;
            articleReturn.LevelUser = artikel.LevelUser;
            articleReturn.Status = 1;
            if (artikel.Category1Id != 0)
            {
                articleReturn.Category1 = _articleCategoryBs.GetDetail(artikel.Category1Id).Name ?? "-";
            }
            articleReturn.Category1Id = artikel.Category1Id;
            if (artikel.Category2Id != 0)
                articleReturn.Category2 = _articleCategoryBs.GetDetail(artikel.Category2Id).Name ?? "-";
            articleReturn.Category2Id = artikel.Category2Id;
            if (artikel.Category3Id != 0)
                articleReturn.Category3 = _articleCategoryBs.GetDetail(artikel.Category3Id).Name ?? "-";
            articleReturn.Category3Id = artikel.Category3Id;
            if (artikel.Category4Id != 0)
                articleReturn.Category4 = _articleCategoryBs.GetDetail(artikel.Category4Id).Name ?? "-";
            articleReturn.Category4Id = artikel.Category4Id;
            if (artikel.Category5Id != 0)
                articleReturn.Category5 = _articleCategoryBs.GetDetail(artikel.Category5Id).Name ?? "-";
            articleReturn.Category4Id = artikel.Category4Id;
            if (artikel.Category6Id != 0)
                articleReturn.Category6 = _articleCategoryBs.GetDetail(artikel.Category6Id).Name ?? "-";
            articleReturn.Category6Id = artikel.Category6Id;
            if (artikel.Category7Id != 0)
                articleReturn.Category7 = _articleCategoryBs.GetDetail(artikel.Category7Id).Name ?? "-";
            articleReturn.Category7Id = artikel.Category7Id;


            //var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            //if (request != null)
            //{
            //    request.KeepAlive = true;
            //    request.Method = "POST";
            //    request.ContentType = "application/json; charset=utf-8";
            //    request.Headers.Add("authorization", "Basic MGY1NTFmZWUtMjBiMi00YzgxLTlkZWUtNmU5ZmVhNzcxMWU4");
            //    var serializer = new JavaScriptSerializer();

            //    var obj = new
            //    {
            //        app_id = "feb47b57-2d6d-40fd-b6c0-9c4b8da99c41",
            //        contents = new { en = articleReturn.Description },
            //        included_segments = new[] { "All" },
            //        headings = new { en = articleReturn.Title }
            //    };
            //    var param = serializer.Serialize(obj);
            //    byte[] byteArray = Encoding.UTF8.GetBytes(param);
            //    string responseContent = null;
            //    try
            //    {
            //        using (var writer = request.GetRequestStream())
            //        {
            //            writer.Write(byteArray, 0, byteArray.Length);
            //        }
            //        using (var response = request.GetResponse() as HttpWebResponse)
            //        {
            //            if (response != null)
            //                // ReSharper disable once AssignNullToNotNullAttribute
            //                using (var reader = new StreamReader(response.GetResponseStream()))
            //                {
            //                    responseContent = reader.ReadToEnd();
            //                }
            //        }
            //    }
            //    catch (WebException ex)
            //    {
            //        System.Diagnostics.Debug.WriteLine(ex.Message);
            //        System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            //    }
            //     System.Diagnostics.Debug.WriteLine(responseContent);
            //}
            articleReturn.CreatedAt = DateTime.Now;
            articleReturn.CreatedBy = Convert.ToInt32(Session["useridbackend"]);

            return articleReturn;

        }

        private void InsertArticleTags(string tags, int artikelId)
        {
            ArticleTags articleTag = new ArticleTags();

            string[] tagArrau = tags.Split(',');

            for (int i = 0, iLen = tagArrau.Length; i < iLen; i++)
            {
                articleTag.Name = tagArrau[i];
                articleTag.ArticleId = artikelId;

                _articleTagsService.Add(articleTag);
            }
        }

        private void SetDropdownArtikelCategory(Article article)
        {
            ArticleCategory articleCategoryEmpty = Factory.Create<ArticleCategory>("ArticleCategory", ClassType.clsTypeDataModel);
            articleCategoryEmpty.ArticleCategoryId = 0;
            articleCategoryEmpty.Name = "Select";

            List<ArticleCategory> articleListEmpty = new List<ArticleCategory> { articleCategoryEmpty };


            ViewBag.ListCategory1 = _articleCategoryBs.GetListParent(0);

            ViewBag.ListCategory2 = article.Category2Id != 0 ? _articleCategoryBs.GetListParent(article.Category1Id) : articleListEmpty;

            ViewBag.ListCategory3 = article.Category3Id != 0 ? _articleCategoryBs.GetListParent(article.Category2Id) : articleListEmpty;

            ViewBag.ListCategory4 = article.Category4Id != 0 ? _articleCategoryBs.GetListParent(article.Category3Id) : articleListEmpty;

            ViewBag.ListCategory5 = article.Category5Id != 0 ? _articleCategoryBs.GetListParent(article.Category4Id) : articleListEmpty;

            ViewBag.ListCategory6 = article.Category6Id != 0 ? _articleCategoryBs.GetListParent(article.Category5Id) : articleListEmpty;

            ViewBag.ListCategory7 = article.Category7Id != 0 ? _articleCategoryBs.GetListParent(article.Category6Id) : articleListEmpty;
        }

        private void UpdateArticleTags(string tags, int artikelId)
        {
            string[] tagsArray = tags.Split(',');
            var currentTags = _articleTagsService.GetTagsByArticle(artikelId);
            if (currentTags != null)
            {
                List<TicketTagsAPI> listCurrentTagApi = new List<TicketTagsAPI>();
                foreach (var currentTag in currentTags)
                {
                    TicketTagsAPI tagApi = new TicketTagsAPI { Name = currentTag.Name };
                    listCurrentTagApi.Add(tagApi);
                }

                foreach (var currentTagApi in listCurrentTagApi)
                {
                    if (!tags.Any(currentTagApi.Name.Contains))
                    {
                        _articleTagsService.Delete(artikelId, currentTagApi.Name);
                    }
                }
            }

            ArticleTags articleTag = new ArticleTags();

            for (int i = 0, iLen = tagsArray.Length; i < iLen; i++)
            {
                if (!_articleTagsService.IsThisTagExists(artikelId, tagsArray[i]))
                {
                    articleTag.Name = tagsArray[i];
                    articleTag.ArticleId = artikelId;

                    _articleTagsService.Add(articleTag);
                }
            }
        }
        [HttpPost]
        public ActionResult InputUseFullLink(FormCollection formCollection)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            Article article = new Article();
            article.Title = formCollection["Title"];
            article.Description = formCollection["Link"];
            article.Status = 1;
            article.Type = 2;
            article.CreatedBy = Convert.ToInt32(Session["useridbackend"]);
            article.CreatedAt = DateTime.Now;
            article.Position = 0;
            _articleService.AddUseFullLink(article);
            return RedirectToAction("Usefull", "Articles");

        }
        public ActionResult Usefull(int? page)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            int pageSize = 999999999;
            int pageNumber = (page ?? 1);
            var listArtikel = _articleService.getUseFullLink();
            List<User> userData = new List<User>();
            if (listArtikel != null)
            {
                foreach (var item in listArtikel)
                {
                    User userdata = _UserBService.GetDetail(item.CreatedBy);
                    if (userdata != null)
                    {
                        User userobject = new User()
                        {
                            UserId = item.ArticleId,
                            Name = userdata.Name
                        };
                        userData.Add(userobject);
                    }
                    else
                    {
                        User userobject = new User()
                        {
                            UserId = item.ArticleId,
                            Name = null
                        };
                        userData.Add(userobject);
                    }
                }
            }
            ViewBag.UserData = userData;
            return View(listArtikel.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult EditUsefull(FormCollection collection)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            var articleId = Convert.ToInt32(collection["ArticleId"]);
            Article link = _articleService.getDetailUseFullLink(articleId);
            link.Description = collection["Description"];
            link.Title = collection["Title"];
            link.Type = 2;
            link.Status = 1;
            link.Position = 0;
            _articleService.EditUseFullLink(link);
            return RedirectToAction("Usefull");
        }
        [HttpPost]
        public ActionResult DeleteUseFull(FormCollection collection)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            _articleService.DeleteUseFullLink(Convert.ToInt32(collection["ArticleId"]));
            return RedirectToAction("Usefull");
        }

        [HttpPost]
        public ActionResult DeleteFileArticle(int id)
        {
            var deleteFileArticle = _articleFileBs.Delete(_articleFileBs.GetDetail(id));
            //Response.Write("{\"status\":true}");
            return Json(new { data = deleteFileArticle });
        }

        public string GetAuthorName(int id)
        {
            string Name = _UserBService.GetDetail(id).Name;
            return (String.IsNullOrWhiteSpace(Name) ? "" :Name );
        }
    }
}
