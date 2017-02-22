using PostponedPosting.Domain.Entities.CredentialModel;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.StatusEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Domain.Entities.SocialNetworkModels
{
    public class UserSocialNetwork
    {
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual SocialNetwork SocialNetwork { get; set; }

        public virtual EntityStatus Status { get; set; } 

        public virtual UserCredentials Credentials { get; set; }

        public virtual ICollection<GroupOfLinks> PagesGroups { get; set; }
    }
}
