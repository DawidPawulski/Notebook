using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Commands.Categories;
using NotebookAPI.Data;
using NotebookAPI.Handlers.Categories;
using NotebookAPI.Mapping;
using NUnit.Framework;

namespace NotebookAPITests.tests.Application.IntegrationTests.Categories.Commands
{
    [TestFixture]
    public class CreateCategoryTests
    {
        private DbContextOptions<DataContext> _options;
        private IMapper _mapper;
        private FakeData _fakeData;
        
        [SetUp]
        public void Setup()
        {
            _fakeData = new FakeData();
            
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Create Category Handler Test")
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
        public void CreateCategory_CreateNewCategory_ShouldHaveNewCategoryInDatabase()
        {
            var expectedNewCategoriesLength = 5;
            var newCategoryName = "Weekend";

            using(var context = new DataContext(_options))
            {
                var createCategoryHandler = new CreateCategoryHandler(context, _mapper);
                var query = new CreateCategoryCommand
                {
                    Name = newCategoryName,
                };
                
                createCategoryHandler.Handle(query, new System.Threading.CancellationToken());

                var numberOfAllCategories = context.Categories.Count();
                
                Assert.AreEqual(expectedNewCategoriesLength, numberOfAllCategories);
            }
        }
        
        [Test]
        public void CreateCategory_CreateNewCategoryWithName_ShouldReturnNewCategoryName()
        {
            var newCategoryName = "Weekend";

            using(var context = new DataContext(_options))
            {
                var createCategoryHandler = new CreateCategoryHandler(context, _mapper);
                var query = new CreateCategoryCommand
                {
                    Name = newCategoryName,
                };
                
                var result = createCategoryHandler
                    .Handle(query, new System.Threading.CancellationToken()).Result;

                var newCategoryFromDb = context.Categories.Find(result.Id);
                
                Assert.AreEqual(newCategoryName, newCategoryFromDb.Name);
            }
        }
    }
}