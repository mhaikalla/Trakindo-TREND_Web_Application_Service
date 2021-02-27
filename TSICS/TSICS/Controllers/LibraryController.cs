using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TSICS.Helper;
using System.Collections.Generic;
using System.Net;
using PagedList;

using System.Text;
using System.Web.Script.Serialization;
using System.Reflection;

namespace TSICS.Controllers
{


    public class LibraryController : Controller
    {
        private readonly ArticleBusinessService _articleBs = Factory.Create<ArticleBusinessService>("Article", ClassType.clsTypeBusinessService);
        private readonly ArticleFileBusinessService _articleFileBs = Factory.Create<ArticleFileBusinessService>("ArticleFile", ClassType.clsTypeBusinessService);
        private readonly ArticleCategoryBusinessService _articleCategoryBs = Factory.Create<ArticleCategoryBusinessService>("ArticleCategory", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly ArticleTagsBusinessService _articleTagsService = Factory.Create<ArticleTagsBusinessService>("ArticleTags", ClassType.clsTypeBusinessService);
        private readonly EmployeeMasterBusinessService _mstEmployeeBService = Factory.Create<EmployeeMasterBusinessService>("EmployeeMaster", ClassType.clsTypeBusinessService);
        private readonly UserRoleBusinessService _userRoleBService = Factory.Create<UserRoleBusinessService>("UserRole", ClassType.clsTypeBusinessService);
        private readonly UserMessageBusinessService _userMessageBService = Factory.Create<UserMessageBusinessService>("UserMessage", ClassType.clsTypeBusinessService);
        private readonly UserTsManagerBusinessService _userTsManagerBusinessService = Factory.Create<UserTsManagerBusinessService>("UserTsManager", ClassType.clsTypeBusinessService);
        private readonly LogErrorBusinessService _logErrorBService = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);

        // GET: Library
        public ActionResult Index(String keyword = "", String category = "")
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
                ViewBag.Domain = WebConfigure.GetDomain();
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj() != "" && Session["userid"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (Session["username"] != null)
                {
                    if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }

                ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
                ViewBag.key = keyword;
                ViewBag.Category = category;
                return View();
            }
            catch (Exception ex)
            {
                _logErrorBService.WriteLog("Library Index", MethodBase.GetCurrentMethod().Name, ex.ToString() );
                throw;
            }
        }

        // GET: Library/Details/5
        public ActionResult Detail(int id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj() != "" && Session["userid"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (Session["username"] != null)
                {
                    if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }

                var articleModel = _articleBs.Approved(id);

                var listArticleFileModel = _articleFileBs.GetListByArtikelId(id);
                var listArticleTagsModel = _articleTagsService.GetTagsByArticle(id);

                ViewBag.ListFile = listArticleFileModel.Count == 0 ? null : listArticleFileModel;
                ViewBag.ArticleTag = listArticleTagsModel.Count == 0 ? null : listArticleTagsModel;
                ViewBag.PathImg = articleModel.HeaderImage == null ? WebConfigure.GetDomain() + "/assets/images/repository/empty-image.jpg" : WebConfigure.GetDomain() + "/Upload/Article/Header/" + articleModel.HeaderImage;
                ViewBag.Domain = WebConfigure.GetDomain();

                ViewBag.RecentArticle = _articleBs.GetRecent();
                ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
                return View(articleModel);
            }
            catch (Exception ex)
            {
                _logErrorBService.WriteLog("Detail Library", MethodBase.GetCurrentMethod().Name, ex.ToString());
                throw;
            }
        }

        // GET: Library/Create
        public ActionResult Create()
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj() != "" && Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["username"] != null)
            {
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            if (!Common.CheckUserYellow())
                return RedirectToAction("Login", "Account");
            ViewBag.EmptyImg = WebConfigure.GetDomain() + "/assets/images/repository/empty-image.jpg";
            ViewBag.ListCategory1 = _articleCategoryBs.GetListParent(0);
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            return View();
        }

        // POST: Library/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Article articleObject, FormCollection collection)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj() != "" && Session["userid"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (Session["username"] != null)
                {
                    if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                if (!Common.CheckUserYellow())
                    return RedirectToAction("Login", "Account");

                articleObject.Position = 0;
                if (articleObject.Category1Id != 0)
                    articleObject.Category1 = _articleCategoryBs.GetDetail(articleObject.Category1Id).Name ?? "-";
                if(articleObject.Category2Id != 0)
                    articleObject.Category2 = _articleCategoryBs.GetDetail(articleObject.Category2Id).Name ?? "-";
                if(articleObject.Category3Id != 0)
                    articleObject.Category3 = _articleCategoryBs.GetDetail(articleObject.Category3Id).Name ?? "-";
                if(articleObject.Category4Id != 0)
                    articleObject.Category4 = _articleCategoryBs.GetDetail(articleObject.Category4Id).Name ?? "-";
                if(articleObject.Category5Id != 0)
                    articleObject.Category5 = _articleCategoryBs.GetDetail(articleObject.Category5Id).Name ?? "-";
                if(articleObject.Category6Id != 0)
                    articleObject.Category6 = _articleCategoryBs.GetDetail(articleObject.Category6Id).Name ?? "-";
                if(articleObject.Category7Id != 0)
                    articleObject.Category7 = _articleCategoryBs.GetDetail(articleObject.Category7Id).Name ?? "-";
                
                articleObject.CreatedAt = DateTime.Now;
            if (collection["type"] == "draft")
            {
                articleObject.Status = 2;
            }
            else
            {
                articleObject.Status = 0;
            }
            articleObject.CreatedBy = Convert.ToInt32(Session["userid"]);
               
                String token = Common.AccessToken() + DateTime.Now.ToString("ddMMyyyy");
                token = token.Replace("/", "");
                token = token.Replace("+", "");
                articleObject.Token = token;
                var articleModel = _articleBs.Add(articleObject);

                if (Request.Files.Count > 0)
                {

                    //ArticleFile articleFile = Factory.Create<ArticleFile>("ArticleFile", ClassType.clsTypeDataModel);

                    //for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                    //{
                    //    var file = Request.Files[i];
                    //    string response = Common.ValidateFileUpload(file);

                    //    if (response.Equals("true"))
                    //    {
                    //        articleFile.ArticleId = articleModel.ArticleId;
                    //        articleFile.Name = Common.UploadFileArticle(file, articleModel.ArticleId + "-" + i.ToString());
                    //        articleFile.CreatedAt = DateTime.Now;

                    //        _articleFileBs.Add(articleFile);
                    //    }
                    //}
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
                                    var fileName = articleModel.ArticleId + Path.GetExtension(file.FileName);

                                    var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Article/Header/"), fileName);

                                    file.SaveAs(path);

                                    articleModel.HeaderImage = fileName;
                                }

                                _articleBs.Edit(articleModel);
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
                                artikelFile.ArticleId = articleModel.ArticleId;
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
            if (collection["type"] == "draft")
            {
                return RedirectToAction("Draft", "Library");
            }
            else
            {
                Email.SendMailPublishArticle(articleModel);
                Email.SendMailNeedApp1Article(articleModel);
                return RedirectToAction("List", "Library");
            }
        }
        // GET: Library/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj() != "" && Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["username"] != null)
            {
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            if (!Common.CheckUserYellow())
                return RedirectToAction("Login", "Account");

            Article articleObject = _articleBs.Detail(id);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Article artikelObject, FormCollection collection)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj() != "" && Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["username"] != null)
            {
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            if (!Common.CheckUserYellow())
                return RedirectToAction("Login", "Account");
            SetDropdownArtikelCategory(artikelObject);

            var artikelModel = ParsingEdit(artikelObject, collection);

            artikelModel =  _articleBs.Edit(artikelModel);

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

                            _articleBs.Edit(artikelModel);
                        }
                    }
                }

                if (Request.Files.AllKeys.Contains("attachment[]"))
                {
                    _articleFileBs.DeleteAll(artikelModel.ArticleId);
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
            if (collection["type"] == "draft")
            {
                return RedirectToAction("Draft", "Library");
            }
            else
            {
                Email.SendMailPublishArticle(artikelModel);
                Email.SendMailNeedApp1Article(artikelModel);
                return RedirectToAction("List", "Library");
            }
        }

        // GET: Library/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
            {
                return RedirectToAction("Login", "Account");
            }

            Article articleObject = _articleBs.Detail(id);
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
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
                            Type = Path.GetExtension(attachment.Name),
                            NameWithOutBase64 = attachment.Name
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

        // POST: Library/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            Article article = _articleBs.GetDetail(id);
            article.Status = 4;
            _articleBs.Edit(article);
            return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult GetChild(int id)
        {
            return Json(new SelectList(_articleCategoryBs.GetListParent(id), "ArticleCategoryId", "Name"));
        }

        [HttpGet]
        public ActionResult PreviewLibrary(Article article)
        {

            return View(article);
        }
        public string GetAuthorName(int id)
        {
            if (id == 0)
                return "";

            return _userBService.GetDetail(id) == null ? "TREND Admin" : _userBService.GetDetail(id).Name;
        }

        [HttpGet]
        public ActionResult List()
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            if (Session["userid"] == null)
            {
                return View("LoginRedirect");
            }
            else
            {
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                {
                    return View("LoginRedirect");
                }
                var listArtikel = _articleBs.GetMyList(Convert.ToInt32(Session["userid"]));
                List<User> userData = new List<User>();
                if (listArtikel != null)
                {
                    foreach (var item in listArtikel)
                    {
                        User userdata = _userBService.GetDetail(item.CreatedBy);
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
                return View(listArtikel);
            }
        }

        [HttpGet]
        public ActionResult Draft()
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            if (Session["userid"] == null)
            {
                return View("LoginRedirect");
            }
            else
            {
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                {
                    return View("LoginRedirect");
                }
                return View(_articleBs.GetMyDraft(Convert.ToInt32(Session["userid"])));
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
        [HttpGet]
        public ActionResult Category(string id)
        {
            ViewBag.category = id;
            return View();
        }
        public ActionResult Approve1Mail(string id)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            var articleData = _articleBs.GetDetailbyToken(id);
            var userData = _userBService.GetDetail(articleData.CreatedBy);
            ViewBag.User = userData;
            ViewBag.RoleLevel = _userRoleBService.GetDetail(userData.RoleId).Name;

            ViewBag.App1 = _mstEmployeeBService.GetDetail(_mstEmployeeBService.GetDetail(userData.EmployeeId).Superior_ID);
            ViewBag.DomainLink = WebConfigure.GetDomain();
            ViewBag.RecentArticle = _articleBs.GetRecent();
            ViewBag.UseFullLink = _articleBs.getUseFullLink();
            return View(articleData);
        }

        [HttpPost]
        public ActionResult Approve1Mail(FormCollection collect)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            string token = Common.AccessToken() + Convert.ToInt32(collect["ArticleId"]);
            token = token.Replace("/", "");
            token = token.Replace("+", "");
            
            var employeeM = _mstEmployeeBService.GetDetail(Convert.ToInt32(collect["EmployeeSPVId"]));
            var employeeM2 = _mstEmployeeBService.GetDetail(employeeM.Superior_ID);
            var userT = _userBService.GetByEmployeeId(Convert.ToInt32(collect["EmployeeId"]));
            var articleData = _articleBs.GetDetail(Convert.ToInt32(collect["ArticleId"]));
            
            //articleData.Token = token;
            articleData.Aproved1By = employeeM.Employee_Id;
            articleData.Aproved1At = DateTime.Now;
            articleData.Status = 5;
            articleData.Token = token;
            var Article = _articleBs.Edit(articleData);


            //insert UserMessage
            UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
            userM.Message = String.IsNullOrWhiteSpace(collect["ApproveMsg"]) ? null : collect["ApproveMsg"];
            userM.MessageType = "Approve 1";
            userM.ToUserId = userT.UserId;
            //UserM.FromUserId = Convert.ToInt32(Session["userid"]);
            userM.CreatedAt = DateTime.Now;
            userM.ToEmployeeId = userT.EmployeeId;
            userM.FromEmployeeId = employeeM.Employee_Id;
            userM.FromUserType = Article.ArticleId;
            _userMessageBService.Add(userM, "Article");

            Dictionary<String, EmployeeMaster> ApprovalData = new Dictionary<string, EmployeeMaster>();
            ApprovalData.Add("Approval1", employeeM);
            ApprovalData.Add("Approval2", employeeM2);

            Email.SendMailResAppArticle(Article, employeeM, userT, collect["ApproveMsg"]);
            Email.SendMailNeedApp2Article(Article, ApprovalData, userT, collect["ApproveMsg"]);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Reject1(FormCollection collect)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            string token = Common.AccessToken() + Convert.ToInt32(collect["ArticleId"]);
            token = token.Replace("/", "");
            token = token.Replace("+", "");

            var employeeM = _mstEmployeeBService.GetDetail(Convert.ToInt32(collect["EmployeeSPVId"]));

            var userT = _userBService.GetByEmployeeId(Convert.ToInt32(collect["EmployeeId"]));
            var articleData = _articleBs.GetDetail(Convert.ToInt32(collect["ArticleId"]));

            //articleData.Token = token;
            articleData.Aproved1By = employeeM.Employee_Id;
            articleData.Aproved1At = DateTime.Now;
            articleData.Status = 6;
            articleData.Token = token;
            var Article = _articleBs.Edit(articleData);


            //insert UserMessage
            UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
            userM.Message = String.IsNullOrWhiteSpace(collect["RejectMsg"]) ? null : collect["RejectMsg"];
            userM.MessageType = "Reject 1";
            userM.ToUserId = userT.UserId;
            //UserM.FromUserId = Convert.ToInt32(Session["userid"]);
            userM.CreatedAt = DateTime.Now;
            userM.ToEmployeeId = userT.EmployeeId;
            userM.FromEmployeeId = userT.Approval1;
            userM.FromUserType = Article.ArticleId;
            _userMessageBService.Add(userM, "Article");
            Email.SendMailResAppArticle(Article, employeeM, userT, collect["RejectMsg"], false);
            Email.SendMailRejectArticle(Article, employeeM, userT, collect["RejectMsg"]);
            return RedirectToAction("Index");
        }


        public ActionResult Approve2Mail(string id)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            var articleData = _articleBs.GetDetailbyToken(id);
            var userData = _userBService.GetDetail(articleData.CreatedBy);
            var app1 = _mstEmployeeBService.GetDetail(_mstEmployeeBService.GetDetail(userData.EmployeeId).Superior_ID);
            ViewBag.App1 = app1;
            ViewBag.App2 = _mstEmployeeBService.GetDetail(app1.Superior_ID);
            ViewBag.User = userData;
            ViewBag.RoleLevel = _userRoleBService.GetDetail(userData.RoleId).Name;
            ViewBag.MessageResponse = _userMessageBService.GetDetail(app1.Employee_Id, articleData.ArticleId,"Article");
            ViewBag.DomainLink = WebConfigure.GetDomain();
            ViewBag.RecentArticle = _articleBs.GetRecent();
            ViewBag.UseFullLink = _articleBs.getUseFullLink();
            return View(articleData);
        }

        [HttpPost]
        public ActionResult Approve2Mail(FormCollection collect)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            string token = Common.AccessToken() + Convert.ToInt32(collect["ArticleId"]);
            token = token.Replace("/", "");
            token = token.Replace("+", "");
            var articleData = _articleBs.GetDetail(Convert.ToInt32(collect["ArticleId"]));
            var userT = _userBService.GetByEmployeeId(Convert.ToInt32(collect["EmployeeId"]));
            var Spv2 = _mstEmployeeBService.GetDetail(Convert.ToInt32(collect["EmployeeSPV2Id"]));
            var Spv1 = _mstEmployeeBService.GetDetail(Convert.ToInt32(collect["EmployeeSPVId"]));
            var TsManager = _userTsManagerBusinessService.GetDetail(1);
           
            
            articleData.Aproved2By = Spv2.Employee_Id;
            articleData.Aproved2At = DateTime.Now;
            articleData.Status = 7;
            articleData.Token = token;
            var Article = _articleBs.Edit(articleData);


            //insert UserMessage
            UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
            userM.Message = String.IsNullOrWhiteSpace(collect["ApproveMsg"]) ? null : collect["ApproveMsg"];
            userM.MessageType = "Approve 2";
            userM.ToUserId = userT.UserId;
            //UserM.FromUserId = Convert.ToInt32(Session["userid"]);
            userM.CreatedAt = DateTime.Now;
            userM.ToEmployeeId = userT.EmployeeId;
            userM.FromEmployeeId = Spv2.Employee_Id;
            userM.FromUserType = Article.ArticleId;
            _userMessageBService.Add(userM, "Article");

            Dictionary<String, EmployeeMaster> ApprovalData = new Dictionary<string, EmployeeMaster>();
            Dictionary<String, String> Message = new Dictionary<String, String>();
            ApprovalData.Add("Approval1", Spv1);
            ApprovalData.Add("Approval2", Spv2);
            Message.Add("Approval1", _userMessageBService.GetDetail(Spv1.Employee_Id, Article.ArticleId, "Approve 1 - Article").Message);
            Message.Add("Approval2", userM.Message);
            
            
            Email.SendMailResAppArticle(Article, Spv2, userT, collect["ApproveMsg"]);
            Email.SendMailNeedAppTsArticle(Article, ApprovalData, TsManager ,userT, Message );
            return RedirectToAction("Index");
        }
        public ActionResult ApproveTsMail(string id)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            var articleData = _articleBs.GetDetailbyToken(id);
            var userData = _userBService.GetDetail(articleData.CreatedBy);
            var app1 = _mstEmployeeBService.GetDetail(articleData.Aproved1By);
            var app2 = _mstEmployeeBService.GetDetail(articleData.Aproved2By);
            ViewBag.App1 = app1;
            ViewBag.App2 = app2;
            ViewBag.TsManager = _userTsManagerBusinessService.GetDetail(1);
            ViewBag.User = userData;
            ViewBag.RoleLevel = _userRoleBService.GetDetail(userData.RoleId).Name;
            ViewBag.MessageResponse1 = _userMessageBService.GetDetail(app1.Employee_Id,  articleData.ArticleId,  "Article");
            ViewBag.MessageResponse2 = _userMessageBService.GetDetail(app2.Employee_Id,  articleData.ArticleId,  "Article");
            ViewBag.DomainLink = WebConfigure.GetDomain();
            ViewBag.RecentArticle = _articleBs.GetRecent();
            ViewBag.UseFullLink = _articleBs.getUseFullLink();
            return View(articleData);
        }

        [HttpPost]
        public ActionResult ApproveTsMail(FormCollection collect)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            string token = Common.AccessToken() + Convert.ToInt32(collect["ArticleId"]);
            token = token.Replace("/", "");
            token = token.Replace("+", "");
            var articleData = _articleBs.GetDetail(Convert.ToInt32(collect["ArticleId"]));
            var userT = _userBService.GetByEmployeeId(Convert.ToInt32(collect["EmployeeId"]));
            var Spv2 = _mstEmployeeBService.GetDetail(Convert.ToInt32(collect["EmployeeSPVId"]));
            var Spv1 = _mstEmployeeBService.GetDetail(articleData.Aproved1By);
            var TsManager = _userTsManagerBusinessService.GetDetail(1);

            articleData.Token = token;
            articleData.Aproved2By = Spv2.Employee_Id;
            articleData.Aproved2At = DateTime.Now;
            articleData.Status = 9;
            var Article = _articleBs.Edit(articleData);


            //insert UserMessage
            UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
            userM.Message = String.IsNullOrWhiteSpace(collect["ApproveMsg"]) ? null : collect["ApproveMsg"];
            userM.MessageType = "Approve 2";
            userM.ToUserId = userT.UserId;
            //UserM.FromUserId = Convert.ToInt32(Session["userid"]);
            userM.CreatedAt = DateTime.Now;
            userM.ToEmployeeId = userT.EmployeeId;
            userM.FromEmployeeId = TsManager.EmployeeId;
            userM.FromUserType = Article.ArticleId;
            _userMessageBService.Add(userM, "Article");

            Dictionary<String, EmployeeMaster> ApprovalData = new Dictionary<string, EmployeeMaster>();
            Dictionary<String, String> Message = new Dictionary<String, String>();
            ApprovalData.Add("Approval1", Spv1);
            ApprovalData.Add("Approval2", Spv2);
            ApprovalData.Add("ApprovalTS", _mstEmployeeBService.GetDetail(TsManager.EmployeeId));
            
            Message.Add("Approval1", _userMessageBService.GetDetail(Spv1.Employee_Id, Article.ArticleId,  "Article").Message);
            Message.Add("Approval2", _userMessageBService.GetDetail(Spv2.Employee_Id, Article.ArticleId, "Article").Message);
            Message.Add("ApprovalTS", collect["ApproveMsg"]);

            Email.SendMailResAppArticle(Article, Spv2, userT, collect["ApproveMsg"]);
            Email.SendMailNeedSubmitedAdminArticle(Article, ApprovalData, userT, Message);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RejectTs(FormCollection collect)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            string token = Common.AccessToken() + Convert.ToInt32(collect["ArticleId"]);
            token = token.Replace("/", "");
            token = token.Replace("+", "");

            var employeeM = _mstEmployeeBService.GetDetail(Convert.ToInt32(collect["EmployeeSPVId"]));
            var employeeM2 = _mstEmployeeBService.GetDetail(Convert.ToInt32(collect["EmployeeSPV2Id"]));
            var TsManager = _mstEmployeeBService.GetDetail(Convert.ToInt32(collect["EmployeeTsManagerId"]));
            var userT = _userBService.GetByEmployeeId(Convert.ToInt32(collect["EmployeeId"]));
            var articleData = _articleBs.GetDetail(Convert.ToInt32(collect["ArticleId"]));

            articleData.Aproved1By = employeeM.Employee_Id;
            articleData.Aproved1At = DateTime.Now;
            articleData.Status = 10;
            articleData.Token = token;
            var Article = _articleBs.Edit(articleData);

            //insert UserMessage
            UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
            userM.Message = String.IsNullOrWhiteSpace(collect["RejectMsg"]) ? null : collect["RejectMsg"];
            userM.MessageType = "Reject 2";
            userM.ToUserId = userT.UserId;
            //UserM.FromUserId = Convert.ToInt32(Session["userid"]);
            userM.CreatedAt = DateTime.Now;
            userM.ToEmployeeId = userT.EmployeeId;
            userM.FromEmployeeId = TsManager.Employee_Id;
            userM.FromUserType = Article.ArticleId;
            _userMessageBService.Add(userM, "Article");

            Email.SendMailResAppArticle(Article, employeeM, userT, collect["RejectMsg"], false);
            Email.SendMailRejectArticle(Article, TsManager, userT, collect["RejectMsg"]);
            return RedirectToAction("Index");
        }
      
        [HttpPost]
        public ActionResult Reject2(FormCollection collect)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            string token = Common.AccessToken() + Convert.ToInt32(collect["ArticleId"]);
            token = token.Replace("/", "");
            token = token.Replace("+", "");

            var employeeM = _mstEmployeeBService.GetDetail(Convert.ToInt32(collect["EmployeeSPVId"]));
            var employeeM2 = _mstEmployeeBService.GetDetail(Convert.ToInt32(collect["EmployeeSPV2Id"]));
            var userT = _userBService.GetByEmployeeId(Convert.ToInt32(collect["EmployeeId"]));
            var articleData = _articleBs.GetDetail(Convert.ToInt32(collect["ArticleId"]));

            articleData.Aproved1By = employeeM.Employee_Id;
            articleData.Aproved1At = DateTime.Now;
            articleData.Status = 8;
            articleData.Token = token;
            var Article = _articleBs.Edit(articleData);

            //insert UserMessage
            UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
            userM.Message = String.IsNullOrWhiteSpace(collect["RejectMsg"]) ? null : collect["RejectMsg"];
            userM.MessageType = "Reject 2";
            userM.ToUserId = userT.UserId;
            //UserM.FromUserId = Convert.ToInt32(Session["userid"]);
            userM.CreatedAt = DateTime.Now;
            userM.ToEmployeeId = userT.EmployeeId;
            userM.FromEmployeeId = employeeM2.Employee_Id;
            userM.FromUserType = Article.ArticleId;
            _userMessageBService.Add(userM, "Article");

            Email.SendMailResAppArticle(Article, employeeM, userT, collect["RejectMsg"], false);
            Email.SendMailRejectArticle(Article, employeeM2, userT, collect["RejectMsg"]);
            return RedirectToAction("Index");
        }
        private Article ParsingEdit(Article artikel, FormCollection collect)
        {
            var articleOld = _articleBs.Detail(artikel.ArticleId);

            var articleReturn = articleOld;
            articleReturn.Position = 0;
            articleReturn.Title = artikel.Title;
            articleReturn.Description = artikel.Description;
            if (collect["type"] == "draft")
                articleReturn.Status =  2 ;
            else
            {
                articleReturn.Status = 0;
            }
            articleReturn.LevelUser = artikel.LevelUser;

            articleReturn.Category1Id = artikel.Category1Id;
            articleReturn.Category1 = _articleCategoryBs.GetDetail(artikel.Category1Id).Name ?? "-";

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
    }
}
