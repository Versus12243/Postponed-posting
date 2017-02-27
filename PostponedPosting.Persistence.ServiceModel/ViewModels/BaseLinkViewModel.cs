using PostponedPosting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ServiceModel.ViewModels
{
    public class BaseLinkViewModel: BaseEntityWithName_and_Id
    {
        public DateTime DateOfCreation { get; set; }
    }
}
