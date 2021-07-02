using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using NotebookAPI.Contracts.Responses;
using NotebookAPI.Data;
using NotebookAPI.Models;
using NotebookAPI.Queries.Categories;
using Npgsql;

namespace NotebookAPI.Handlers.Categories
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public GetCategoryByIdHandler(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }
        
        public async Task<CategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            Category category;

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                category = connection.Query<Category>
                    ($"SELECT * FROM \"Categories\" WHERE \"Id\" = {request.Id}").FirstOrDefault();
            }
            
            var response = _mapper.Map<CategoryResponse>(category);
            
            return response;
        }
    }
}