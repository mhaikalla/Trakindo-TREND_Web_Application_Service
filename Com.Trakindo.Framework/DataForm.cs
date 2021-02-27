using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Com.Trakindo.Framework
{
    public class DataForm
    {
        public string BaseUrl { get; set; }

        public DataForm Clone()
        {
            DataForm newDataForm = (DataForm)Activator.CreateInstance(this.GetType());
            foreach(PropertyInfo prop in this.GetType().GetProperties())
            {
                newDataForm.GetType().GetProperty(prop.Name).SetValue(newDataForm, prop.GetValue(this, null), null);
            }
            return newDataForm;
        }

        public void Copy(DataForm form)
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                foreach (PropertyInfo prop2 in form.GetType().GetProperties())
                {

                    prop2.SetValue(form, prop.GetValue(this, null), null);
                }

            }
        }
    }
}
