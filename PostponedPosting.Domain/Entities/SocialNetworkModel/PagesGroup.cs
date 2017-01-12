using PostponedPosting.Common.Extentions;
using System.Collections.Generic;

namespace PostponedPosting.Domain.Entities.SocialNetworkModel
{
    public class PagesGroup
    {
        public int Id { get; set; }
        public int Name { get; set; }

        public virtual SocialNetwork SocialNetwork { get; set; }
        public virtual ICollection<Page> Pages { get; set; }

        public PagesGroup()
        {
            Pages = Pages.Empty();
        }
    }
}
