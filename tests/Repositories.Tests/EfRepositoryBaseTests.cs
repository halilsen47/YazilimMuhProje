using Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositries.Concrate.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Repositories.Tests
{
    public class TestEntity : IEntitiy
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }
    }

    public class TestRepository : EfRepositoryBase<TestEntity, TestDbContext>
    {
        public TestRepository(TestDbContext context) : base(context)
        {
        }
    }

    public class EfRepositoryBaseTests
    {
        private DbContextOptions<TestDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Add_ShouldAddEntityToDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var entity = new TestEntity { Id = 1, Name = "Test Name" };

            using (var context = new TestDbContext(options))
            {
                var repository = new TestRepository(context);

                // Act
                repository.Add(entity);
            }

            // Assert
            using (var context = new TestDbContext(options))
            {
                Assert.Equal(1, context.TestEntities.Count());
                var addedEntity = context.TestEntities.Single();
                Assert.Equal(1, addedEntity.Id);
                Assert.Equal("Test Name", addedEntity.Name);
            }
        }

        [Fact]
        public void Delete_ShouldRemoveEntityFromDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var entity = new TestEntity { Id = 1, Name = "Test Name" };

            using (var context = new TestDbContext(options))
            {
                context.TestEntities.Add(entity);
                context.SaveChanges();
                // Detach entity so the repository gets a new instance or state change can happen correctly
                context.Entry(entity).State = EntityState.Detached;
            }

            using (var context = new TestDbContext(options))
            {
                var repository = new TestRepository(context);

                // Act
                repository.Delete(entity);
            }

            // Assert
            using (var context = new TestDbContext(options))
            {
                Assert.Empty(context.TestEntities);
            }
        }

        [Fact]
        public void Update_ShouldModifyEntityInDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var entity = new TestEntity { Id = 1, Name = "Original Name" };

            using (var context = new TestDbContext(options))
            {
                context.TestEntities.Add(entity);
                context.SaveChanges();
            }

            using (var context = new TestDbContext(options))
            {
                var repository = new TestRepository(context);
                var entityToUpdate = new TestEntity { Id = 1, Name = "Updated Name" };

                // Act
                repository.Update(entityToUpdate);
            }

            // Assert
            using (var context = new TestDbContext(options))
            {
                var updatedEntity = context.TestEntities.Single();
                Assert.Equal("Updated Name", updatedEntity.Name);
            }
        }

        [Fact]
        public void Get_ShouldReturnEntityMatchingFilter()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new TestDbContext(options))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test 1" });
                context.TestEntities.Add(new TestEntity { Id = 2, Name = "Test 2" });
                context.SaveChanges();
            }

            using (var context = new TestDbContext(options))
            {
                var repository = new TestRepository(context);

                // Act
                var result = repository.Get(e => e.Id == 2);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Id);
                Assert.Equal("Test 2", result.Name);
            }
        }

        [Fact]
        public void GetAll_WithoutFilter_ShouldReturnAllEntities()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new TestDbContext(options))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test 1" });
                context.TestEntities.Add(new TestEntity { Id = 2, Name = "Test 2" });
                context.SaveChanges();
            }

            using (var context = new TestDbContext(options))
            {
                var repository = new TestRepository(context);

                // Act
                var results = repository.GetAll();

                // Assert
                Assert.Equal(2, results.Count);
            }
        }

        [Fact]
        public void GetAll_WithFilter_ShouldReturnMatchingEntities()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new TestDbContext(options))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Match" });
                context.TestEntities.Add(new TestEntity { Id = 2, Name = "No Match" });
                context.TestEntities.Add(new TestEntity { Id = 3, Name = "Match" });
                context.SaveChanges();
            }

            using (var context = new TestDbContext(options))
            {
                var repository = new TestRepository(context);

                // Act
                var results = repository.GetAll(e => e.Name == "Match");

                // Assert
                Assert.Equal(2, results.Count);
                Assert.All(results, r => Assert.Equal("Match", r.Name));
            }
        }
    }
}
