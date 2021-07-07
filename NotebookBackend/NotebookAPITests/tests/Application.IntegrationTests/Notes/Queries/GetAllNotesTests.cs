using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NotebookAPI.Application.Common.Mappings;
using NotebookAPI.Application.Notes.Queries.GetAllNotes;
using NUnit.Framework;

namespace NotebookAPITests.tests.Application.IntegrationTests.Notes.Queries
{
    [TestFixture]
    public class GetAllNotesTests
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
        public void GetAllNotes_ExecuteHandler_ShouldReturnListWithAllNotes()
        {
            var getAllNotesHandler = new GetAllNotesHandler(_mapper, _configuration);
            var query = new GetAllNotesQuery();
            var result = getAllNotesHandler
                .Handle(query, new System.Threading.CancellationToken()).Result;
            
            Assert.True(result.Count > 0);
            
        }
    }
}
