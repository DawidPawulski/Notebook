using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Application.Common.Mappings;
using NotebookAPI.Application.Notes.Commands.DeleteNote;
using NotebookAPI.Infrastructure.Persistence;
using NUnit.Framework;

namespace NotebookAPITests.tests.Application.IntegrationTests.Notes.Commands
{
    [TestFixture]
    public class DeleteNoteTests
    {
        private DbContextOptions<DataContext> _options;
        private IMapper _mapper;
        private FakeData _fakeData;
        
        [SetUp]
        public void Setup()
        {
            _fakeData = new FakeData();
            
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Delete Note Handler Test")
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
        public void DeleteNote_DeleteFirstNote_ShouldReturnListWithOneNoteLess()
        {
            var firstNoteId = 1;
            var expectedListCount = 2;

            using(var context = new DataContext(_options))
            {
                var deleteNoteHandler = new DeleteNoteHandler(context, _mapper);
                var query = new DeleteNoteCommand(firstNoteId);

                deleteNoteHandler.Handle(query, new System.Threading.CancellationToken());

                var allNotesListCount = context.Notes.Count();
                
                Assert.AreEqual(expectedListCount, allNotesListCount);
            }
        }
        
        [Test]
        public void DeleteNote_DeleteNotExistingNote_ShouldReturnNull()
        {
            var notExistingNote = 10;

            using(var context = new DataContext(_options))
            {
                var deleteNoteHandler = new DeleteNoteHandler(context, _mapper);
                var query = new DeleteNoteCommand(notExistingNote);

                var result = deleteNoteHandler
                    .Handle(query, new System.Threading.CancellationToken()).Result;

                Assert.Null(result);
            }
        }
    }
}