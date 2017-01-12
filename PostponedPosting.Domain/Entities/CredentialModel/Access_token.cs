using PostponedPosting.Domain.Entities;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.SocialNetworkModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Entities.CredentialModel
{
    public class AccessToken
    {
        [Key]
        public int Id { get; set; }

        public virtual SocialNetwork Social_network { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
