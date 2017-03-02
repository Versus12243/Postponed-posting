using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Domain.Entities.StatusEnums;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.ViewModel;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Services
{
    public class PostingService : IPostingService
    {
        [Inject]
        public IRepository<Post> PostRepository { get; set; }
        [Inject]
        public IRepository<ApplicationUser> UserRepository { get; set; }
        [Inject]
        public IRepository<SocialNetwork> SocialNetworkRepository { get; set; }

        public async Task<int> EditPostAsync(PostViewModel model, string userId)
        {
            var user = UserRepository.GetById(userId);
            if (user != null && user.Status == EntityStatus.Active)
            {
                if (model.Id > 0)
                {
                    var post = PostRepository.Find(p => p.Id == model.Id && p.Status != EntityStatus.Disabled);
                    if (post != null)
                    {
                        post.Name = model.Name;
                        post.Content = model.Content;
                        post.DateOfPublish = (model.SendAfterSaving ? DateTime.UtcNow.AddMinutes(1) : model.DateOfPublish.ToUniversalTime());
                        post.SendingStatus = Domain.Entities.StatusEnums.PostStatus.Pending;
                        post.Status = EntityStatus.Active;

                        foreach (var groupId in model.GroupsIds)
                        {
                            var group = user.GroupsOfLinks.FirstOrDefault(w => w.Id == groupId);
                            if (group != null && !post.GroupsOfLinks.Contains(group))
                            {
                                post.GroupsOfLinks.Add(group);
                            }
                        }

                        PostRepository.Update(post);

                        return post.Id;
                    }
                    throw new Exception("Post is not found");
                }
                else
                {
                    var post = new Post
                    {
                        Name = model.Name,
                        Content = model.Content,
                        DateOfCreation = DateTime.UtcNow,
                        DateOfPublish = (model.SendAfterSaving ? DateTime.UtcNow.AddMinutes(1) : model.DateOfPublish),
                        SendingStatus = Domain.Entities.StatusEnums.PostStatus.Pending,
                        Status = Domain.Entities.StatusEnums.EntityStatus.Active,
                        SocialNetworkId = model.SocialNetworkId,
                        User = user
                    };
                    
                    foreach (var groupId in model.GroupsIds)
                    {
                        var group = user.GroupsOfLinks.FirstOrDefault(w => w.Id == groupId && w.Status == EntityStatus.Active);
                        if (group != null && !post.GroupsOfLinks.Contains(group))
                        {
                            post.GroupsOfLinks.Add(group);
                        }
                    }

                    await PostRepository.InsertAsync(post);

                    return post.Id;
                }
            }
            throw new Exception(" Not enogh permission");
        }

        public DTResult<PostViewModel> PostsDataHandler(PostDTViewModel param, string userId)
        {
            try
            {
                var dtsource = PostRepository.FindAll(w => w.UserId == userId && w.SocialNetworkId == param.SocialNetworkId && w.Status != EntityStatus.Disabled)
                                             .Select(t => new PostViewModel
                                             {
                                                 Id = t.Id,
                                                 Name = t.Name,
                                                 DateOfCreation = t.DateOfCreation,
                                                 DateOfPublish = t.DateOfPublish,
                                                 Status = t.Status.ToString(),
                                                 StatusOfSending = t.SendingStatus.ToString()
                                             });



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
                var col4 = columnFilters[4];
                var col5 = columnFilters[5];

                Expression<Func<PostViewModel, bool>> predicate = p => (search == null || ((p.Name != null && p.Name.ToLower().Contains(search))
                || (p.DateOfCreation != null && p.DateOfCreation.ToString().ToLower().Contains(search))
                || (p.DateOfPublish != null && p.DateOfPublish.ToString().ToLower().Contains(search))
                || (p.StatusOfSending != null && p.StatusOfSending.ToString().ToLower().Contains(search))
                || (p.Status != null && p.Status.ToString().ToLower().Contains(search)))
                && (col1 == null || (p.Name != null && p.Name.ToLower().Contains(col1)))
                && (col2 == null || (p.DateOfCreation != null && p.DateOfCreation.ToString().ToLower().Contains(col2)))
                && (col3 == null || (p.DateOfPublish != null && p.DateOfPublish.ToString().ToLower().Contains(col3)))
                && (col4 == null || (p.StatusOfSending != null && p.StatusOfSending.ToString().ToLower().Contains(col4)))
                && (col5 == null || (p.Status != null && p.Status.ToString().ToLower().Contains(col5))));

                List<PostViewModel> data = new ResultSet<PostViewModel>().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, dtsource, predicate);
                int count = new ResultSet<PostViewModel>().Count(param.Search.Value, dtsource, predicate);
                DTResult<PostViewModel> result = new DTResult<PostViewModel>
                {
                    draw = param.Draw,
                    data = data.Select(p => new PostViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        DateOfCreation = p.DateOfCreation,
                        DateOfPublish = p.DateOfPublish,
                        StatusOfSending = p.StatusOfSending,
                        Status = p.Status
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

        public DTResult<PostSelectedGroupViewModel> SelectetGroupsDataHandler(PostSelectedGroupDTViewModel param, string userId)
        {
            try
            {
                IQueryable<PostSelectedGroupViewModel> groups = null;

                if (param.PostId > 0)
                {
                    var post = PostRepository.Find(p => p.Id == param.PostId && p.SocialNetworkId == param.SocialNetworkId && p.UserId == userId && p.Status != EntityStatus.Disabled);

                    if (post != null)
                    {
                        var groupsIds = post.GroupsOfLinks.Select(g => g.Id).ToList();

                        foreach(var id in groupsIds)
                        {
                            if(!param.GroupsIds.Contains(id))
                            {
                                var group = post.GroupsOfLinks.FirstOrDefault(w => w.Id == id);

                                if(group != null)
                                {
                                    post.GroupsOfLinks.Remove(group);
                                }
                            }
                        }

                        groupsIds = post.GroupsOfLinks.Select(g => g.Id).ToList();

                        foreach (var id in param.GroupsIds)
                        {
                            if (!groupsIds.Contains(id))
                            {
                                var group = post.User.GroupsOfLinks.FirstOrDefault(w => w.SocialNetwork.Id == post.SocialNetworkId && w.Status == EntityStatus.Active && w.Id == id);
                                if (group != null)
                                {
                                    post.GroupsOfLinks.Add(group);
                                }
                                else
                                    throw new Exception("Group is not found");
                            }
                        }

                        PostRepository.Update(post);

                        groupsIds = post.GroupsOfLinks.Select(g => g.Id).ToList();

                        groups = post.User.GroupsOfLinks.Where(w => w.SocialNetwork.Id == post.SocialNetworkId && w.Status == EntityStatus.Active)
                            .Select(g => new PostSelectedGroupViewModel
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Selection = groupsIds.Contains(g.Id)
                            }).AsQueryable();
                    }
                    else throw new Exception("Post is not found");
                }
                else
                {
                    var user = UserRepository.Find(u => u.Id == userId && u.Status == EntityStatus.Active);
                    if (user != null)
                    {
                        groups = user.GroupsOfLinks.Where(w => w.SocialNetwork.Id == param.SocialNetworkId && w.Status == EntityStatus.Active)
                       .Select(g => new PostSelectedGroupViewModel
                       {
                           Id = g.Id,
                           Name = g.Name,
                           Selection = false
                       }).AsQueryable();
                    }
                    else
                        throw new Exception("Out of permission");
                }

                List<String> columnFilters = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnFilters.Add(col.Search.Value);
                }

                var search = param.Search.Value.ToLower();

                var col2 = columnFilters[2];

                Expression<Func<PostSelectedGroupViewModel, bool>> predicate = p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search))
                && (col2 == null || (p.Name != null && p.Name.ToLower().Contains(col2.ToLower()))));

                List<PostSelectedGroupViewModel> data = new ResultSet<PostSelectedGroupViewModel>().GetResult(param.Search.Value, param.SortOrder, param.Start, param.Length, groups, predicate);
                int count = new ResultSet<PostSelectedGroupViewModel>().Count(param.Search.Value, groups, predicate);
                
                DTResult<PostSelectedGroupViewModel> result = new DTResult<PostSelectedGroupViewModel>
                {
                    draw = param.Draw,
                    data = data.Select(p => new PostSelectedGroupViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Selection = p.Selection
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

        public PostViewModel GetPostData(string userId, int postId)
        {
            var user = UserRepository.Find(u => u.Id == userId && u.Status == EntityStatus.Active);

            if (user != null)
            {
                var post = user.Posts.FirstOrDefault(w => w.Id == postId);

                if (post != null)
                {
                    return new PostViewModel
                    {
                        Id = post.Id,
                        Name = post.Name,
                        Content = post.Content,
                        DateOfCreation = post.DateOfCreation,
                        DateOfPublish = post.DateOfPublish,
                        Status = post.Status.ToString(),
                        StatusOfSending = post.SendingStatus.ToString(),
                        GroupsIds = post.GroupsOfLinks.Where(t => t.Status == EntityStatus.Active).Select(g => g.Id).ToList()
                    };
                }

                throw new Exception("Post is not found");
            }

            throw new Exception("Not enogh permission");
        }

        public int SwitchSendingStatus(string userId, int postId)
        {
            var user = UserRepository.Find(u => u.Id == userId && u.Status == EntityStatus.Active);

            if (user != null)
            {
                var post = user.Posts.FirstOrDefault(w => w.Id == postId);

                if (post != null)
                {
                    if (post.SendingStatus == PostStatus.Pending)
                    {
                        post.SendingStatus = PostStatus.Suspended;
                    }
                    else if(post.SendingStatus != PostStatus.Ready && post.SendingStatus != PostStatus.Sending)
                    {
                        if (DateTime.Compare(post.DateOfPublish, DateTime.Now) <= 0)
                            throw new Exception("Time of publish must be greater than current time");
                        post.SendingStatus = PostStatus.Pending;
                    }

                    PostRepository.Update(post);
                    return post.Id;
                }

                throw new Exception("Post is not found");
            }

            throw new Exception("Not enogh permission");
        }

        public int DeletePost(string userId, int id)
        {
            var user = UserRepository.Find(u => u.Id == userId && u.Status == EntityStatus.Active);

            if(user != null)
            {
                var post = PostRepository.Find(p => p.Id == id);

                if(post != null)
                {
                    if(post.Status == EntityStatus.Active)
                        post.Status = EntityStatus.Deleted;
                    else if(post.Status == EntityStatus.Deleted)
                        post.Status = EntityStatus.Disabled;

                    post.SendingStatus = PostStatus.Suspended;
                    PostRepository.Update(post);
                    return post.Id;
                }
                throw new Exception("Post is not found");
            }
            throw new Exception("Not enogh permission");
        }
    }
}
