using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Application.Categories.Commands.UpdateCategory;
using NotebookAPI.Application.Common.Mappings;
using NotebookAPI.Infrastructure.Persistence;
using NUnit.Framework;

namespace NotebookAPITests.tests.Application.IntegrationTests.Categories.Commands
{
    [TestFixture]
    public class UpdateCategoryTests
    {
        private DbContextOptions<DataContext> _options;
        private IMapper _mapper;
        private FakeData _fakeData;
        
        [SetUp]
        public void Setup()
        {
            _fakeData = new FakeData();
            
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Update Category Handler Test")
                .Options;

            using(var context = new DataContext(_options))
            {
                _fakeData.SeedData(context);
            }
            
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToResponse());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Test]
        public void UpdateCategory_UpdateFirstCategory_ShouldReturnNewCategoryName()
        {
            var firstCategoryId = 1;
            var newCategoryName = "update";

            using(var context = new DataContext(_options))
            {
                var updateCategoryHandler = new UpdateCategoryHandler(context, _mapper);
                var query = new UpdateCategoryCommand
                {
                    Id = firstCategoryId,
                    Name = newCategoryName
                };
                
                updateCategoryHandler.Handle(query, new System.Threading.CancellationToken());

                var updatedCategoryFromDb = context.Categories.Find(firstCategoryId);
                
                Assert.AreEqual(newCategoryName, updatedCategoryFromDb.Name);
            }
        }

        [Test]
        public void UpdateCategory_UpdateNotExistingCategory_ShouldReturnNull()
        {
            var notExistingCategory = 10;
            var newCategoryName = "update";

            using(var context = new DataContext(_options))
            {
                var updateCategoryHandler = new UpdateCategoryHandler(context, _mapper);
                var query = new UpdateCategoryCommand
                {
                    Id = notExistingCategory,
                    Name = newCategoryName
                };
                
                var result = updateCategoryHandler
                    .Handle(query, new System.Threading.CancellationToken()).Result;

                Assert.Null(result);
            }
        }
    }
}