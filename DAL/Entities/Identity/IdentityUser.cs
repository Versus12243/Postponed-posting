using Microsoft.AspNet.Identity.EntityFramework;
using Postponed_posting.Common.Extentions;
using PostponedPosting.Entities.CredentialModel;
using PostponedPosting.Domain.Entities.SocialNetworkModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Domain.Entities.Identity
{ 
     public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Access_token> Access_tokens { get; set; }

        public ApplicationUser()
        {
            Access_tokens = Access_tokens.Empty();
        }
    }
}
