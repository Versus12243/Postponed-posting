using Postponed_posting.Common.Extentions;
using PostponedPosting.Entities.CredentialModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostponedPosting.Domain.Entities.SocialNetworkModel
{
    public class Social_network
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Access_token> Access_tokens { get; set; }

        public Social_network()
        {
            Access_tokens = Access_tokens.Empty();
        }
    }
}
