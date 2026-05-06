using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Repositries.Context;
using System;
using System.Linq;
using Xunit;

namespace Repositories.Tests
{
    public class AppDbContextTests
    {
        private DbContextOptions<AppDbContext> GetOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void AppDbContext_CanBeInstantiated()
        {
            // Arrange
            var options = GetOptions();

            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                Assert.NotNull(context);
            }
        }

        [Fact]
        public void AppDbContext_HasApplicationsDbSet()
        {
            // Arrange
            var options = GetOptions();

            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                Assert.NotNull(context.Applications);
            }
        }

        [Fact]
        public void AppDbContext_CanAddAndRetrieveApplicationEntity()
        {
            // Arrange
            var options = GetOptions();
            var application = new ApplicationEntity
            {
                ApplicationName = "Test Application",
                SystemRequirement = new SystemRequirementEntity
                {
                    Cpu = "Test CPU",
                    Gpu = "Test GPU",
                    Ram = "16GB",
                    Storage = "500GB SSD",
                    OperatingSystem = "Windows 11",
                    User = new UserEntity
                    {
                        userName = "testuser",
                        password = "password123"
                    }
                }
            };

            // Act
            using (var context = new AppDbContext(options))
            {
                context.Applications.Add(application);
                context.SaveChanges();
            }

            // Assert
            using (var context = new AppDbContext(options))
            {
                var savedApplication = context.Applications
                    .Include(a => a.SystemRequirement)
                    .ThenInclude(s => s.User)
                    .FirstOrDefault(a => a.ApplicationName == "Test Application");
                
                Assert.NotNull(savedApplication);
                Assert.Equal("Test Application", savedApplication.ApplicationName);
                Assert.True(savedApplication.Id > 0); 

                Assert.NotNull(savedApplication.SystemRequirement);
                Assert.Equal("Test CPU", savedApplication.SystemRequirement.Cpu);

                Assert.NotNull(savedApplication.SystemRequirement.User);
                Assert.Equal("testuser", savedApplication.SystemRequirement.User.userName);
            }
        }

        [Fact]
        public void AppDbContext_ModelCreating_AppliesConfigurationsFromAssembly()
        {
            // Arrange
            var options = GetOptions();

            // Act
            using (var context = new AppDbContext(options))
            {
                var model = context.Model;
                
                // Assert
                // We know that SystemRequirementConfiguration configures SystemRequirementEntity
                // So we can check if SystemRequirementEntity is part of the model.
                var systemRequirementType = model.FindEntityType(typeof(SystemRequirementEntity));
                
                Assert.NotNull(systemRequirementType);
                
                // Check for UserEntity relationship in SystemRequirementEntity
                var fkUser = systemRequirementType.GetForeignKeys()
                    .SingleOrDefault(fk => fk.PrincipalEntityType.ClrType == typeof(UserEntity));
                Assert.NotNull(fkUser);

                // Check for ApplicationEntity relationship in SystemRequirementEntity
                var fkApp = systemRequirementType.GetForeignKeys()
                    .SingleOrDefault(fk => fk.PrincipalEntityType.ClrType == typeof(ApplicationEntity));
                Assert.NotNull(fkApp);
            }
        }
    }
}
