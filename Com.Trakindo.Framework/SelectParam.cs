using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Trakindo.Utility;

namespace Com.Trakindo.Framework
{
    public enum Ordering
    {
        ASC=1,
        DESC=2
    }

    public class SelectParam
    {
        public object Data { get; set; }
        public String Keyword { get; set; }
        public String Where { get; set; }
        private List<String> orderBy;
        public List<String> OrderBy
        {
            get
            {
                if (this.orderBy == null)
                    this.orderBy = new List<string>();
                return this.orderBy;
            }
        }

        private List<object> parameters = null;
        public void AddParam(object o)
        {
            if (this.parameters == null)
                this.parameters = new List<object>();
            this.parameters.Add(o);
        }
        public object [] Parameters
        {
            get
            {
                if (this.parameters != null)
                    return this.parameters.ToArray();
                else
                    return null;
            }

        }

        public Ordering Ordering { get; set; }

        public String OrderByString {
            get
            {
                return String.Join(",", this.OrderBy.ToArray());
            }

        }

        public SelectParam() : this("")
        {

        }

        public SelectParam(String where) : this(where, Ordering.ASC)
        {

        }

        public SelectParam(String where, Ordering ordering)
        {
            this.Where = where;
            this.Ordering = ordering;
        }

        public void AddWhere(ListUtility.Parameters parameters)
        {
            Com.Trakindo.Utility.ListUtility.IListProcessor listProcessor = new Com.Trakindo.Utility.Database.QueryConditionAndConverter();
            string where = parameters.Convert(listProcessor);
            this.Where = where;
        }

        public void AddOrderBy(String propName)
        {
            this.OrderBy.Add(propName);
        }
    }
}
