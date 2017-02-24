using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.ViewModel;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace PostponedPosting.Persistence.ApplicationService.Services
{
    public class ResultSet<T> where T: class
    {
        public List<T> GetResult(string search, string sortOrder, int start, int length, IQueryable<T> dtResult, Expression<Func<T, bool>> predicate)
        {
            return FilterResult(search, dtResult, predicate).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, IQueryable<T> dtResult, Expression<Func<T, bool>> predicate)
        {
            return FilterResult(search, dtResult, predicate).Count();
        }

        private IQueryable<T> FilterResult(string search, IQueryable<T> dtResult, Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> results = dtResult.AsQueryable();

            if (search != null)
            {
                search = search.ToLower();
            }
                       
            results = results.Where(predicate);

            return results;
        }
    }

    public class LinksService: ILinksService
    {       
        [Inject]
        public IRepository<ApplicationUser> UserRepository { get; set; }
        [Inject]
        public IRepository<GroupOfLinks> GroupOfLinksRepository { get; set; }
        [Inject]
        public IRepository<Link> LinkRepository { get; set; }

        public LinksForGroupViewModel GetAllLinksForGroup(string userId, int groupId, bool showAll)
        {
            var model = new LinksForGroupViewModel();
            model.ListOfLinks = new List<LinkViewModel>();

            var user = UserRepository.Find(u => u.Id == userId && u.Status == Domain.Entities.StatusEnums.EntityStatus.Active);

            if (user != null)
            {
                var group = GroupOfLinksRepository.Find(g => g.Id == groupId && g.Status == Domain.Entities.StatusEnums.EntityStatus.Active);

                if (group != null)
                {
                    model.Id = groupId;
                    model.Name = group.Name;

                    if (showAll)
                    {
                        model.ListOfLinks = LinkRepository.FindAll(w => w.SocialNetwork.Id == group.SocialNetwork.Id && w.Groups.FirstOrDefault(g => g.User.Id == userId) != null
                                                                        && w.Status == Domain.Entities.StatusEnums.EntityStatus.Active)
                                            .Select(s => new LinkViewModel
                                            {
                                                Id = s.Id,
                                                Name = s.Name,
                                                Url = s.Url,
                                                DateOfCreation = s.DateOfCreation,
                                                GroupId = s.Groups.Select(i => i.Id).Contains(group.Id) ? groupId : 0
                                            }).ToList();
                    }
                    else
                    {
                        model.ListOfLinks = LinkRepository.FindAll(w => w.Groups.FirstOrDefault(g => g.User.Id == userId && g.Id == groupId) != null
                                                                        && w.Status == Domain.Entities.StatusEnums.EntityStatus.Active)
                                            .Select(s => new LinkViewModel
                                            {
                                                Id = s.Id,
                                                Name = s.Name,
                                                Url = s.Url,
                                                DateOfCreation = s.DateOfCreation,
                                                GroupId = s.Groups.Select(i => i.Id).Contains(group.Id) ? groupId : 0
                                            }).ToList();
                    }

                }
                else
                    throw new Exception("Group is not found");
            }
            else
                throw new Exception("User is not found");

            return model;
        }
        
        public DTResult<LinkViewModel> DataHandler(DTParameters param, string userId, int groupId, bool showAll)
        {
            try
            {
                var dtsource = GetAllLinksForGroup(userId, groupId, showAll).ListOfLinks.AsQueryable();

                List<String> columnFilters = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnFilters.Add(col.Search.Value);
                }

                var search = param.Search.Value.ToLower();
                
                var col0 = columnFilters[0];
                var col1 = columnFilters[1];
                var col2 = columnFilters[2];
                var col3 = columnFilters[3];

                Expression<Func<LinkViewModel, bool>> predicate = p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search)
                || p.Url != null && p.Url.ToLower().Contains(search) || p.DateOfCreation.ToString().Contains(search))
                && (col1 == null || (p.Name != null && p.Name.ToLower().Contains(col0.ToLower())))
                && (col2 == null || (p.Url != null && p.Url.ToLower().Contains(col1.ToLower())))
                && (col3 == null || (p.DateOfCreation != null && p.DateOfCreation.ToString().Contains(col2.ToLower()))));

                List<LinkViewModel> data = new ResultSet<LinkViewModel>().GetResult(search, param.SortOrder, param.Start, param.Length, dtsource, predicate);
                int count = new ResultSet<LinkViewModel>().Count(search, dtsource, predicate);
                DTResult<LinkViewModel> result = new DTResult<LinkViewModel>
                {
                    draw = param.Draw,
                    data = data.Select(p => new LinkViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Url = p.Url,
                        DateOfCreation = p.DateOfCreation,
                        GroupId = p.GroupId
                    }).ToList(),
                    recordsFiltered = count,
                    recordsTotal = count
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public int EditLink(LinkViewModel model, string userId)
        {
            var group = GroupOfLinksRepository.Find(g => g.Id == model.GroupId);
            if (model.Id != 0)
            {
                var link = LinkRepository.Find(w => w.Id == model.Id && w.Groups.FirstOrDefault(u => u.User.Id == userId) != null);
                if(link != null)
                {
                    link.Name = model.Name;
                    link.Url = model.Url;
                    if (!link.Groups.Contains(group))
                    {
                        link.Groups.Add(group);                       
                    }
                    LinkRepository.Update(link);
                    return link.Id;
                }
            }
            else {
                //Different user can create same links
                var link = LinkRepository.Find(w => w.Url == model.Url && w.Groups.FirstOrDefault(u => u.User.Id == userId) != null && w.Status == Domain.Entities.StatusEnums.EntityStatus.Active);                

                if(group == null)
                {
                    throw new Exception("Group is not found");
                }

                if (link != null)
                {                   
                    if (link.Name != model.Name)
                    {
                        throw new Exception("Link already exist. It name is " + link.Name);
                    }  
                    if(!link.Groups.Contains(group))
                    {
                        link.Groups.Add(group);
                        LinkRepository.Update(link);
                        return link.Id;
                    }                 
                }
                else
                {
                        Link lnk = new Link()
                        {
                            Name = model.Name,
                            Url = model.Url,
                            Status = Domain.Entities.StatusEnums.EntityStatus.Active,
                            DateOfCreation = DateTime.UtcNow,
                            SocialNetwork = group.SocialNetwork
                        };
                        lnk.Groups = new List<GroupOfLinks>() { group };
                        LinkRepository.Insert(lnk);
                    return lnk.Id;                                 
                }
            }
            return -1;
        }

        public int SwitchLinkPresenceInGroup(string userId, int linkId, int groupId)
        {
            var user = UserRepository.Find(u => u.Id == userId);

            if(user != null)
            {
                var link = LinkRepository.GetById(linkId);
                if (link != null)
                {
                    var group = GroupOfLinksRepository.Find(g => g.Id == groupId);
                    if(group != null)
                    {
                        if(link.Groups.Select(i => i.Id).Contains(groupId))
                        {
                            link.Groups.Remove(group);
                            LinkRepository.Update(link);
                            return link.Id;
                        }
                        link.Groups.Add(group);
                        LinkRepository.Update(link);
                        return link.Id;
                    }
                    throw new Exception("Group is not found");
                }
                    throw new Exception("Link is not found");
            }
                throw new Exception("User is not found");
        }
    }
}
