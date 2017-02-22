using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Abstract
{
    public interface IUserService
    {
        CredentialsViewModel GetCredentials(int socialNetworkId, string userId);
        int SaveCredentials(CredentialsViewModel model, string userId);
    }
}
