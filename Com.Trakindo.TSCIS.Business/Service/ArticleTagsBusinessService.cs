using System;
using System.Collections.Generic;
using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class ArticleTagsBusinessService
    {
        private readonly TsicsContext _tsicsContext = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        public ArticleTags Add(ArticleTags articleTag)
        {
            _tsicsContext.ArticleTags.Add(articleTag);
            _tsicsContext.SaveChanges();
            return articleTag;
        }

        public List<ArticleTags> GetTagsByArticle(int articleId)
        {
            return _tsicsContext.ArticleTags.Where(q => q.ArticleId == articleId).ToList();
        }
        public List<int> SearchArticleByTags(String Tag)
        {
            return _tsicsContext.ArticleTags.Where(q => q.Name.Contains(Tag)).Select(t => t.ArticleId).ToList();
        }

        public void Delete(int articleId, string name)
        {
            ArticleTags articleTags = _tsicsContext.ArticleTags.FirstOrDefault(q => q.ArticleId == articleId && q.Name == name);
            if(articleTags != null)
            {
                _tsicsContext.ArticleTags.Remove(articleTags);
                _tsicsContext.SaveChanges();
            }
        }

        public Boolean IsThisTagExists(int articleId, string name)
        {
            return _tsicsContext.ArticleTags.Where(q => q.ArticleId == articleId && q.Name == name).ToList().Count > 0;
        }
    }
}
