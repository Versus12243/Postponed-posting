using Microsoft.AspNet.Identity.EntityFramework;
using PostponedPosting.Common.Extentions;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
using System.Collections.Generic;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.Domain.Entities.StatusEnums;
using PostponedPosting.Domain.Entities.CredentialModel;
using Microsoft.AspNet.Identity;

namespace PostponedPosting.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<AccessToken> Access_tokens { get; set; }

        public virtual ICollection<GroupOfLinks> GroupsOfLinks { get; set; }

        public virtual ICollection<UserSocialNetwork> UserSocialNetworks { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual EntityStatus Status { get; set; }

        public ApplicationUser()
        {
            Access_tokens = Access_tokens.Empty();
            GroupsOfLinks = GroupsOfLinks.Empty();
            Posts = Posts.Empty();
            UserSocialNetworks = UserSocialNetworks.Empty();
        }
    }
}
