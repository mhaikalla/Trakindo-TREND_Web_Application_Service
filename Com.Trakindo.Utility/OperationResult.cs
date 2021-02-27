using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Trakindo.Utility
{
    public class OperationResult
    {
        public System.String Message { get; set; }
        public bool Result { get; set; }
        public object Data { get; set; }
        public object OtherData { get; set; }

        public OperationResult()
        {

        }

        public OperationResult(bool result) : this(result, null, "")
        {

        }

        public OperationResult(bool result, object data) : this(result, data, null)
        {

        }

        public OperationResult(bool result, string message) : this(result, null, message)
        {

        }



        public OperationResult(bool result, object data, string message)
        {
            this.Message = message;
            this.Result = result;
            this.Data = data;
        }
    }
}
