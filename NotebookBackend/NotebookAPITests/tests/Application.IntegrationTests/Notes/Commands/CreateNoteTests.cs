using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Application.Common.Mappings;
using NotebookAPI.Application.NoteCategory.Contracts.Request;
using NotebookAPI.Application.Notes.Commands.CreateNote;
using NotebookAPI.Infrastructure.Persistence;
using NUnit.Framework;

namespace NotebookAPITests.tests.Application.IntegrationTests.Notes.Commands
{
    [TestFixture]
    public class CreateNoteTests
    {
        private DbContextOptions<DataContext> _options;
        private IMapper _mapper;
        private FakeData _fakeData;
        
        [SetUp]
        public void Setup()
        {
            _fakeData = new FakeData();
            
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Create Note Handler Test")
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
        public void CreateNote_CreateNewNote_ShouldHaveNewNoteInDatabase()
        {
            var expectedNewNotesLength = 4;
            var newNoteContent = "my new note";

            using(var context = new DataContext(_options))
            {
                var createNoteHandler = new CreateNoteHandler(context, _mapper);
                var query = new CreateNoteCommand
                {
                    Content = newNoteContent,
                };
                
                createNoteHandler.Handle(query, new System.Threading.CancellationToken());

                var numberOfAllNotes = context.Notes.Count();
                
                Assert.AreEqual(expectedNewNotesLength, numberOfAllNotes);
            }
        }
        
        [Test]
        public void CreateNote_CreateNewNoteWithContent_ShouldReturnNewNoteContent()
        {
            var newNoteContent = "my new note";

            using(var context = new DataContext(_options))
            {
                var createNoteHandler = new CreateNoteHandler(context, _mapper);
                var query = new CreateNoteCommand
                {
                    Content = newNoteContent,
                };
                
                var result = createNoteHandler
                    .Handle(query, new System.Threading.CancellationToken()).Result;

                var newNoteFromDb = context.Notes.Find(result.Id);
                
                Assert.AreEqual(newNoteContent, newNoteFromDb.Content);
            }
        }
        
        [Test]
        public void CreateNote_CreateNewNoteWithCategories_ShouldReturnListWithNoteCategories()
        {
            var expectedCategoriesListLength = 1;
            var newNoteContent = "my new note";
            var categoriesToAdd = new List<NoteCategoryRequest>();
            var homeCategory = new NoteCategoryRequest {Id = 1};
            categoriesToAdd.Add(homeCategory);

            using (var context = new DataContext(_options))
            {
                var createNoteHandler = new CreateNoteHandler(context, _mapper);
                var query = new CreateNoteCommand
                {
                    Content = newNoteContent,
                    Categories = categoriesToAdd
                };

                var result = createNoteHandler
                    .Handle(query, new System.Threading.CancellationToken()).Result;

                Assert.AreEqual(expectedCategoriesListLength, result.Categories.Count());
            }
        }
    }
}