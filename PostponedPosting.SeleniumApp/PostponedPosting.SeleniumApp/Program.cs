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

namespace PostponedPosting.SeleniumApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //NinjectHelper.InitializeKernal();
            //var userRepository = NinjectHelper.kernal.Get<IRepository<ApplicationUser>>();
            //var users = userRepository.GetAll();
            SeleniumModule module = new SeleniumModule();
            module.SetupTest();
            module.The1Test();
        }
    }
}
