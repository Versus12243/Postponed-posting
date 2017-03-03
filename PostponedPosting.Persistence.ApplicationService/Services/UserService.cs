using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.CredentialModel;
using PostponedPosting.Domain.Entities.Identity;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Services
{
    public class UserService: IUserService
    {
        [Inject]
        public IRepository<UserCredentials> UserCredetialsRepository { get; set; }

        [Inject]
        public IRepository<ApplicationUser> UserRepository { get; set; }

        [Inject]
        public IRepository<SocialNetwork> SocialNetworkRepository { get; set; }

        [Inject]
        public IRepository<UserSocialNetwork> UserSocialNetworkRepository { get; set; }

        [Inject]
        public ICryptoService CryptoService { get; set; }

        public CredentialsViewModel GetCredentials(int socialNetworkId, string userId)
        {
            var credentialsForSN = UserSocialNetworkRepository.Find(t => t.User.Id == userId && t.SocialNetwork.Id == socialNetworkId);

            CredentialsViewModel model = new CredentialsViewModel();

            if(credentialsForSN != null && credentialsForSN.Credentials != null)
            {
                model.Login = CryptoService.GetDecryptedValue(credentialsForSN.User.PasswordHash, credentialsForSN.Credentials.Login);
                model.Password = CryptoService.GetDecryptedValue(credentialsForSN.User.PasswordHash, credentialsForSN.Credentials.Password);
                model.SocialNetworkId = credentialsForSN.SocialNetwork.Id;             
            }

            model.SocialNetworkId = socialNetworkId;

            return model;
        }

        public int SaveCredentials(CredentialsViewModel model, string userId)
        {
            var user = UserRepository.Find(u => u.Id == userId);
            var sn = SocialNetworkRepository.Find(s => s.Id == model.SocialNetworkId);
            var usn = UserSocialNetworkRepository.Find(r => r.User.Id == userId && r.SocialNetwork.Id == model.SocialNetworkId);

            if (user != null && sn != null)
            {
                var cred = new UserCredentials();
                cred.Login = CryptoService.GetEncryptedValue(user.PasswordHash, model.Login);
                cred.Password = CryptoService.GetEncryptedValue(user.PasswordHash, model.Password);
                cred.EntityStatus = Domain.Entities.StatusEnums.EntityStatus.Active;
                cred.Status = Domain.Entities.StatusEnums.CredentialsStatus.Active;
                if (usn != null)
                {
                    usn.Credentials = cred;
                    UserSocialNetworkRepository.Update(usn);
                }
                else
                {                  
                    var userSN = new UserSocialNetwork();
                    userSN.SocialNetwork = sn;
                    userSN.User = user;
                    userSN.Credentials = cred;
                    UserSocialNetworkRepository.Insert(userSN);
                }
                return cred.Id;     
            }
            else
            {
                throw new Exception("User or social network was not found");
            }            
        }
    }
}
