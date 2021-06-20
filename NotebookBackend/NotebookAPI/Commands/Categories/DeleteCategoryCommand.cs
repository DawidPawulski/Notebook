using MediatR;
using NotebookAPI.Contracts.Responses;

namespace NotebookAPI.Commands.Categories
{
    public class DeleteCategoryCommand : IRequest<CategoryResponse>
    {
        public int Id { get; set; }

        public DeleteCategoryCommand(int id)
        {
            Id = id;
        }
    }
}