using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static void ThrowsWhenNull(this object obj, string paramName = null)
        {
            if (obj == null)
            {
                if (string.IsNullOrEmpty(paramName))
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    throw new ArgumentNullException(paramName);
                }
            }
        }
    }
}
