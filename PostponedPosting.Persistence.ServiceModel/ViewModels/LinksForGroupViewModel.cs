using PostponedPosting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ServiceModel.ViewModels
{
    public class LinksForGroupViewModel: BaseEntityWithName_and_Id
    {
        public List<LinkViewModel> ListOfLinks { get; set; }
    }
}
