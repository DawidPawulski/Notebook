using System.Collections.Generic;
using MediatR;
using NotebookAPI.Contracts.Responses;

namespace NotebookAPI.Queries.Categories
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryResponse>>
    {
        
    }
}