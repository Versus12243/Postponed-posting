using PostponedPosting.Persistence.ServiceModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ServiceModel.ViewModels
{
    public class PostSelectedGroupDTViewModel: DTParameters
    {
        public int PostId { get; set; }
        public int SocialNetworkId { get; set; }
        //public bool NeedGroupsIds { get; set; }
        public List<int> GroupsIds { get; set; }
    }
}
