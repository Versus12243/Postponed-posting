using PostponedPosting.Persistence.ServiceModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ServiceModel.ViewModels
{
    public class PostDTViewModel: DTParameters
    {
        public int SocialNetworkId { get; set; }
    }
}
