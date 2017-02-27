using PostponedPosting.Persistence.ServiceModel.ViewModel;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Abstract
{
    public interface IPostingService
    {
       DTResult<PostViewModel> PostsDataHandler(PostDTViewModel param, string userId);
       DTResult<PostSelectedGroupViewModel> SelectetGroupsDataHandler(PostSelectedGroupDTViewModel param, string userId);
       Task<int> EditPostAsync(PostViewModel model, string userId);
       PostViewModel GetPostData(string userId, int postId);
       int SwitchSendingStatus(string userId, int postId);
       int DeletePost(string userId, int id);
    }
}
