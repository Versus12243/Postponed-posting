using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Abstract
{
    public interface ISocialNetworksService
    {
        List<SocialNetwork> GetSocialNetworks();
        List<AvailableSocialNetworkViewModel> GetAviableSNForUser(string UserId);
    }
}
