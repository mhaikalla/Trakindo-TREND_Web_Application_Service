using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Data.Entity;
using Com.Trakindo.Utility;
using Com.Trakindo.Framework;
using System.Linq.Expressions;

namespace Com.Trakindo.Framework
{
    public abstract class BaseBusinessService<T>  : IBusinessService<T>
    {
        protected IRepository<T> repo;

        public BaseBusinessService(string repoKey)
        {
            repo = (IRepository<T>)Factory.Create(repoKey, ClassType.clsTypeRepository);
        }

        public virtual OperationResult FindAll(SelectParam selectParam, int? page, int? size)
        {
            if (page == 0)
                page = null;

            if (size == 0)
                size = null;
            try
            {
                if(selectParam != null)
                    selectParam = this.SetAdditionalParameter(selectParam);
                List<T> items = repo.FindAll(selectParam, page, size);
                return new OperationResult(true, items);
            }
            catch (Exception e)
            {
                return new OperationResult(false, null, e.Message);
            }
        }

        public virtual OperationResult FindAllByKeyword(SelectParam selectParam, int? page, int? size)
        {
            string where = GetPredicateByKeyword(selectParam.Keyword);
            selectParam.Where = where;
            return this.FindAll(selectParam, page, size);
        }

        protected virtual String GetPredicateByKeyword(string keyword)
        {
            return keyword;
        }

        protected virtual SelectParam SetAdditionalParameter(SelectParam selectParam)
        {
            return selectParam;
        }

        public virtual OperationResult Get(object id)
        {
            object o = null;
            try
            {
                o = repo.Get(id);
                return new OperationResult(true, o);
            }
            catch (Exception e)
            {
                return new OperationResult(false, e, e.Message);
            }
        }


        public virtual OperationResult Add(T o)
        {
            try
            {
                repo.Add(o);
                return new OperationResult(true, o);
            }
            catch (Exception e)
            {
                return new OperationResult(false, e, e.Message);
            }
        }

        public virtual OperationResult Update(T o)
        {
            try
            {
                repo.Update(o);
                return new OperationResult(true, o);
            }
            catch (Exception e)
            {
                return new OperationResult(false, e, e.Message);
            }
        }

        public virtual OperationResult Delete(object o)
        {
            try
            {
                repo.Delete(o);
                return new OperationResult(true, o);
            }
            catch (Exception e)
            {
                return new OperationResult(false, e, e.Message);
            }
        }

        public virtual OperationResult GetTotal(SelectParam selectParam)
        {
            try
            {
                Int64 total = repo.GetTotal(selectParam);
                return new OperationResult(true, total);
            }
            catch (Exception e)
            {
                return new OperationResult(false, e, e.Message);
            }
        }
    }
}
