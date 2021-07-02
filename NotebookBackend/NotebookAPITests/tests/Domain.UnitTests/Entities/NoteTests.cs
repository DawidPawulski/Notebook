using System.Linq;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Data;
using NotebookAPI.Models;
using NUnit.Framework;

namespace NotebookAPITests.tests.Domain.UnitTests.Entities
{
    [TestFixture]
    public class NoteTests
    {
        private DbContextOptions<DataContext> _options;
        private FakeData _fakeData;
        
        [SetUp]
        public void Setup()
        {
            _fakeData = new FakeData();
            
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Note Test")
                .Options;

            using(var context = new DataContext(_options))
            {
                _fakeData.SeedData(context);
            }
        }

        [Test]
        public void Note_GetFirstNote_ContentPropertyShouldReturnCorrectValue()
        {
            Note note;
            var firstNoteId = 1;
            
            using(var context = new DataContext(_options))
            {
                note = context.Notes.Find(firstNoteId);
            }

            Assert.AreEqual("first note", note.Content);
        }
        
        [Test]
        public void Note_GetAllCategoriesForFirstNote_ShouldReturnListWithThreeElements()
        {
            Note note;
            var firstNoteId = 1;
            var expectedListCount = 3;
            
            using(var context = new DataContext(_options))
            {
                note = context.Notes.Include(x => x.NoteCategories)
                        .ThenInclude(x => x.Category)
                        .FirstOrDefault(x => x.Id == firstNoteId);
            }

            Assert.AreEqual(expectedListCount, note.NoteCategories.Count);
        }
        
        [Test]
        public void Note_GetAllCategoriesForSecondNote_ShouldReturnEmptyList()
        {
            Note note;
            var secondNoteId = 1;
            var expectedListCount = 3;
            
            using(var context = new DataContext(_options))
            {
                note = context.Notes.Include(x => x.NoteCategories)
                        .ThenInclude(x => x.Category)
                        .FirstOrDefault(x => x.Id == secondNoteId);
            }

            Assert.AreEqual(expectedListCount, note.NoteCategories.Count);
        }
        
        [Test]
        public void Note_GetNotExistingNote_ShouldBeNull()
        {
            Note note;
            var wrongNoteId = 100;
            
            using(var context = new DataContext(_options))
            {
                note = context.Notes.Find(wrongNoteId);
            }
            Assert.Null(note);
        }
    }
}