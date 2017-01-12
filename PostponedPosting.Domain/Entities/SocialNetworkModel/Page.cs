using PostponedPosting.Common.Extentions;
using PostponedPosting.Domain.Entities.Identity;
using System.Collections.Generic;

namespace PostponedPosting.Domain.Entities.SocialNetworkModel
{
    public class Page
    {
        #region properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ApplicationUser User { get; set; }

        public virtual SocialNetwork Social_network { get; set; }

        public virtual ICollection<PagesGroup> Groups { get; set; }

        #endregion

        public Page()
        {
            Groups = Groups.Empty();
        }
    }
}
