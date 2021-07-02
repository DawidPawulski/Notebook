using System.Linq;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Data;
using NotebookAPI.Models;
using NUnit.Framework;

namespace NotebookAPITests.tests.Domain.UnitTests.ModelsTests
{
    [TestFixture]
    public class CategoryTests
    {
        private DbContextOptions<DataContext> _options;
        private FakeData _fakeData;
        
        [SetUp]
        public void Setup()
        {
            _fakeData = new FakeData();
            
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Category Test")
                .Options;

            using(var context = new DataContext(_options))
            {
                _fakeData.SeedData(context);
            }
        }

        [Test]
        public void Category_GetFirstCategory_NamePropertyShouldReturnCorrectValue()
        {
            Category category;
            var firstCategoryId = 1;
            
            using(var context = new DataContext(_options))
            {
                category = context.Categories.Find(firstCategoryId);
            }

            Assert.AreEqual("Home", category.Name);
        }
        
        [Test]
        public void Category_GetAllNotesForWorkCategory_ShouldReturnListWithTwoElements()
        {
            Category category;
            var workCategoryId = 2;
            var expectedListCount = 2;
            
            using(var context = new DataContext(_options))
            {
                category = context.Categories.Include(x => x.NoteCategories)
                        .ThenInclude(x => x.Note)
                        .FirstOrDefault(x => x.Id == workCategoryId);
            }

            Assert.AreEqual(expectedListCount, category.NoteCategories.Count);
        }
        
        [Test]
        public void Category_GetAllNotesForSchoolCategory_ShouldReturnEmptyList()
        {
            Category category;
            var schoolCategoryId = 4;
            var expectedListCount = 0;
            
            using(var context = new DataContext(_options))
            {
                category = context.Categories.Include(x => x.NoteCategories)
                        .ThenInclude(x => x.Category)
                        .FirstOrDefault(x => x.Id == schoolCategoryId);
            }

            Assert.AreEqual(expectedListCount, category.NoteCategories.Count);
        }
        
        [Test]
        public void Category_GetNotExistingCategory_ShouldBeNull()
        {
            Category category;
            var wrongCategoryId = 100;
            
            using(var context = new DataContext(_options))
            {
                category = context.Categories.Find(wrongCategoryId);
            }
            Assert.Null(category);
        }
    }
}