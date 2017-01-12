using PostponedPosting.Domain.Entities.PostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Abstract
{
    public interface ISeleniumPublishPost
    {
        int PublishPost(int id);
    }
}
