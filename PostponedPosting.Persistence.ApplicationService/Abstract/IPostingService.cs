using PostponedPosting.Persistence.ServiceModel.RequestModel;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Abstract
{
    public interface IPostingService
    {
       Task<int> AddPostAsync(PostViewModel model, string userId);
    }
}
