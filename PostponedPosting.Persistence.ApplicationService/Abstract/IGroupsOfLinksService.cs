using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Abstract
{
    public interface IGroupsOfLinksService
    {
       GroupOfLinks GetGroupById(int id, string userId);
       List<GroupOfLinksViewModel> GetAllGroupsOfLinks(string userId, int socialNetworkId);
       int EditGroupOfLinks(string userId, int socialNetworkId, GroupOfLinksViewModel model);
       List<LinkViewModel> GetAllLinksOfGroup(string userId, int groupId);
    }
}
