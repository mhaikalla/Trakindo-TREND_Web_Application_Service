using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Com.Trakindo.Framework
{
    public class BaseFormDataConverter : IFormDataConverter
    {
        protected object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        public virtual void PopulateData(object dataSource, object data)
        {
            if (dataSource != null && data != null)
            {
                foreach (PropertyInfo prop in dataSource.GetType().GetProperties())
                {
                    foreach (PropertyInfo prop2 in data.GetType().GetProperties())
                    {
                        var attribute = Attribute.GetCustomAttribute(prop2, typeof(KeyAttribute))
                         as KeyAttribute;
                        if (prop2.Name.Equals(prop.Name))
                        {
                            object value = prop.GetValue(dataSource, null);

                            int val = 0;

                            try
                            {
                                val = Convert.ToInt16(value);

                            }
                            catch
                            {
                                // ignored
                            }


                            if (attribute == null || (attribute != null && value != null && val != 0))
                            {
                                try
                                {
                                    if (value != null)
                                        prop2.SetValue(data, this.ChangeType(value, prop2.PropertyType), null);
                                }
                                catch
                                {
                                    // ignored
                                }
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="data"></param>
        public virtual void PopulateForm(object dataSource, object form)
        {
            if(dataSource != null && form != null)
            {
                foreach (PropertyInfo prop in dataSource.GetType().GetProperties())
                {
                    foreach (PropertyInfo prop2 in form.GetType().GetProperties())
                    {
                        if (prop2.Name.Equals(prop.Name))
                        {
                            object value = prop.GetValue(dataSource, null);
                            try
                            {
                                prop2.SetValue(form, Convert.ChangeType(value, prop2.PropertyType), null);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }
                }
            }
        }
    }
}
