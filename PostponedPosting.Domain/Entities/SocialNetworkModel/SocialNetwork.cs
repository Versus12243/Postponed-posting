using PostponedPosting.Common.Extentions;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.Entities.CredentialModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostponedPosting.Domain.Entities.SocialNetworkModel
{
    public class SocialNetwork
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccessToken> Access_tokens { get; set; }

        public virtual ICollection<Page> Pages { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public SocialNetwork()
        {
            Access_tokens = Access_tokens.Empty();
            Pages = Pages.Empty();
            Posts = Posts.Empty();
        }
    }
}
