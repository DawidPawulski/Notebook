using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NotebookAPI.Handlers.Notes;
using NotebookAPI.Mapping;
using NotebookAPI.Queries.Notes;
using NUnit.Framework;

namespace NotebookAPITests.HandlerTests.Notes
{
    [TestFixture]
    public class GetAllNotesHandlerTests
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        
        [SetUp]
        public void Setup()
        {
            var testConfiguration = new Dictionary<string, string>
            {
                {"ConnectionStrings:DefaultConnection", "Server=127.0.0.1; port=5432; user id = postgres; password = coderslab; database=notebookDB; pooling = true"},
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
        public void GetAllNotesHandler_ExecuteHandler_ShouldReturnListWithAllNotes()
        {
            var getAllNotesHandler = new GetAllNotesHandler(_mapper, _configuration);
            var query = new GetAllNotesQuery();
            var result = getAllNotesHandler
                .Handle(query, new System.Threading.CancellationToken()).Result;
            
            Assert.True(result.Count > 0);
            
        }
    }
}