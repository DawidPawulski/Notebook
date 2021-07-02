using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Commands.Notes;
using NotebookAPI.Contracts.Requests;
using NotebookAPI.Data;
using NotebookAPI.Handlers.Notes;
using NotebookAPI.Mapping;
using NUnit.Framework;

namespace NotebookAPITests.tests.Application.IntegrationTests.Notes.Commands
{
    [TestFixture]
    public class UpdateNoteTests
    {
        private DbContextOptions<DataContext> _options;
        private IMapper _mapper;
        private FakeData _fakeData;
        
        [SetUp]
        public void Setup()
        {
            _fakeData = new FakeData();
            
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Update Note Handler Test")
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
        public void UpdateNote_UpdateFirstNote_ShouldReturnNewNoteContent()
        {
            var firstNoteId = 1;
            var newNoteContent = "update";

            using(var context = new DataContext(_options))
            {
                var updateNoteHandler = new UpdateNoteHandler(context, _mapper);
                var query = new UpdateNoteCommand
                {
                    Id = firstNoteId,
                    Content = newNoteContent,
                };
                
                updateNoteHandler.Handle(query, new System.Threading.CancellationToken());

                var updatedNoteFromDb = context.Notes.Find(firstNoteId);
                
                Assert.AreEqual(newNoteContent, updatedNoteFromDb.Content);
            }
        }
        
        [Test]
        public void UpdateNote_ChangeNoteCategoriesFromOneToTwo_ShouldReturnListWithTwoCategories()
        {
            var noteIdWithOneCategory = 3;
            var expectedNumberOfCategories = 2;
            var newNoteContent = "my new content";
            var categoriesToAdd = new List<NoteCategoryRequest>();
            var homeCategory = new NoteCategoryRequest {Id = 1};
            var workCategory = new NoteCategoryRequest {Id = 2};
            categoriesToAdd.Add(homeCategory);
            categoriesToAdd.Add(workCategory);

            using(var context = new DataContext(_options))
            {
                var updateNoteHandler = new UpdateNoteHandler(context, _mapper);
                var query = new UpdateNoteCommand
                {
                    Id = noteIdWithOneCategory,
                    Content = newNoteContent,
                    Categories = categoriesToAdd
                };
                
                var result = updateNoteHandler
                    .Handle(query, new System.Threading.CancellationToken()).Result;

                Assert.AreEqual(expectedNumberOfCategories, result.Categories.Count());
            }
        }
        
        [Test]
        public void UpdateNote_UpdateNotExistingNote_ShouldReturnNull()
        {
            var notExistingNote = 10;
            var newNoteContent = "update";

            using(var context = new DataContext(_options))
            {
                var updateNoteHandler = new UpdateNoteHandler(context, _mapper);
                var query = new UpdateNoteCommand
                {
                    Id = notExistingNote,
                    Content = newNoteContent,
                };
                
                var result = updateNoteHandler
                    .Handle(query, new System.Threading.CancellationToken()).Result;

                Assert.Null(result);
            }
        }
    }
}