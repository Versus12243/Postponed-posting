using PostponedPosting.Domain.Entities;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Domain.Entities.StatusEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Domain.Entities.CredentialModel
{
    public class AccessToken
    {
        [Key]
        public int Id { get; set; }

        public virtual SocialNetwork Social_network { get; set; }

        public virtual ApplicationUser User { get; set; }   
        
        public virtual EntityStatus Status { get; set; }     
    }
}
