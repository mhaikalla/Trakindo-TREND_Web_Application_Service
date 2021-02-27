using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Web.Mvc;
using TSICS.Helper;
using PagedList;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using System.Reflection;

namespace TSICS.Controllers
{
    public class LiterationController : Controller
    {
        private readonly ArticleBusinessService _articleBs = Factory.Create<ArticleBusinessService>("Article", ClassType.clsTypeBusinessService);
        private readonly ArticleFileBusinessService _articleFileBs = Factory.Create<ArticleFileBusinessService>("ArticleFile", ClassType.clsTypeBusinessService);
        private readonly LiteratureBusinessService _literatureService = Factory.Create<LiteratureBusinessService>("Literature", ClassType.clsTypeBusinessService);
        private readonly TicketCategoryBusinessService _TicketCategoryBs = Factory.Create<TicketCategoryBusinessService>("TicketCategory", ClassType.clsTypeBusinessService);
        private readonly ArticleTagsBusinessService _articleTagsBs = Factory.Create<ArticleTagsBusinessService>("ArticleTags", ClassType.clsTypeBusinessService);
        private readonly ArticleCategoryBusinessService _articleCategoryBs = Factory.Create<ArticleCategoryBusinessService>("ArticleCategory", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);

        private readonly LogErrorBusinessService _LogErrorBs = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);
        // GET: Library


        private Boolean isLocal =true;
        private String domain(Boolean isLocal = true)
        {
            if (isLocal == true)
            {
                return "http://localhost/TSICS";
            }
            else{
                return WebConfigure.GetDomain();
            }
        }

        public ActionResult Index(int? page, int Category = 0,String keyword = "")
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
            ViewBag.Domain = domain(isLocal);
            ViewBag.CategoryName = Category == 0 ? null : _articleCategoryBs.GetDetail(Category).Name;
            ViewBag.IconCategory = Category == 0 ? null : _articleCategoryBs.GetDetail(Category).Icon;
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
            List<Article> listArtikel = null ;
            //var start = 6 * (Convert.ToInt32(1) - 1);
            if (Category == 0)
            {
                listArtikel = _literatureService.GetListkeyword(keyword);
            }
            else
            {
               listArtikel = _literatureService.GetListbyCategory(Category,keyword);
            }

            #region Category

            ViewBag.Domain = WebConfigure.GetDomain();
            ViewBag.ListCategory1 = _articleCategoryBs.GetListCategoryMostUsedLiterature(0);
            ViewBag.key = keyword;
            #endregion

            List<ArticleFileData> ArticleData = new List<ArticleFileData>();
            List<CustomArticleTags> ArticleTagsData = new List<CustomArticleTags>();
            if (listArtikel != null)
            {
                foreach (var item in listArtikel)
                {
                    List<String> FileData = _articleFileBs.GetNamefileByRoleColor(item.ArticleId, item.LevelUser);
                    if(FileData.Count > 0)
                    {
                        ArticleFileData filedata = new ArticleFileData()
                        {
                            idFile = item.ArticleId,
                            Name =  FileData,
                            Level = _articleFileBs.GetLevelFileByRoleColor(item.ArticleId, item.LevelUser)
                        };
                        ArticleData.Add(filedata);
                    }
                    var TagsData = _articleTagsBs.GetTagsByArticle(item.ArticleId);
                    CustomArticleTags tags = new CustomArticleTags()
                    {
                          ArticleId = item.ArticleId,
                          Object = TagsData.Count == 0 ? null : TagsData
                    };
                    ArticleTagsData.Add(tags);
                }
            }
            ViewBag.LiteratureTags = ArticleTagsData;
            ViewBag.keyword = keyword;
            ViewBag.LiteratureItem = ArticleData;
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(listArtikel.ToPagedList(pageNumber, pageSize));
            
        }
        
        public ActionResult SaveFile(int id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
            {
                return RedirectToAction("Login", "Account");
            }

            List <ArticleFile> FileData = _articleFileBs.GetListByArtikelId(id);
            try
            {
                String FileName = "Trakindo TREND-Literature.zip";
                if (FileData.Count == 1)
                {
                    foreach (var data in FileData)
                    {
                        if (Session["roleColor"].ToString().ToLower().Contains("guest"))
                        {
                            if (data.LevelUser.ToLower().Contains("guest"))
                            {
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.ClearContent();
                                Response.Clear();
                                Response.ContentType = "application/msi";
                                Response.AppendHeader("Content-Disposition", "attachment; filename=" + data.Name);
                                Response.TransmitFile(Server.MapPath("~/Upload/Article/Attachments/") + data.Name);
                                Response.End();
                            }

                        }
                        else if (Session["roleColor"].ToString().ToLower().Contains("green"))
                        {
                            if (data.LevelUser.ToLower().Contains("guest") || data.LevelUser.ToLower().Contains("green"))
                            {
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.ClearContent();
                                Response.Clear();
                                Response.ContentType = "application/msi";
                                Response.AppendHeader("Content-Disposition", "attachment; filename=" + data.Name);
                                Response.TransmitFile(Server.MapPath("~/Upload/Article/Attachments/") + data.Name);
                                Response.End();
                            }
                        }
                        else if (Session["roleColor"].ToString().ToLower().Contains("yellow"))
                        {
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.ClearContent();
                                Response.Clear();
                                Response.ContentType = "application/msi";
                                Response.AppendHeader("Content-Disposition", "attachment; filename=" + data.Name);
                                Response.TransmitFile(Server.MapPath("~/Upload/Article/Attachments/") + data.Name);
                                Response.End(); 
                        }
                    }
                    return File(FileName, "application/msi");
                }
                else
                {
                    using (ZipOutputStream zipOutputStream = new ZipOutputStream(System.IO.File.Create(Server.MapPath("~/Upload/Article/Attachments/") + FileName)))
                    {
                        zipOutputStream.SetLevel(9);
                        byte[] buffer = new Byte[4096];
                        var DataList = new List<string>();
                        foreach (var datafile in FileData)
                        {
                            if (Session["roleColor"].ToString().ToLower() == "guest") {
                                if (datafile.LevelUser.ToLower() == "guest")
                                {
                                    DataList.Add(Server.MapPath("~/Upload/Article/Attachments/") + datafile.Name);
                                }
                                
                            }
                            else if (Session["roleColor"].ToString().ToLower() == "green")
                            {
                                if (datafile.LevelUser.ToLower() == "guest" || datafile.LevelUser.ToLower() == "green")
                                {
                                    DataList.Add(Server.MapPath("~/Upload/Article/Attachments/") + datafile.Name);
                                }
                            }
                            else if (Session["roleColor"].ToString().ToLower() == "yellow")
                            {
                                DataList.Add(Server.MapPath("~/Upload/Article/Attachments/") + datafile.Name);
                            }

                        }
                        for (int i = 0; i < DataList.Count; i++)
                        {
                            ZipEntry entry = new ZipEntry(Path.GetFileName(DataList[i]));
                            entry.DateTime = DateTime.Now;
                            entry.IsUnicodeText = true;
                            zipOutputStream.PutNextEntry(entry);

                            using (FileStream fileStream = System.IO.File.OpenRead(DataList[i]))
                            {
                                int sourcebytes;
                                do
                                {
                                    sourcebytes = fileStream.Read(buffer, 0, buffer.Length);
                                    zipOutputStream.Write(buffer, 0, sourcebytes);
                                } while (sourcebytes > 0);
                            }
                        }
                        zipOutputStream.Finish();
                        zipOutputStream.Flush();
                        zipOutputStream.Close();
                    }
                    var path = Server.MapPath("~/Upload/Article/Attachments/") + FileName;
                    byte[] finalresult = System.IO.File.ReadAllBytes(path);

                    return File(finalresult, "application/zip", FileName);
                }
            }
            catch (FileNotFoundException er)
            {
                
                _LogErrorBs.WriteLog("Download ZIP Literature", MethodBase.GetCurrentMethod().Name, er.ToString());
                return Content("<script language='javascript' type='text/javascript'>alert('File Literature is Not Found'); location.href = '" + WebConfigure.GetDomain() + "/Literation';</script>");
                //return RedirectToAction("Index", "Literation");
            }
        }
        public ActionResult FileNotFound()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Alert = "File Not Found";
            return View("Index", "Literation");
        }
           // GET: Library/Details/5
        public ActionResult Detail(int id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

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

            var articleModel = _literatureService.GetDetail(id);

            var listArticleFileModel = _articleFileBs.GetListByArtikelId(id);

            ViewBag.ListFile = listArticleFileModel.Count == 0 ? null : listArticleFileModel;

            ViewBag.UseFullLink = _articleBs.getUseFullLink();
            return View(articleModel);

        }
        
        [HttpPost]
        public JsonResult GetChild(int id)
        {
            return Json(new SelectList(_articleCategoryBs.GetListParent(id), "ArticleCategoryId", "Name"));
        }

      
        public string GetAuthorName(int id)
        {
            if (id == 0)
                return "Admin";
            return _userBService.GetDetail(id).Name;
        }

        [HttpGet]
        public ActionResult Category(string id)
        {
            ViewBag.category = id;
            return View();
        }

        [HttpGet]
        public ActionResult Testing(int parent =0 )
        {
            #region Category 
            ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
            ViewBag.Domain = WebConfigure.GetDomain();
            ViewBag.ListCategoryChild = _articleCategoryBs.GetListCategoryMostUsedLiterature(parent); /*_articleCategoryBs.GetListMainCategory(parent);*/
            #endregion
            return View();
        }
        [HttpGet]
        public ActionResult LiteratureTemplate(int? page, int Category = 0,String keyword = "")
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
            ViewBag.Domain = domain(isLocal);
            ViewBag.CategoryName = Category == 0 ? null : _articleCategoryBs.GetDetail(Category).Name;
            ViewBag.IconCategory = Category == 0 ? null : _articleCategoryBs.GetDetail(Category).Icon;
           
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            List<Article> listArtikel = null;
            if (Category == 0)
            {
                listArtikel = _literatureService.GetListkeyword(keyword);
            }
            else
            {
               listArtikel = _literatureService.GetListbyCategory(Category,keyword);
            }


            #region Category
            ViewBag.Domain = domain(isLocal);
            ViewBag.ListCategory1 = _articleCategoryBs.GetListCategoryMostUsedLiterature(0);
            #endregion

            List<ArticleFile> ArticleData = new List<ArticleFile>();
            List<CustomArticleTags> ArticleTagsData = new List<CustomArticleTags>();
            if (listArtikel != null)
            {
                foreach (var artikel in listArtikel)
                {
                    var FileData = _articleFileBs.GetListByArtikelId(artikel.ArticleId);
                    if (FileData != null)
                    {
                        foreach (var FileItem in FileData)
                        {
                            var fileCek = _articleFileBs.GetDetail(FileItem.ArticleId);
                            if (fileCek != null)
                            {
                                ArticleFile file = new ArticleFile()
                                {
                                    ArticleId = artikel.ArticleId,
                                    Name = fileCek.Name,

                                };
                                ArticleData.Add(file);
                            }
                            var TagsData = _articleTagsBs.GetTagsByArticle(artikel.ArticleId);
                            CustomArticleTags tags = new CustomArticleTags()
                            {
                                ArticleId = artikel.ArticleId,
                                Object = TagsData.Count == 0 ? null : TagsData
                            };
                            ArticleTagsData.Add(tags);
                        }
                    }
                }
            }
            ViewBag.LiteratureTags = ArticleTagsData;
            ViewBag.LiteratureData = listArtikel;
            ViewBag.LiteratureItem = ArticleData;
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(listArtikel.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult LoadMore(int? page, int Category = 0,String keyword = "")
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
            ViewBag.CategoryName = Category == 0 ? null : _articleCategoryBs.GetDetail(Category).Name;
            ViewBag.IconCategory = Category == 0 ? null : _articleCategoryBs.GetDetail(Category).Icon;

            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            List<Article> listArtikel = null;
            if (Category == 0)
            {
                listArtikel = _literatureService.GetListkeyword(keyword);
            }
            else
            {
                listArtikel = _literatureService.GetListbyCategory(Category, keyword);
            }
            #region Category
            ViewBag.Domain = domain(isLocal);
            ViewBag.ListCategory1 = _articleCategoryBs.GetListCategoryMostUsedLiterature(0);
            #endregion

            List<ArticleFile> ArticleData = new List<ArticleFile>();
            List<CustomArticleTags> ArticleTagsData = new List<CustomArticleTags>();
            if (listArtikel != null)
            {
                foreach (var artikel in listArtikel)
                {
                    var FileData = _articleFileBs.GetListByArtikelId(artikel.ArticleId);
                    if (FileData != null)
                    {
                        foreach (var FileItem in FileData)
                        {
                            var fileCek = _articleFileBs.GetDetail(FileItem.ArticleId);
                            if (fileCek != null)
                            {
                                ArticleFile file = new ArticleFile()
                                {
                                    ArticleId = artikel.ArticleId,
                                    Name = fileCek.Name,

                                };
                                ArticleData.Add(file);
                            }
                            var TagsData = _articleTagsBs.GetTagsByArticle(artikel.ArticleId);
                            CustomArticleTags tags = new CustomArticleTags()
                            {
                                ArticleId = artikel.ArticleId,
                                Object = TagsData.Count == 0 ? null : TagsData
                            };
                            ArticleTagsData.Add(tags);
                        }
                    }
                }
            }
            ViewBag.LiteratureTags = ArticleTagsData;
            ViewBag.LiteratureData = listArtikel;
            ViewBag.LiteratureItem = ArticleData;
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(listArtikel.ToPagedList(pageNumber, pageSize));
        }
    }
}
