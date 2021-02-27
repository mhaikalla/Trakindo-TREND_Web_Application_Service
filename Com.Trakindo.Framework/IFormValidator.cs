using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Trakindo.Framework
{
    public interface IFormValidator
    {
        ValidationResult Validate(DataForm form);
    }
}
