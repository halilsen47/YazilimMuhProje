using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositries.configuration
{
    public class SystemRequirementConfiguration : IEntityTypeConfiguration<SystemRequirementEntity>
    {
        public void Configure(EntityTypeBuilder<SystemRequirementEntity> builder)
        {
            builder.HasOne(s => s.User)
               .WithOne(u => u.SystemRequirement)
               .HasForeignKey<SystemRequirementEntity>(s => s.UserID);

            builder.HasOne(s => s.Application)
                   .WithOne(a => a.SystemRequirement)
                   .HasForeignKey<SystemRequirementEntity>(s => s.ApplicationId);

        }
    }
}
