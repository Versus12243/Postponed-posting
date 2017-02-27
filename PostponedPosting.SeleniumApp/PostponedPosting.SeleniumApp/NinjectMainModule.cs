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
    public class NinjectBindings : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IDataContext>().To<DataContext>();
            Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            Bind<IPostingService>().To<PostingService>();
            Bind<ICryptoService>().To<CryptoService>();
        }
    }
}
