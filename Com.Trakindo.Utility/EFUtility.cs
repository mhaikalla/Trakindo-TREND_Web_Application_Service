using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Com.Trakindo.Utility
{
    public class EFUtility
    {
        public static T UnProxy<T>(DbContext context, T proxyObject) where T : class
        {
            var proxyCreationEnabled = context.Configuration.ProxyCreationEnabled;
            try
            {
                context.Configuration.ProxyCreationEnabled = false;
                T poco = context.Entry(proxyObject).CurrentValues.ToObject() as T;
                return poco;
            }
            finally
            {
                context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
            }
        }

        public static T UnProxy<T>(object proxyObject) where T : class
        {
            DbContext context = (DbContext)proxyObject.GetType().GetProperty("DbContext").GetValue(proxyObject, null);
            if (context != null)
            {
                var proxyCreationEnabled = context.Configuration.ProxyCreationEnabled;
                try
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    T poco = context.Entry(proxyObject).CurrentValues.ToObject() as T;
                    return poco;
                }
                finally
                {
                    context.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
                }
            }
            return (T)proxyObject;
        }
    }
}
