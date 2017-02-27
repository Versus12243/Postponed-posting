using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.PostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Services
{
    class SeleniumPublishPost
    {
        [Inject]
        public IRepository<Post> PostRepository { get; set; }

        public int PublishPost(int id)
        {
            var post = PostRepository.GetById(id);

            return id;
        }
    }
}
