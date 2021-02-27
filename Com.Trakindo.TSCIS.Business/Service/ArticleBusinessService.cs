using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{

    public class ArticleBusinessService
    {
        private readonly TsicsContext _dBtsicsContext = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        private readonly ArticleTagsBusinessService _articleTagsBs = Factory.Create<ArticleTagsBusinessService>("ArticleTags", ClassType.clsTypeBusinessService);
            
        public List<Article> GetList()
        {
            int[] tampilkan = { 0,1,3,5,6,7,8,9,10 };

            List<Article> result = _dBtsicsContext.Article
                .OrderByDescending(a => a.CreatedAt)
                .Where(s => tampilkan.Contains(s.Status) && s.Type.Equals(0))
                .Distinct()
                .ToList();

            return result;
        }
        public List<Article> GetMyList(int CreatedBy)
        {
            int[] tampilkan = { 0, 1, 3, 5, 6, 7, 8, 9, 10 };

            List<Article> result = _dBtsicsContext.Article
                .OrderByDescending(a => a.CreatedAt)
                .Where(s => tampilkan.Contains(s.Status) && s.Type.Equals(0) && s.CreatedBy.Equals(CreatedBy))
                .Distinct()
                .ToList();

            return result;
        }
        public List<Article> GetMyDraft(int CreatedBy)
        {
            int[] tampilkan = { 2 };

            List<Article> result = _dBtsicsContext.Article
                .OrderByDescending(a => a.CreatedAt)
                .Where(s => tampilkan.Contains(s.Status) && s.Type.Equals(0) && s.CreatedBy.Equals(CreatedBy))
                .Distinct()
                .ToList();

            return result;
        }
        public List<Article> GetListDraft()
        {
            int[] tampilkan = { 2 };

            List<Article> result = _dBtsicsContext.Article
                .OrderByDescending(a => a.AprovedAdminAt)
                .Where(s => tampilkan.Contains(s.Status) & s.Type.Equals(0))
                .ToList();

            return result;
        }

        public Article Add(Article article)
        {
            article.Type = 0;
            _dBtsicsContext.Article.Add(article);
            _dBtsicsContext.SaveChanges();
            return article;
        }

        public Article Detail(int id)
        {
            Article result = _dBtsicsContext.Article
                .Find(id);

            return result;
        }

        public Article GetDetail(int id)
        {
            Article result = _dBtsicsContext.Article.SingleOrDefault(s => s.ArticleId.Equals(id) && s.Type.Equals(0));

            return result;
        }
        public Article GetDetailbyToken(String id)
        {
            Article result = _dBtsicsContext.Article.SingleOrDefault(s => s.Token.Equals(id) && s.Type.Equals(0));

            return result;
        }
        public Article Approved(int ida)
        {
            int[] stat = { 0,1,2,3,5,6,7,8,9,10};
            Article result = _dBtsicsContext.Article
                .FirstOrDefault(i => i.ArticleId == ida && stat.Contains(i.Status) && i.Type.Equals(0));

            return result;
        }
        public List<Article> GetListApproved(int start, string level, string category = null, string keyword = null)
        {
            List<Article> result = new List<Article>();

            int[] ListArticleTags = _articleTagsBs.SearchArticleByTags(keyword).ToArray();
            switch (level.ToLower())
            {
                case "guest":
                        result = _dBtsicsContext.Article
                           .Where(s => s.Status == 1 && s.LevelUser == level && s.Type.Equals(0))
                           .OrderByDescending(i => i.ArticleId)
                           .Skip(start)
                           .Take(6)
                           .Distinct()
                           .ToList(); 
                    break;
                case "green":
                        result = _dBtsicsContext.Article
                           .Where(s => s.Status == 1 && s.LevelUser.ToLower() == level || s.Status == 1 || s.LevelUser.ToLower() == "guest")
                           .Where(s => s.Type.Equals(0))
                           .OrderByDescending(i => i.ArticleId)
                           .Skip(start)
                           .Take(6)
                           .Distinct()
                           .ToList();   
                    break;
                case "yellow":
                    result = _dBtsicsContext.Article
                        .Where(s => s.Status == 1 && s.Type.Equals(0))
                        .OrderByDescending(i => i.ArticleId)
                        .Skip(start)
                        .Take(6)
                        .Distinct()
                        .ToList();
                    break;
            }
            if (!String.IsNullOrWhiteSpace(category))
            {
                result = result.Where(Category => Category.Category1Id == Convert.ToInt32(category)).ToList();
            }
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                if(ListArticleTags.Length > 0)
                {
                    result = result.Where(Category => Category.Title.ToLower().Contains(keyword.ToLower()) || Category.Description.ToLower().Contains(keyword.ToLower()) || ListArticleTags.Contains(Category.ArticleId)).ToList();
                }
                else
                {
                    result = result.Where(Category => Category.Title.ToLower().Contains(keyword.ToLower()) || Category.Description.ToLower().Contains(keyword.ToLower())).ToList();
                }
               
            }
            return result;
        }
        public List<Article> GetListCategory(int start, string level, string id)
        {
            List<Article> result = new List<Article>();
            switch (level.ToLower())
            {
                case "guest":
                    result = _dBtsicsContext.Article
                        .Where(s =>
                            s.Status == 1 && s.Category1.Contains(id) && s.LevelUser.ToLower() == level && s.Type.Equals(0)
                        )
                        .OrderByDescending(i => i.ArticleId)
                        .Skip(start)
                        .Take(3)
                        .ToList();
                    break;
                case "green":
                    result = _dBtsicsContext.Article
                        .Where(s =>
                            s.Status == 1 && s.Category1.Contains(id) && s.LevelUser.ToLower() == level ||
                            s.Status == 1 && s.Category1.Contains(id) && s.LevelUser.ToLower() == "guest"
                        )
                        .OrderByDescending(i => i.ArticleId)
                        .Skip(start)
                        .Take(3)
                        .ToList();
                    break;
                case "yellow":
                    result = _dBtsicsContext.Article
                        .Where(s =>
                            s.Status == 1 && s.Category1.Contains(id)
                        )
                        .OrderByDescending(i => i.ArticleId)
                        .Skip(start)
                        .Take(3)
                        .ToList();
                    break;
            }

            return result;
        }

        public List<Article> GetRecent()
        {
            return _dBtsicsContext.Article.Where(q => q.Status == 1 && q.Type.Equals(0)).OrderByDescending(q => q.CreatedAt).Take(5).ToList();
        }
        public Article Edit(Article article)
        {
            _dBtsicsContext.Entry(article).State = EntityState.Modified;
            _dBtsicsContext.SaveChanges();
            return article;
        }
        public List<Article> GetListDraft(int idUser)
        {
            int[] tampilkan = { 2};

            List<Article> result = _dBtsicsContext.Article
                .OrderByDescending(a => a.AprovedAdminAt)
                .Where(u => u.CreatedBy == idUser)
                .Where(s => tampilkan.Contains(s.Status) && s.Type.Equals(0))
                .ToList();

            return result;
        }
        public List<Article> GetListApprovedByCategory(int start, string category)
        { 
            List<Article> result = _dBtsicsContext.Article 
                .Where(s => s.Status == 1 && s.Type.Equals(0))
                .Where(c => category.Contains(c.Category1))
                .OrderByDescending(i => i.ArticleId)
                .Skip(start)
                .Take(3)
                .ToList();

            return result;
        }
		public List<Article> GetSearchData(int start, String keyword, String level)
        {
            List<Article> result = new List<Article>();
            switch (level.ToLower())
            {
                case "guset":
                    result = _dBtsicsContext.Article
                        .Where(s =>
                            s.Status == 1 && s.Title.Contains(keyword) && s.LevelUser.ToLower() == level ||
                            s.Status == 1 && s.Description.Contains(keyword) && s.LevelUser.ToLower() == level
                            )
                            .Where(s => s.Type.Equals(0))
                        .OrderByDescending(i => i.ArticleId)
                        .Skip(start)
                        .Take(3)
                        .Distinct()
                        .ToList();
                    break;
                case "green":
                    result = _dBtsicsContext.Article
                        .Where(s =>
                            s.Status == 1 && s.Title.Contains(keyword) && s.LevelUser.ToLower() == level ||
                            s.Status == 1 && s.Description.Contains(keyword) && s.LevelUser.ToLower() == level ||
                            s.Status == 1 && s.Title.Contains(keyword) && s.LevelUser.ToLower() == "guest" ||
                            s.Status == 1 && s.Description.Contains(keyword) && s.LevelUser.ToLower() == "guest"
                            ).Where(s => s.Type.Equals(0))
                        .OrderByDescending(i => i.ArticleId)
                        .Skip(start)
                        .Take(3)
                        .Distinct()
                        .ToList();
                    break;
                case "yellow":
                    result = _dBtsicsContext.Article
                        .Where(s =>
                            s.Status == 1 && s.Title.Contains(keyword) ||
                            s.Status == 1 && s.Description.Contains(keyword)
                            ).Where(s => s.Type.Equals(0))
                        .OrderByDescending(i => i.ArticleId)
                        .Skip(start)
                        .Take(3)
                        .Distinct()
                        .ToList();
                    break;
            }
            
            return result;
        }
        public Article AddUseFullLink(Article article)
        {
            using (TsicsContext db = new TsicsContext())
            {
                
                _dBtsicsContext.Article.Add(article);
                _dBtsicsContext.SaveChanges();
                return article;
            }
            
        }
        public List<Article> getUseFullLink()
        {
            return _dBtsicsContext.Article.Where(a => a.Type.Equals(2))
                .OrderByDescending(a => a.CreatedAt)
                .ToList();
        }
        public Article getDetailUseFullLink(int id)
        {
            return _dBtsicsContext.Article.FirstOrDefault(l => l.ArticleId.Equals(id));
        }
        public void DeleteUseFullLink(int id)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("delete from Article where Article.ArticleId = "+ id);
            }   
        }
        public Article EditUseFullLink(Article Link)
        {
            _dBtsicsContext.Entry(Link).State = EntityState.Modified;
            _dBtsicsContext.SaveChanges();
            return Link;
        }
    }
}
