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

namespace PostponedPosting.SeleniumApp
{
    public delegate void RemoveDriver(string value);

    class Program
    {
        static void Main(string[] args)
        {
            object locker = new object();
            
            NinjectHelper.InitializeKernal();

            var postRepository = NinjectHelper.kernal.Get<IRepository<Post>>();
            var cryptoService = (ICryptoService)NinjectHelper.kernal.GetService(typeof(ICryptoService));

            List<UserPostsSender> Senders = new List<UserPostsSender>();            

            new Thread(() =>
            {
                while (true)
                {
                    var currentTime = DateTime.UtcNow;
                    var timeOfPrevScan = currentTime.AddMinutes(-1);
                    var timeOfNextScan = currentTime.AddMinutes(1);
                    var posts = postRepository.FindAll(p => p.SendingStatus == Domain.Entities.StatusEnums.PostStatus.Pending && DateTime.Compare(p.DateOfPublish, timeOfNextScan) <= 0 && DateTime.Compare(p.DateOfPublish, timeOfPrevScan) > 0);
                    var usersPosts = posts.GroupBy(u => u.Id);

                    lock (locker)
                    {
                        foreach (var userPosts in usersPosts)
                        {
                            var firstUserPost = userPosts.FirstOrDefault();
                            if (firstUserPost != null)
                            {
                                var user = firstUserPost.User;
                                if(user != null)
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
                                        UserPostsSender postsSender = new UserPostsSender(user.Id, login, password, new RemoveDriver((string value) => {
                                            lock (locker)
                                            {
                                                var element = Senders.FirstOrDefault(w => w.Id == value);
                                                if(element != null)
                                                {
                                                    Senders.Remove(element);
                                                }
                                            }
                                        }));
                                        foreach (var post in userPosts)
                                        {
                                            postsSender.AddPost(post);
                                        }
                                        Senders.Add(postsSender);
                                        Senders.Last().HandleThreadDone(null, null);
                                    }
                                }
                            }
                        }
                    }

                    //SeleniumModule module = new SeleniumModule();
                    //module.SetupTest();
                    //module.The1Test();
                    Thread.Sleep(60000);
                }
            }).Start();
        }
    }
}
