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
    public class DeleteCategoryTests
    {
        private DbContextOptions<DataContext> _options;
        private IMapper _mapper;
        private FakeData _fakeData;
        
        [SetUp]
        public void Setup()
        {
            _fakeData = new FakeData();
            
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Delete Category Handler Test")
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
        public void DeleteCategory_DeleteFirstCategory_ShouldReturnListWithOneCategoryLess()
        {
            var firstCategoryId = 1;
            var expectedListCount = 3;

            using(var context = new DataContext(_options))
            {
                var deleteCategoryHandler = new DeleteCategoryHandler(context, _mapper);
                var query = new DeleteCategoryCommand(firstCategoryId);

                deleteCategoryHandler.Handle(query, new System.Threading.CancellationToken());

                var allCategoriesListCount = context.Categories.Count();
                
                Assert.AreEqual(expectedListCount, allCategoriesListCount);
            }
        }
        
        [Test]
        public void DeleteCategory_DeleteNotExistingCategory_ShouldReturnNull()
        {
            var notExistingNote = 10;

            using(var context = new DataContext(_options))
            {
                var deleteCategoryHandler = new DeleteCategoryHandler(context, _mapper);
                var query = new DeleteCategoryCommand(notExistingNote);

                var result = deleteCategoryHandler
                    .Handle(query, new System.Threading.CancellationToken()).Result;

                Assert.Null(result);
            }
        }
    }
}