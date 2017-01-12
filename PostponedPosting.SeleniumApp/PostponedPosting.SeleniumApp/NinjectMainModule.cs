using Ninject;
using Ninject.Modules;
using PostponedPosting.Domain.Core;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ApplicationService.Services;
using PostponedPosting.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.SeleniumApp
{
    public static class NinjectMainModule
    {
        private static IKernel _kernel = null;       

        private static IKernel GetKernel()
        {
            return _kernel ?? CreateKernel();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                RegisterServices(kernel);               
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
            kernel.Bind<IDataContext>().To<DataContext>();
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));
        }
    }
}
