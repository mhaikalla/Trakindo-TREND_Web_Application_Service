using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Trakindo.Utility
{
    public class ListUtility
    {

        public interface IListProcessor
        {
            string Process(IDictionary<string, object> list);
        }

        public class Parameters
        {
            Dictionary<string, object> parameterList = new Dictionary<string, object>();
            public Parameters Add(string key, string value)
            {
                this.parameterList.Add(key, value);
                return this;
            }
            public Parameters Remove(string key)
            {
                this.parameterList.Remove(key);
                return this;
            }
            public object Get(string key)
            {
                if (this.parameterList.ContainsKey(key))
                    return this.parameterList[key];
                return null;
            }
            public string Convert(IListProcessor processor)
            {
                return processor.Process(this.parameterList);
            }

        }
    }
}
