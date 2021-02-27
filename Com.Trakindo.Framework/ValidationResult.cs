using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;

namespace Com.Trakindo.Framework
{
    public class ValidationResult
    {
        public bool Result { get; set; }
        public String ErrorMessage { get; set; }
        public Object PropertyValue { get; set; }

        public ValidationResult()
        {

        }

        public ValidationResult(bool result) : this(result, "", null)
        {

        }

        public ValidationResult(bool result, string errorMessage, object value)
        {
            this.Result = result;
            this.ErrorMessage = errorMessage;
            this.PropertyValue = value;
        }
    }
}
