using Microsoft.AspNet.Identity.EntityFramework;
using PostponedPosting.Common.Extentions;
using PostponedPosting.Entities.CredentialModel;
using PostponedPosting.Domain.Entities.SocialNetworkModel;
using System.Collections.Generic;
using PostponedPosting.Domain.Entities.PostModels;

namespace PostponedPosting.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<AccessToken> Access_tokens { get; set; }

        public virtual ICollection<Page> Pages { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public ApplicationUser()
        {
            Access_tokens = Access_tokens.Empty();
            Pages = Pages.Empty();
            Posts = Posts.Empty();
        }
    }
}
