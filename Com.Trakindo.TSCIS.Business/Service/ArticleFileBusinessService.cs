using System.Collections.Generic;
using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class ArticleFileBusinessService
    {
        private readonly TsicsContext _dBtsicsContext = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public List<ArticleFile> GetList()
        {
            List<ArticleFile> result = _dBtsicsContext.ArticleFile
                .ToList();

            return result;
        }

        public ArticleFile Add(ArticleFile article)
        {
            _dBtsicsContext.ArticleFile.Add(article);
            _dBtsicsContext.SaveChanges();
            return article;
        }

        public List<ArticleFile> GetListByArtikelId(int aid)
        {
            List<ArticleFile> result = _dBtsicsContext.ArticleFile
                .Where(d => d.ArticleId==aid)
                .ToList();

            return result;
        }

        public ArticleFile GetDetail(int Articleid)
        {
            return _dBtsicsContext.ArticleFile.Where(a => a.ArticleFileId.Equals(Articleid)).FirstOrDefault();
        }
        public int Delete(ArticleFile fileData)
        {
            _dBtsicsContext.ArticleFile.Remove(fileData);
            var deleteFile = _dBtsicsContext.SaveChanges();
            return deleteFile;
        }
        public void DeleteAll(int ArticleId)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("delete from ArticleFile where ArticleId = " + ArticleId);
            }
        }
        public List<string> GetNamefileByRoleColor(int articleid, string role = "")
        {
            if (role == "" || role == null)
            {
                role = "Guest";
            }
            
            return _dBtsicsContext.ArticleFile.Where(s => s.LevelUser.Contains(role) && s.ArticleFileId.Equals(articleid)).Select(a => a.Name).ToList();
        }
        public List<string> GetLevelFileByRoleColor(int articleid, string role = "")
        {
            if (role == "" || role == null)
            {
                role = "Guest";
            }
            return _dBtsicsContext.ArticleFile.Where(s => s.LevelUser.Contains(role) && s.ArticleFileId.Equals(articleid)).Select(a => a.LevelUser).ToList();
        }
    }
}
