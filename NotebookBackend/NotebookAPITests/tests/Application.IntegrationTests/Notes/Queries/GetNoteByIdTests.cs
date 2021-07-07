using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NotebookAPI.Application.Common.Mappings;
using NotebookAPI.Application.Notes.Contracts.Response;
using NotebookAPI.Application.Notes.Queries.GetAllNotes;
using NotebookAPI.Application.Notes.Queries.GetNoteById;
using NUnit.Framework;

namespace NotebookAPITests.tests.Application.IntegrationTests.Notes.Queries
{
    [TestFixture]
    public class GetNoteByIdTests
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        
        [SetUp]
        public void Setup()
        {
            var testConfiguration = new Dictionary<string, string>
            {
                {"ConnectionStrings:DefaultConnection", ""},
                {"AllowedHosts", "*"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfiguration)
                .Build();
            
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToResponse());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Test]
        public void GetNoteById_GetFirstNote_ShouldReturnFirstNote()
        {
            var getAllNotesHandler = new GetAllNotesHandler(_mapper, _configuration);
            var allNotesQuery = new GetAllNotesQuery();
            var firstNote= getAllNotesHandler
                .Handle(allNotesQuery, new System.Threading.CancellationToken()).Result.FirstOrDefault();

            var getNoteByIdHandler = new GetNoteByIdHandler(_mapper, _configuration);
            var query = new GetNoteByIdQuery(firstNote.Id);
            var result = getNoteByIdHandler
                .Handle(query, new System.Threading.CancellationToken()).Result;
            
            Assert.IsInstanceOf(typeof(NoteResponse), result);
            
        }
        
        [Test]
        public void GetNoteById_GetNotExistingNote_ShouldReturnEmptyObject()
        {
            var notExistingNoteId = -1;
                
            var getNoteByIdHandler = new GetNoteByIdHandler(_mapper, _configuration);
            var query = new GetNoteByIdQuery(notExistingNoteId);
            var result = getNoteByIdHandler
                .Handle(query, new System.Threading.CancellationToken()).Result;

            Assert.Null(result);
        }
    }
}
