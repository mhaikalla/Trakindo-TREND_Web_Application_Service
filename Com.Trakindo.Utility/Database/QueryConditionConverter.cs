using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Trakindo.Utility.Database
{
    public class QueryConditionAndConverter : Com.Trakindo.Utility.ListUtility.IListProcessor
    {
        public string Process(IDictionary<string, object> list)
        {
            string where = "";
            foreach(KeyValuePair<string, object> item in list )
            {
                if (item.Value.GetType().Equals(typeof(string)))
                    where += item.Key + ".Contains(\"" + item.Value + "\") &&";
                else if(item.Value.GetType().Equals(typeof(int)) || item.Value.GetType().Equals(typeof(Int16)) || item.Value.GetType().Equals(typeof(Int32))
                     || item.Value.GetType().Equals(typeof(Int64)))
                     where += item.Key + " = " + item.Value + " &&";
            }

            if (where.Length > 0)
                where = "(" + where.Substring(0, where.Length - 3) + ")";

            return where;
        }
    }

    public class QueryConditionOrConverter : Com.Trakindo.Utility.ListUtility.IListProcessor
    {
        public string Process(IDictionary<string, object> list)
        {
            string where = "";
            foreach (KeyValuePair<string, object> item in list)
            {
                if (item.Value.GetType().Equals(typeof(string)))
                    where += item.Key + ".Contains(\"" + item.Value + "\") ||";
                else if (item.Value.GetType().Equals(typeof(int)) || item.Value.GetType().Equals(typeof(Int16)) || item.Value.GetType().Equals(typeof(Int32))
                     || item.Value.GetType().Equals(typeof(Int64)))
                    where += item.Key + " = " + item.Value + " ||";
            }

            if (where.Length > 0)
                where = "(" + where.Substring(0, where.Length - 3) + ")";

            return where;
        }
    }
}
