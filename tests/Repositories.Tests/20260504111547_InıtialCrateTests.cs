using Microsoft.EntityFrameworkCore.Migrations;
using Repositries.Migrations;
using System.Reflection;
using Xunit;

namespace Repositories.Tests
{
    public class InıtialCrateTests
    {
        [Fact]
        public void InıtialCrate_ShouldInheritFromMigration()
        {
            // Arrange
            var type = typeof(InıtialCrate);

            // Act & Assert
            Assert.True(typeof(Migration).IsAssignableFrom(type));
        }

        [Fact]
        public void InıtialCrate_ShouldHaveMigrationAttributeWithCorrectId()
        {
            // Arrange
            var type = typeof(InıtialCrate);

            // Act
            var attribute = type.GetCustomAttribute<MigrationAttribute>();

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("20260504111547_InıtialCrate", attribute.Id);
        }

        [Fact]
        public void InıtialCrate_CanBeInstantiated()
        {
            // Act
            var migration = new InıtialCrate();

            // Assert
            Assert.NotNull(migration);
        }
    }
}
