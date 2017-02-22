[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PostponedPosting.WebUI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PostponedPosting.WebUI.App_Start.NinjectWebCommon), "Stop")]

namespace PostponedPosting.WebUI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Domain.Core;
    using Persistence.Data;
    using Persistence.ApplicationService.Abstract;
    using Persistence.ApplicationService.Services;
    using System.Web.Http;
    using Common.Extensions;
    using Microsoft.AspNet.Identity;
    using Domain.Entities.Identity;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                // Install our Ninject-based IDependencyResolver into the Web API config
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDataContext>().To<DataContext>().InRequestScope();
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InRequestScope();
            kernel.Bind<IUser>().To<ApplicationUser>().InRequestScope();
            kernel.Bind<IPostingService>().To<PostingService>().InRequestScope();
            kernel.Bind<ISocialNetworksService>().To<SocialNetworksService>().InRequestScope();
            kernel.Bind<IUserService>().To<UserService>().InRequestScope();
            kernel.Bind<ICryptoService>().To<CryptoService>().InRequestScope();
            kernel.Bind<IGroupsOfLinksService>().To<GroupsOfLinksService>().InRequestScope();
            kernel.Bind<ILinksService>().To<LinksService>().InRequestScope();
        }        
    }
}
