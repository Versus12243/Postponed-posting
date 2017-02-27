using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.SeleniumApp
{
    public static class NinjectHelper
    {
        public static IKernel kernal;
       
        public static void InitializeKernal()
        {
            kernal = new StandardKernel();
            kernal.Load(Assembly.GetExecutingAssembly());
        }

    }
}
