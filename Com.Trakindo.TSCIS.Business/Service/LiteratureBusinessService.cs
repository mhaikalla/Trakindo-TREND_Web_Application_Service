using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class LiteratureBusinessService
    {
        private readonly TsicsContext _dBtsicsContext = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
         private readonly ArticleTagsBusinessService _articleTagsBs = Factory.Create<ArticleTagsBusinessService>("ArticleTags", ClassType.clsTypeBusinessService);
               
        public List<Article> GetList()
        {
            int[] stat = { 1 };
            List<Article> result = new List<Article>();

            return _dBtsicsContext.Article
                .OrderByDescending(a => a.CreatedAt)
                .Where(s => stat.Contains(s.Status) && s.Type.Equals(1))
                .ToList();
        }
        public List<Article> GetListkeyword(string keyword = null)
        {
            int[] stat = { 1 };
            
            List<Article> result = new List<Article>();
           
             result = _dBtsicsContext.Article
                .Where(s =>
                            s.Status == 1 && s.Title.Contains(keyword) ||
                            s.Status == 1 && s.Description.Contains(keyword)
                            ).Where(s => s.Type.Equals(1)).OrderByDescending(a => a.CreatedAt).ToList();
         
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                int[] ListArticleTags = _articleTagsBs.SearchArticleByTags(keyword).ToArray();
                if(ListArticleTags.Length > 0)
                {
                    result = result.Where(Category => Category.Title.ToLower().Contains(keyword.ToLower()) || Category.Description.ToLower().Contains(keyword.ToLower()) || ListArticleTags.Contains(Category.ArticleId)).OrderByDescending(a => a.CreatedAt).ToList();
                }
                else
                {
                    result = result.Where(Category => Category.Title.ToLower().Contains(keyword.ToLower()) || Category.Description.ToLower().Contains(keyword.ToLower())).OrderByDescending(a => a.CreatedAt).ToList();
                }
               
            }
            return result;
        }
        public List<Article> GetListPage(int skip)
        {
            int[] stat = { 1 };
            
            return _dBtsicsContext.Article
                .OrderByDescending(a => a.CreatedAt)
                .Where(s => stat.Contains(s.Status) && s.Type.Equals(1))
                .Skip(skip)
                .Take(6)
                .Distinct()
                .ToList();
        }
        public List<Article> GetListbyCategory(int category,string keyword = null)
        {
            int[] stat = { 1 };
             List<Article> result = new List<Article>();
            
             result = _dBtsicsContext.Article
                .Where(s => (s.Status == 1 
                && s.Type.Equals(1)) &&
                (s.Title.Contains(keyword) || s.Description.Contains(keyword)) &&
                (s.Category1Id.Equals(category) && s.Category2Id == 0 && s.Category3Id == 0 && s.Category4Id == 0 && s.Category5Id == 0 && s.Category6Id == 0 && s.Category7Id == 0 || 
                s.Category2Id.Equals(category) && s.Category3Id == 0 && s.Category4Id == 0 && s.Category5Id == 0 && s.Category6Id == 0 && s.Category7Id == 0 ||
                s.Category3Id.Equals(category) && s.Category4Id == 0 && s.Category5Id == 0 && s.Category6Id == 0 && s.Category7Id == 0 ||
                s.Category4Id.Equals(category) && s.Category5Id == 0 && s.Category6Id == 0 && s.Category7Id == 0 ||
                s.Category5Id.Equals(category) && s.Category6Id == 0 && s.Category7Id == 0 ||
                s.Category6Id.Equals(category) && s.Category7Id == 0 ||
                s.Category7Id.Equals(category) ))
                 .OrderByDescending(a => a.CreatedAt).ToList();
          
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                int[] ListArticleTags = _articleTagsBs.SearchArticleByTags(keyword).ToArray();
                if(ListArticleTags.Length > 0)
                {
                    result = result.Where(Category => ListArticleTags.Contains(Category.ArticleId)).OrderByDescending(a => a.CreatedAt).ToList();
                }
                
               
            }   
            return result;
        }

        public List<Article> GetListDraft()
        {
            int[] tampilkan = { 2 };

            List<Article> result = _dBtsicsContext.Article
                .OrderByDescending(a => a.AprovedAdminAt)
                .Where(s => tampilkan.Contains(s.Status) && s.Type.Equals(1))
                .ToList();

            return result;
        }

        public Article Add(Article article)
        {
            _dBtsicsContext.Article.Add(article);
            _dBtsicsContext.SaveChanges();
            return article;
        }
        
        public Article GetDetail(int id)
        {
            Article result = _dBtsicsContext.Article.SingleOrDefault(s => s.ArticleId.Equals(id) && s.Type.Equals(1));

            return result;
        }

        public List<Article> GetRecent()
        {
            return _dBtsicsContext.Article.Where(q => q.Status == 1).Where(s => s.Type.Equals(1)).OrderByDescending(q => q.CreatedAt).Take(5).ToList();
        }
        public Article Edit(Article article)
        {
            _dBtsicsContext.Entry(article).State = EntityState.Modified;
            _dBtsicsContext.SaveChanges();
            return article;
        }
        public List<Article> GetListDraft(int idUser)
        {
            int[] tampilkan = { 2 };

            List<Article> result = _dBtsicsContext.Article
                .OrderByDescending(a => a.AprovedAdminAt)
                .Where(u => u.CreatedBy == idUser)
                .Where(s => tampilkan.Contains(s.Status) && s.Type.Equals(1))
                .ToList();

            return result;
        }
       
        public List<Article> GetSearchData(int start, String keyword, String level)
        {
            List<Article> result = new List<Article>();
            switch (level.ToLower())
            {
                case "guest":
                    result = _dBtsicsContext.Article
                        .Where(s =>
                            s.Status == 1 && s.Title.Contains(keyword) && s.LevelUser.ToLower() == level ||
                            s.Status == 1 && s.Description.Contains(keyword) && s.LevelUser.ToLower() == level
                            ).Where(s=> s.Type.Equals(1))
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
                            ).Where(s => s.Type.Equals(1))
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
                            ).Where(s => s.Type.Equals(1))
                        .OrderByDescending(i => i.ArticleId)
                        .Skip(start)
                        .Take(3)
                        .Distinct()
                        .ToList();
                    break;
            }

            return result;
        }
    }
}
