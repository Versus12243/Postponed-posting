using PostponedPosting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ServiceModel.ViewModels
{
    public class AvailableSocialNetworkViewModel: BaseEntityWithName_and_Id
    {
        public int SocialNetworkId { get; set; }
        public bool IsActive { get; set; }
        public bool CredentialsExist { get; set; }
        public bool CredentialsIsActive { get; set; }
        public bool SocialNetworkIsAvailable { get; set; }
        public bool SocialNetworkIsActive { get; set; }
    }
}
