using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Com.Trakindo.Utility
{
    class LinqProcessor
    {
        public class SearchInfo<T>
        {
            public Expression<Func<T, object>> PropertySelector { get; set; }
            public QueryOperator Operator { get; set; }
            public StringComparer Comparer { get; set; }
            public string Value { get; set; }
        }

        public enum QueryOperator
        {
            And = 0,
            Or = 1
        }

        public enum StringComparer
        {
            Equals = 0,
            StartsWith = 1,
            EndsWith = 2,
            Contains = 3
        }

    }
}
