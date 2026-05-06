using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrate
{
    public class UserEntity : BaseEntitiy
    {
        public string userName { get; set; }
        public string password { get; set; }
        // Navigation Property
        public SystemRequirementEntity SystemRequirement { get; set; }
    }
}
