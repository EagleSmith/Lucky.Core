using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Utility
{
    [AttributeUsage(AttributeTargets.Parameter|AttributeTargets.Property | AttributeTargets.Field )]
    public class NotNullAttribute:ArgumentValidationAttribute
    {
        public override void Validate(object value, string argumentName)
        {
            if (value == null)
                throw new ArgumentNullException(argumentName);
        }
    }
}
