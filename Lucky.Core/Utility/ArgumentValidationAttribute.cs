using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Utility
{
    public abstract class ArgumentValidationAttribute:Attribute
    {
        public abstract void Validate(object value, string argumentName);
    }
}
