using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.Identity;
using System.Reflection;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using System.Data.SqlClient;
using NLog;

namespace PostponedPosting.SeleniumApp
{
    public delegate void RemoveDriver(string value);

    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            object locker = new object();
            
            NinjectHelper.InitializeKernal();

            List<UserPostsSender> Senders = new List<UserPostsSender>();   

            var cryptoService = (ICryptoService)NinjectHelper.kernal.GetService(typeof(ICryptoService));
            var postRepositoryForThread = NinjectHelper.kernal.Get<IRepository<Post>>();         
                               
            while (true)
            {
                    var currentTime = DateTime.UtcNow;
                    var timeOfPrevScan = currentTime.AddMinutes(-1);
                    var timeOfNextScan = currentTime.AddMinutes(1);
                    var posts = postRepositoryForThread.FindAll(p => p.SendingStatus == Domain.Entities.StatusEnums.PostStatus.Pending && DateTime.Compare(p.DateOfPublish, timeOfNextScan) <= 0 && DateTime.Compare(p.DateOfPublish, timeOfPrevScan) > 0);
                    var usersPosts = posts.Where(p => p.User.UserSocialNetworks.FirstOrDefault(w => w.SocialNetwork.Id == p.SocialNetworkId).Credentials.Status == Domain.Entities.StatusEnums.CredentialsStatus.Active).GroupBy(u => u.Id).ToList();

                    lock (locker)
                    {
                        foreach (var userPosts in usersPosts)
                        {
                            var firstUserPost = userPosts.FirstOrDefault();
                            if (firstUserPost != null)
                            {
                                var user = firstUserPost.User;
                                if (user != null)
                                {
                                    UserPostsSender sender = Senders.FirstOrDefault(t => t.Id == user.Id);
                                    if (sender != null)
                                    {
                                        foreach (var post in userPosts)
                                        {
                                            sender.AddPost(post);
                                        }
                                    }
                                    else
                                    {
                                        var userSN = user.UserSocialNetworks.FirstOrDefault(w => w.SocialNetwork.Id == firstUserPost.SocialNetworkId);
                                        var login = cryptoService.GetDecryptedValue(user.PasswordHash, userSN.Credentials.Login);
                                        var password = cryptoService.GetDecryptedValue(user.PasswordHash, userSN.Credentials.Password);
                                        UserPostsSender postsSender = new UserPostsSender(user.Id, login, password, new RemoveDriver((string value) =>
                                        {
                                            lock (locker)
                                            {
                                                var element = Senders.FirstOrDefault(w => w.Id == value);
                                                if (element != null)
                                                {
                                                    Senders.Remove(element);
                                                }
                                            }
                                        }));
                                        if (postsSender.LoginSuccessful)
                                        {
                                            foreach (var post in userPosts)
                                            {
                                                post.SendingStatus = Domain.Entities.StatusEnums.PostStatus.Sending;
                                                postRepositoryForThread.Update(post);
                                                postsSender.AddPost(post);
                                            }
                                            Senders.Add(postsSender);
                                            Senders.Last().HandleThreadDone(null, null);
                                        }
                                        else
                                        {
                                            try
                                            {
                                                userSN.Credentials.Status = Domain.Entities.StatusEnums.CredentialsStatus.ErrorOccurred;
                                                postRepositoryForThread.Update(firstUserPost);
                                            }
                                            catch (Exception ex)
                                            {
                                                logger.Error("Exception occurred while changing status of user credentials with id " + userSN.Credentials.Id + "; Exception message: " + ex.Message);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                Thread.Sleep(60000);
            }
        }
    }
}
