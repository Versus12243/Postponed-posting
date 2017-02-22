using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ServiceModel.ViewModels
{
    public class CredentialsViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int SocialNetworkId { get; set; }
    }
}
