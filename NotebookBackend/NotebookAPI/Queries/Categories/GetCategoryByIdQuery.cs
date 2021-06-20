using MediatR;
using NotebookAPI.Contracts.Responses;

namespace NotebookAPI.Queries.Categories
{
    public class GetCategoryByIdQuery : IRequest<CategoryResponse>
    {
        public int Id { get; set; }

        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }
}