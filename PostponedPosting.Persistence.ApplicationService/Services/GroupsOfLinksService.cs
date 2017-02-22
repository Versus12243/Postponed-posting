using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostponedPosting.Common.Extensions;

namespace PostponedPosting.Persistence.ApplicationService.Services
{
    public class GroupsOfLinksService: IGroupsOfLinksService
    {
        [Inject]
        public IRepository<GroupOfLinks> GroupOfLinksRepository { get; set; }
        [Inject]
        public IRepository<SocialNetwork> SocialNetworkRepository { get; set; }
        [Inject]
        public IRepository<ApplicationUser> UserRepository { get; set; }

        public GroupOfLinks GetGroupById(int id, string userId)
        {
            return GroupOfLinksRepository.Find(g => g.Id == id && g.User.Id == userId && g.Status == Domain.Entities.StatusEnums.EntityStatus.Active);
        }

        public List<GroupOfLinksViewModel> GetAllGroupsOfLinks(string userId, int socialNetworkId)
        {
            var groups = GroupOfLinksRepository.FindAll(w => w.User.Id == userId && w.SocialNetwork.Id == socialNetworkId);
            var model = new List<GroupOfLinksViewModel>();

            foreach(var group in groups)
            {
                var pGroup = new GroupOfLinksViewModel();
                pGroup.Id = group.Id;
                pGroup.Name = group.Name;
                pGroup.DateOfCreation = group.DateOfCreation;
                model.Add(pGroup);
            }

            return model;
        }


        #region DataTables
   
        #endregion

        public int EditGroupOfLinks(string userId, int socialNetworkId, GroupOfLinksViewModel model)
        {
            if (model.Name != null && model.Name != "")
            {
                if (model.Id != 0)
                {
                    var group = GroupOfLinksRepository.Find(g => g.Id == model.Id && g.Status == Domain.Entities.StatusEnums.EntityStatus.Active);

                    if(group != null)
                    {
                        group.Name = model.Name;
                        GroupOfLinksRepository.Update(group);
                        return group.Id;
                    }

                    throw new Exception("Can`t find the group");
                }
                else
                {
                    var existedGroups = GroupOfLinksRepository.Find(g => g.Name == model.Name
                                                                    && g.SocialNetwork.Id == socialNetworkId
                                                                    && g.User.Id == userId
                                                                    && g.Status == Domain.Entities.StatusEnums.EntityStatus.Active);
                    if (existedGroups != null)
                    {
                        throw new Exception("Group with the same name already exist");
                    }

                    var sn = SocialNetworkRepository.Find(s => s.Id == socialNetworkId && s.Status == Domain.Entities.StatusEnums.EntityStatus.Active);
                    var user = UserRepository.Find(u => u.Id == userId && u.Status == Domain.Entities.StatusEnums.EntityStatus.Active);

                    if (sn != null && user != null)
                    {
                        GroupOfLinks group = new GroupOfLinks();
                        group.DateOfCreation = DateTime.Now;
                        group.IsTemp = false;
                        group.Name = model.Name;
                        group.SocialNetwork = sn;
                        group.User = user;
                        group.Status = Domain.Entities.StatusEnums.EntityStatus.Active;

                        GroupOfLinksRepository.Insert(group);

                        return group.Id;
                    }

                    throw new Exception("Can`t find social network or not enought permissions");

                }
            }
            throw new Exception("Name can`t be empty");
        }

        public List<LinkViewModel> GetAllLinksOfGroup(string userId, int groupId)
        {
            var user = UserRepository.Find(u => u.Id == userId && u.Status == Domain.Entities.StatusEnums.EntityStatus.Active);

            if(user != null)
            {
                var group = user.GroupsOfLinks.FirstOrDefault(w => w.Id == groupId && w.Status == Domain.Entities.StatusEnums.EntityStatus.Active);

                if(group != null)
                {
                    var links = group.Links.Select(l => new LinkViewModel
                    {
                        Id = l.Id,
                        DateOfCreation = l.DateOfCreation,
                        GroupId = groupId,
                        Name = l.Name,
                        Url = l.Url
                    }).ToList();

                    return links;
                }
                throw new Exception("Group is not found");
            }
            throw new Exception("Not enough permission");
        }

    }
}
