using PostponedPosting.Common.Extentions;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Domain.Entities.StatusEnums;
using System;
using System.Collections.Generic;

namespace PostponedPosting.Domain.Entities.SocialNetworkModels
{
    public class Link: BaseEntityWithName_and_Id
    {
        #region properties

        public string Url { get; set; }

        public virtual EntityStatus Status { get; set; }

        public DateTime DateOfCreation { get; set; }

        public virtual ICollection<GroupOfLinks> Groups { get; set; }

        public virtual SocialNetwork SocialNetwork { get; set; }

        #endregion     

        #region constructor

        public Link()
        {
            Groups = Groups.Empty();
        }

        #endregion
    }
}
