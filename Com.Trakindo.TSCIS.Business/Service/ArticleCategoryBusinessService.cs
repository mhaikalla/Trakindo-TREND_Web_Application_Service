using System.Collections.Generic;
using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Data.Entity;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class ArticleCategoryBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
          
        public List<ArticleCategoryMostUsed> GetList(string search="") //Search Category
        {
            List<ArticleCategory> result = _dBtsics.ArticleCategory.Where(c => c.Name.Contains(search) && c.Status == 1)
                .ToList();
            List<ArticleCategoryMostUsed> FinalResult = new List<ArticleCategoryMostUsed>();
            foreach (var item in result)
            {
                ArticleCategoryMostUsed articlecategory = new ArticleCategoryMostUsed
                {
                    ArticleCategoryId = item.ArticleCategoryId,
                    Name = item.Name,
                    CreatedAt = item.CreatedAt,
                    Parent = item.Parent,
                    Icon = item.Icon,
                    Status = item.Status,
                    CountUsed = _dBtsics.Article.Where(c => c.Category1Id.Equals(item.ArticleCategoryId) || c.Category2Id.Equals(item.ArticleCategoryId) || c.Category3Id.Equals(item.ArticleCategoryId) || c.Category4Id.Equals(item.ArticleCategoryId) || c.Category5Id.Equals(item.ArticleCategoryId) || c.Category6Id.Equals(item.ArticleCategoryId) || c.Category7Id.Equals(item.ArticleCategoryId)).Count(),
                    Subcategory = _dBtsics.ArticleCategory.Where(p => p.Parent == item.ArticleCategoryId && p.Status == 1).Count(),
                    Position = item.Position
                   
                };
                FinalResult.Add(articlecategory);
            }
            FinalResult = FinalResult.OrderBy(c => c.Position).ToList();
            return FinalResult;
        }
        public List<ArticleCategory> GetListParent(int id)
        {
            List<ArticleCategory> result = _dBtsics.ArticleCategory
                .Where(p=>p.Parent == id && p.Status==1)
                .OrderBy(u => u.Position)
                .ToList();

            return result;
        }
        public List<ArticleCategory> GetListCategoryActive()
        {
            List<ArticleCategory> result = _dBtsics.ArticleCategory
                .Where(p => p.Status == 1)
                .ToList();
            return result;
        }
        public List<ArticleCategory> GetListMainCategory(int parent)
        {
            return _dBtsics.ArticleCategory
                .Where(p => p.Parent == parent && p.Status == 1).ToList();
        }
        public List<ArticleCategory> GetListParentforEdit(int id)
        {
            if(id == 0)
            {
                return null;
            }
            
            var result = GetDetail(GetDetail(id).Parent);
            if (result ==null)
            {
                return null;
            }
            return _dBtsics.ArticleCategory
                .Where(p => p.Parent == result.Parent && p.Status == 1).ToList();
        }
        public List<ArticleCategoryMostUsed> GetListCategoryMostUsedSort(int parent, string search = "")
        {
          var result = _dBtsics.ArticleCategory
                .Where(p => p.Parent == parent && p.Status == 1 && p.Name.Contains(search));
            List <ArticleCategoryMostUsed> FinalResult = new List<ArticleCategoryMostUsed>();
            foreach (var item in result)
            {
                ArticleCategoryMostUsed articlecategory = new ArticleCategoryMostUsed
                {
                    ArticleCategoryId = item.ArticleCategoryId,
                    Name = item.Name,
                    CreatedAt = item.CreatedAt,
                    Parent = item.Parent,
                    Icon = item.Icon,
                    Status = item.Status,
                    CountUsed = _dBtsics.Article.Where(c => c.Category1Id.Equals(item.ArticleCategoryId) || c.Category2Id.Equals(item.ArticleCategoryId) || c.Category3Id.Equals(item.ArticleCategoryId) || c.Category4Id.Equals(item.ArticleCategoryId) || c.Category5Id.Equals(item.ArticleCategoryId) || c.Category6Id.Equals(item.ArticleCategoryId) || c.Category7Id.Equals(item.ArticleCategoryId)).Count(),
                    Subcategory = _dBtsics.ArticleCategory.Where(p => p.Parent == item.ArticleCategoryId && p.Status == 1).Count(),
                    Position = item.Position
                };
                FinalResult.Add(articlecategory);
            }
            FinalResult = FinalResult.OrderBy(c => c.Position).ToList();
            return FinalResult;
       }
        public List<ArticleCategoryMostUsed> GetListCategoryMostUsedLiterature(int parent)
        {
            var result = _dBtsics.ArticleCategory
                  .Where(p => p.Parent == parent && p.Status == 1);
            List<ArticleCategoryMostUsed> FinalResult = new List<ArticleCategoryMostUsed>();
            foreach (var item in result)
            {
                ArticleCategoryMostUsed articlecategory = new ArticleCategoryMostUsed
                {
                    ArticleCategoryId = item.ArticleCategoryId,
                    Name = item.Name,
                    CreatedAt = item.CreatedAt,
                    Parent = item.Parent,
                    Icon = item.Icon,
                    Status = item.Status,
                    Position = item.Position,
                    CountUsed = _dBtsics.Article.Where(s => s.Type.Equals(1) && (s.Category1Id.Equals(item.ArticleCategoryId) && s.Category2Id == 0 && s.Category3Id == 0 && s.Category4Id == 0 && s.Category5Id == 0 && s.Category6Id == 0 && s.Category7Id == 0 ||
                s.Category2Id.Equals(item.ArticleCategoryId) && s.Category3Id == 0 && s.Category4Id == 0 && s.Category5Id == 0 && s.Category6Id == 0 && s.Category7Id == 0 ||
                s.Category3Id.Equals(item.ArticleCategoryId) && s.Category4Id == 0 && s.Category5Id == 0 && s.Category6Id == 0 && s.Category7Id == 0 ||
                s.Category4Id.Equals(item.ArticleCategoryId) && s.Category5Id == 0 && s.Category6Id == 0 && s.Category7Id == 0 ||
                s.Category5Id.Equals(item.ArticleCategoryId) && s.Category6Id == 0 && s.Category7Id == 0 ||
                s.Category6Id.Equals(item.ArticleCategoryId) && s.Category7Id == 0 ||
                s.Category7Id.Equals(item.ArticleCategoryId))).Where(s =>
                            s.Status == 1).Count()
                };
                FinalResult.Add(articlecategory);
            }
            FinalResult = FinalResult.OrderBy(c => c.Position).ToList();
            return FinalResult;
        }
        public ArticleCategory GetDetail(int id)
        {
            ArticleCategory result = _dBtsics.ArticleCategory
                .Where(p => p.Status == 1)
                .FirstOrDefault(i => i.ArticleCategoryId == id);

            return result;
        }
        public ArticleCategory GetDetailByPosition(int Position, int parent)
        {
            ArticleCategory result = _dBtsics.ArticleCategory
                .Where(i => i.Parent == parent && i.Position == Position && i.Status.Equals(1))
                .FirstOrDefault();
            return result;
        }
        public ArticleCategory Edit(ArticleCategory data)
        {
            _dBtsics.Entry(data).State = EntityState.Modified;
            _dBtsics.SaveChanges();
            return data;
        }
        public void SwapPosition(ArticleCategory Category1, ArticleCategory Category2)
        {
            _dBtsics.Database.ExecuteSqlCommand("UPDATE ArticleCategory SET Position = '" + Category2.Position +"' where ArticleCategoryId ='" + Category1.ArticleCategoryId+"'");
        }
        public void DeleteChild(int parent)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("UPDATE ArticleCategory set Status = 2 where Parent = " + parent);
            };        
           
        }
        public ArticleCategory Add(ArticleCategory data)
        {
            _dBtsics.ArticleCategory.Add(data);
            _dBtsics.SaveChanges();
            return data;
        }
        public void Delete(ArticleCategory data)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("Delete from ArticleCategory where Parent = " + data.Parent);
            }
                _dBtsics.Entry(data).State = EntityState.Deleted;
            _dBtsics.SaveChanges();
        }

        public int CountData(int Parent)
        {
            return _dBtsics.ArticleCategory.Count(c => c.Parent.Equals(Parent) && c.Status.Equals(1));
        }
    }
}
