using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Data.Entity;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Trakindo.Framework
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        DbContext context;

        public BaseRepository(String dataContextKey)
        {
            context = (DbContext)Factory.Create(dataContextKey, ClassType.clsTypeDataContext);
        }
        public virtual void Add(T instance)
        {
            context.Set<T>().Add((T)instance);
            context.SaveChanges();
        }


        public virtual void Delete(object id)
        {
            object instance = Get(id);
            context.Set<T>().Remove((T)instance);
            context.SaveChanges();
        }

        public virtual Int64 GetTotal(SelectParam selectParam)
        {
            Int64 total = 0;
            if (selectParam != null && selectParam.Where != null)
                total = context.Set<T>().Where(selectParam.Where).Count();
            else
                total = context.Set<T>().Count();
            return total;
        }

        public virtual List<T> FindAll(SelectParam selectParam, int? page, int? size)
        {
            List<T> items = null;

            if (selectParam != null && selectParam.OrderBy.Count == 0)
            {
                PropertyInfo prop = Trakindo.Utility.Assembly.GetKeyProperty(typeof(T));
                if (prop != null)
                    selectParam.AddOrderBy(prop.Name);
            }
            
            if (page == null || size == null)
            {
                if(selectParam != null && selectParam.Where != null)
                    items = context.Set<T>().Where(selectParam.Where).OrderBy( selectParam.OrderByString ).ToList<T>();
                else if (selectParam != null)
                    items = context.Set<T>().OrderBy( selectParam.OrderByString).ToList<T>();
                else
                    items = context.Set<T>().ToList<T>();
            }
            else
            {
                if (selectParam != null && selectParam.Where != null && selectParam.Where.Length > 0)
                    items = context.Set<T>().Where(selectParam.Where).OrderBy(selectParam.OrderByString).Skip((int)(page - 1) * (int)size).Take((int)size).ToList<T>();
                else if (selectParam != null)
                    items = context.Set<T>().OrderBy(selectParam.OrderByString).Skip((int)(page - 1) * (int)size).Take((int)size).ToList<T>();
                else
                    items = context.Set<T>().ToList<T>();
            }


            return items;
        }


        public virtual object Get(object id)
        {
            string fieldName = "";
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                if(prop.GetCustomAttributes(typeof(KeyAttribute), false)
                             .Cast<KeyAttribute>().FirstOrDefault() != null)
                {
                    fieldName = prop.Name;
                    break;
                }
            }
            IQueryable<T> result = context.Set<T>().Where(fieldName + " = " + id);
            T o = result.FirstOrDefault<T>();

            return o;
        }

        public void Update(T instance)
        {
            object id = null;
            PropertyInfo keyProperty = null;
            foreach (PropertyInfo prop in instance.GetType().GetProperties())
            {
                if (prop.GetCustomAttributes(typeof(KeyAttribute), false)
                             .Cast<KeyAttribute>().FirstOrDefault() != null)
                {
                    id = prop.GetValue(instance, null);
                    keyProperty = prop;
                    break;
                }
            }
            if (id != null)
            {
                
                var entity = Get(id);
                if (entity != null)
                {
                    context.Entry(entity).CurrentValues.SetValues(instance);
                    if (keyProperty != null)
                        context.Entry(entity).Property(keyProperty.Name).IsModified = false;
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Object doesn't exist", null);
                }
            }
            else
                throw new Exception("Object has no primary key value", null);
        }

    }
}
