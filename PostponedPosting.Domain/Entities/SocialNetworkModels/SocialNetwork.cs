using PostponedPosting.Common.Extentions;
using PostponedPosting.Domain.Entities.CredentialModel;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.Domain.Entities.StatusEnums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostponedPosting.Domain.Entities.SocialNetworkModels
{
    public class SocialNetwork: BaseEntityWithName_and_Id
    {
        public virtual SocialNetworkAvailabily Aviability { get; set; }
        public virtual EntityStatus Status { get; set; }
        public virtual ICollection<AccessToken> Access_tokens { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public SocialNetwork()
        {
            Access_tokens = Access_tokens.Empty();
            Posts = Posts.Empty();
        }
    }
}
