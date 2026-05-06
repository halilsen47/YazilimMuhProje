using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Repositries.configuration;
using System.Linq;
using Xunit;

namespace Repositories.Tests
{
    public class SystemRequirementTestDbContext : DbContext
    {
        public DbSet<SystemRequirementEntity> SystemRequirements { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ApplicationEntity> Applications { get; set; }

        public SystemRequirementTestDbContext(DbContextOptions<SystemRequirementTestDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SystemRequirementConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class SystemRequirementConfigurationTests
    {
        private DbContextOptions<SystemRequirementTestDbContext> GetOptions()
        {
            return new DbContextOptionsBuilder<SystemRequirementTestDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Configure_ShouldSetOneToOneRelationshipWithUser()
        {
            // Arrange
            var options = GetOptions();

            using (var context = new SystemRequirementTestDbContext(options))
            {
                // Act
                var entityType = context.Model.FindEntityType(typeof(SystemRequirementEntity));
                var foreignKey = entityType.GetForeignKeys()
                    .SingleOrDefault(fk => fk.PrincipalEntityType.ClrType == typeof(UserEntity));

                // Assert
                Assert.NotNull(foreignKey);
                Assert.True(foreignKey.IsUnique); // One-to-one mapping implies a unique constraint on the FK
                Assert.Equal("UserID", foreignKey.Properties.First().Name);
                Assert.Equal("User", foreignKey.DependentToPrincipal.Name);
                Assert.Equal("SystemRequirement", foreignKey.PrincipalToDependent.Name);
            }
        }

        [Fact]
        public void Configure_ShouldSetOneToOneRelationshipWithApplication()
        {
            // Arrange
            var options = GetOptions();

            using (var context = new SystemRequirementTestDbContext(options))
            {
                // Act
                var entityType = context.Model.FindEntityType(typeof(SystemRequirementEntity));
                var foreignKey = entityType.GetForeignKeys()
                    .SingleOrDefault(fk => fk.PrincipalEntityType.ClrType == typeof(ApplicationEntity));

                // Assert
                Assert.NotNull(foreignKey);
                Assert.True(foreignKey.IsUnique); // One-to-one mapping implies a unique constraint on the FK
                Assert.Equal("ApplicationId", foreignKey.Properties.First().Name);
                Assert.Equal("Application", foreignKey.DependentToPrincipal.Name);
                Assert.Equal("SystemRequirement", foreignKey.PrincipalToDependent.Name);
            }
        }
    }
}
