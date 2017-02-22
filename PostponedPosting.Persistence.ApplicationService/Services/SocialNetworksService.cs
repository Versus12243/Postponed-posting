using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace PostponedPosting.Persistence.ApplicationService.Services
{
    public class SocialNetworksService: ISocialNetworksService
    {
        [Inject]
        public IRepository<SocialNetwork> SNRepository { get; set; }
        [Inject]
        public IRepository<UserSocialNetwork> UserSNRepository { get; set; }

        public List<SocialNetwork> GetSocialNetworks()
        {
            return SNRepository.GetAll().ToList();
        }

        public List<AvailableSocialNetworkViewModel> GetAviableSNForUser(string UserId)
        {
            var model = new List<AvailableSocialNetworkViewModel>();
            var socialNetworks = GetSocialNetworks().Where(t => t.Status == Domain.Entities.StatusEnums.EntityStatus.Active
                                                            && t.Aviability == Domain.Entities.StatusEnums.SocialNetworkAvailabily.Available);
            var userSN = UserSNRepository.FindAll(s => s.User.Id == UserId);

            foreach(var network in userSN)
            {
                var sn = new AvailableSocialNetworkViewModel();
                sn.Id = network.Id;
                sn.CredentialsExist = network.Credentials != null;
                sn.CredentialsIsActive = sn.CredentialsExist && network.Credentials.Status == Domain.Entities.StatusEnums.EntityStatus.Active;
                sn.Name = network.SocialNetwork.Name;
                sn.IsActive = sn.CredentialsIsActive && network.Status == Domain.Entities.StatusEnums.EntityStatus.Active;
                sn.SocialNetworkId = network.SocialNetwork.Id;
                sn.SocialNetworkIsActive = network.SocialNetwork.Status == Domain.Entities.StatusEnums.EntityStatus.Active;
                sn.SocialNetworkIsAvailable = network.SocialNetwork.Aviability == Domain.Entities.StatusEnums.SocialNetworkAvailabily.Available;
                model.Add(sn);
            }

            var unusedSN = socialNetworks.Where(t => !model.Select(n => n.Name).Contains(t.Name));

            foreach(var network in unusedSN)
            {
                var sn = new AvailableSocialNetworkViewModel();
                sn.CredentialsExist = false;
                sn.CredentialsIsActive = false;
                sn.Name = network.Name;
                sn.IsActive = false;
                sn.SocialNetworkId = network.Id;
                model.Add(sn);
            }

            return model;
        }
    }
}
