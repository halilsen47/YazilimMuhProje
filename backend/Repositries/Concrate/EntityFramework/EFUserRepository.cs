using Entities.Concrate;
using Repositries.Context;
using Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositries.Concrate.EntityFramework
{
    public class EFUserRepository : EfRepositoryBase<UserEntity, AppDbContext>, IUserRepository
    {
        public EFUserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
