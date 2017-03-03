using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Domain.Entities.StatusEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Domain.Entities.CredentialModel
{
    public class UserCredentials
    {
        public int Id { get; set; }
        
        public string Login { get; set; }

        public string Password { get; set; }

        public virtual EntityStatus EntityStatus { get; set; }       
        
        public virtual CredentialsStatus Status { get; set; }         
    }
}
