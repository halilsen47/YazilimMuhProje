using Microsoft.EntityFrameworkCore.Migrations;
using Repositries.Migrations;
using System.Reflection;
using Xunit;

namespace Repositories.Tests
{
    public class baseentitiesTests
    {
        [Fact]
        public void baseentities_ShouldInheritFromMigration()
        {
            // Arrange
            var type = typeof(baseentities);

            // Act & Assert
            Assert.True(typeof(Migration).IsAssignableFrom(type));
        }

        [Fact]
        public void baseentities_ShouldHaveMigrationAttributeWithCorrectId()
        {
            // Arrange
            var type = typeof(baseentities);

            // Act
            var attribute = type.GetCustomAttribute<MigrationAttribute>();

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("20260506064703_baseentities", attribute.Id);
        }

        [Fact]
        public void baseentities_CanBeInstantiated()
        {
            // Act
            var migration = new baseentities();

            // Assert
            Assert.NotNull(migration);
        }
    }
}
