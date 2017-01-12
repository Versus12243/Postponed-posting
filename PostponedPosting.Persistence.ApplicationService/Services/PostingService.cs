using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.Domain.Entities.SocialNetworkModel;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Services
{
    public class PostingService: IPostingService
    {
        [Inject]
        public IRepository<Post> PostRepository { get; set; }
        [Inject]
        public IRepository<ApplicationUser> UserRepository { get; set; }
        [Inject]
        public IRepository<SocialNetwork> SocialNetworkRepository { get; set; }
        
        public async Task<int> AddPostAsync(PostViewModel model, string userId)
        {
            var user = UserRepository.GetById(userId);
            var post = new Post
            {
                Content = model.Content,
                DateOfCreate = DateTime.Now,
                DateOfPublish = model.DateOfPublish,
                IsActive = true,
                SocialNetwork = SocialNetworkRepository.GetById(1),
                User = user
        };

            await PostRepository.InsertAsync(post);

            return post.Id;
        }
    }
}
