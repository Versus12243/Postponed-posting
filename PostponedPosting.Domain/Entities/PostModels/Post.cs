using PostponedPosting.Common.Extentions;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.SocialNetworkModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostponedPosting.Domain.Entities.PostModels
{
    public class Post
    {
        #region properties

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime DateOfCreate { get; set; }

        public DateTime DateOfPublish { get; set; }

        public bool IsActive { get; set; }

        public bool IsReady { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("SocialNetwork")]
        public int SocialNetworkId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual SocialNetwork SocialNetwork { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        
        public virtual ApplicationUser User { get; set; }

        #endregion

        public Post()
        {
            Pages = Pages.Empty();
        }
    }
}
