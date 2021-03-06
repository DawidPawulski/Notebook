using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using NotebookAPI.Application.Categories.Contracts.Response;
using NotebookAPI.Domain.Entities;
using Npgsql;

namespace NotebookAPI.Application.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryResponse>>
    {
    }
    
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public GetAllCategoriesHandler(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }
        
        public async Task<List<CategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            List<Category> categories;

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                categories = connection.Query<Category>("SELECT * FROM \"Categories\"").AsList();
            }

            var response = _mapper.Map<List<CategoryResponse>>(categories);
            
            return response;
        }
    }
}