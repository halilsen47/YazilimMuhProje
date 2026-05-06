using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrate
{
    public class SystemRequirementEntity : BaseEntitiy
    {
        public string Cpu { get; set; }
        public string Gpu { get; set; }
        public string Ram { get; set; }
        public string Storage { get; set; }
        public string OperatingSystem { get; set; }

        // Foreign Key
        public int? ApplicationId { get; set; }
        public ApplicationEntity Application { get; set; }
        // Foreign Key
        public int? UserID { get; set; }
        public UserEntity User { get; set; }   
    }
}
