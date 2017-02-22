using PostponedPosting.Common.Extentions;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Domain.Entities.StatusEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostponedPosting.Domain.Entities.PostModels
{
    public class Post: BaseEntityWithName_and_Id
    {
        #region properties        

        public string Content { get; set; }

        public DateTime DateOfCreation { get; set; }

        public DateTime DateOfPublish { get; set; }

        public virtual PostStatus SendingStatus { get; set; }

        public virtual EntityStatus Status { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("SocialNetwork")]
        public int SocialNetworkId { get; set; }       

        #endregion

        #region Navigation Properties

        public virtual SocialNetwork SocialNetwork { get; set; }

        public virtual ICollection<GroupOfLinks> GroupsOfLinks { get; set; } 
        
        public virtual ApplicationUser User { get; set; }

        #endregion

        #region constructor

        public Post()
        {
            GroupsOfLinks = GroupsOfLinks.Empty();
        }

        #endregion
    }
}
