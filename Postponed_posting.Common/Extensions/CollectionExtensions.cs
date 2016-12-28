using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postponed_posting.Common.Extentions
{
    public static class CollectionExtensions
    {
        public static ICollection<TResult> Empty<TResult>(this ICollection<TResult> collection)
        {
            return new List<TResult>();
        }
    }
}
