using Entities.Concrate;
using Repositries.Context;
using Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositries.Concrate.EntityFramework
{
    public class EfApplicationRepository : EfRepositoryBase<ApplicationEntity, AppDbContext>, IApplicationRepository
    {
        public EfApplicationRepository(AppDbContext context) : base(context)
        {

        }
    }
}
