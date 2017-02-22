using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Domain.Entities
{
    public class BaseEntityWithName_and_Id
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
