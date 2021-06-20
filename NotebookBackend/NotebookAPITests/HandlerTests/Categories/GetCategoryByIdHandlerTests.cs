using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NotebookAPI.Contracts.Responses;
using NotebookAPI.Handlers.Categories;
using NotebookAPI.Mapping;
using NotebookAPI.Queries.Categories;
using NUnit.Framework;

namespace NotebookAPITests.HandlerTests.Categories
{
    [TestFixture]
    public class GetCategoryByIdHandlerTests
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
        public void GetCategoryByIdHandler_GetFirstCategory_ShouldReturnFirstCategory()
        {
            var getAllCategoriesHandler = new GetAllCategoriesHandler(_mapper, _configuration);
            var allCategoriesQuery = new GetAllCategoriesQuery();
            var firstCategory = getAllCategoriesHandler
                .Handle(allCategoriesQuery, new System.Threading.CancellationToken()).Result.FirstOrDefault();

            var getCategoryByIdHandler = new GetCategoryByIdHandler(_mapper, _configuration);
            var query = new GetCategoryByIdQuery(firstCategory.Id);
            var result = getCategoryByIdHandler
                .Handle(query, new System.Threading.CancellationToken()).Result;
            
            Assert.IsInstanceOf(typeof(CategoryResponse), result);
            
        }
        
        [Test]
        public void GetCategoryByIdHandler_GetNotExistingCategory_ShouldReturnEmptyObject()
        {
            var notExistingCategoryId = -1;
  
            var getCategoryByIdHandler = new GetCategoryByIdHandler(_mapper, _configuration);
            var query = new GetCategoryByIdQuery(notExistingCategoryId);
            var result = getCategoryByIdHandler
                .Handle(query, new System.Threading.CancellationToken()).Result;
            
            Assert.Null(result);
        }
    }
}