using PostponedPosting.Persistence.ServiceModel.ViewModel;
using PostponedPosting.Persistence.ServiceModel.ViewModels;

namespace PostponedPosting.Persistence.ApplicationService.Abstract
{
    public interface ILinksService
    {
        DTResult<LinkViewModel> DataHandler(DTParameters param, string userId, int groupId, bool showAll);
        LinksForGroupViewModel GetAllLinksForGroup(string userId, int groupId, bool showAll);
        int EditLink(LinkViewModel model, string userId);
        int SwitchLinkPresenceInGroup(string userId, int linkId, int groupId);
    }
}
