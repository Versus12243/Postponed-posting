using System.Collections.Generic;

namespace PostponedPosting.Common.Extentions
{
    public static class CollectionExtensions
    {
        public static ICollection<TResult> Empty<TResult>(this ICollection<TResult> collection)
        {
            return new List<TResult>();
        }
    }
}
