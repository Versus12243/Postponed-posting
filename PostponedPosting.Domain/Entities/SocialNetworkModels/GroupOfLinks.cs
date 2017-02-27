using PostponedPosting.Common.Extentions;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.Domain.Entities.StatusEnums;
using System;
using System.Collections.Generic;

namespace PostponedPosting.Domain.Entities.SocialNetworkModels
{
    public class GroupOfLinks: BaseEntityWithName_and_Id
    {
        public DateTime DateOfCreation { get; set; }
        public bool IsTemp { get; set; }
        public virtual EntityStatus Status { get; set; }
        public virtual ICollection<Link> Pages { get; set; }
        #region Navigation Properties

        public virtual ApplicationUser User { get; set; }

        public virtual SocialNetwork SocialNetwork { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Link> Links { get; set; }

        #endregion

        public GroupOfLinks()
        {
            Pages = Pages.Empty();
            Posts = Posts.Empty();
            Links = Links.Empty();
        }
    }
}
