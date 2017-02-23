using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ServiceModel.ViewModels
{
    public class PostViewModel: BaseLinkViewModel
    {
        public string Content { get; set; }
        public DateTime DateOfPublish { get; set; }
        public string StatusOfSending { get; set; }
        public string Status { get; set; }
        public List<int> GroupsIds { get; set; }
        public bool SendAfterSaving { get; set; }
        public int SocialNetworkId { get; set; }
    }
}
