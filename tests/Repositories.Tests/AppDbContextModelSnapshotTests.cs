using Microsoft.EntityFrameworkCore.Infrastructure;
using Repositries.Context;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Repositories.Tests
{
    public class AppDbContextModelSnapshotTests
    {
        private Type GetSnapshotType()
        {
            // The snapshot class is generated as 'internal' by default in EF Core.
            // Since it's not public, we use reflection to find it in the Repositries assembly
            // instead of modifying auto-generated code.
            var assembly = typeof(AppDbContext).Assembly;
            return assembly.GetTypes().SingleOrDefault(t => t.Name == "AppDbContextModelSnapshot");
        }

        [Fact]
        public void AppDbContextModelSnapshot_ShouldExist()
        {
            var type = GetSnapshotType();
            Assert.NotNull(type);
        }

        [Fact]
        public void AppDbContextModelSnapshot_ShouldInheritFromModelSnapshot()
        {
            var type = GetSnapshotType();
            Assert.True(typeof(ModelSnapshot).IsAssignableFrom(type));
        }

        [Fact]
        public void AppDbContextModelSnapshot_ShouldHaveDbContextAttributeWithAppDbContext()
        {
            var type = GetSnapshotType();
            var attribute = type.GetCustomAttribute<DbContextAttribute>();

            Assert.NotNull(attribute);
            Assert.Equal(typeof(AppDbContext), attribute.ContextType);
        }

        [Fact]
        public void AppDbContextModelSnapshot_CanBeInstantiated()
        {
            var type = GetSnapshotType();
            var snapshot = Activator.CreateInstance(type);

            Assert.NotNull(snapshot);
        }
    }
}
