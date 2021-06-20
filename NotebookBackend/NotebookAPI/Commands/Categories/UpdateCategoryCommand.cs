using MediatR;
using NotebookAPI.Contracts.Responses;

namespace NotebookAPI.Commands.Categories
{
    public class UpdateCategoryCommand : IRequest<CategoryResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UpdateCategoryCommand(){}
        
        public UpdateCategoryCommand(string name)
        {
            Name = name;
        }
    }
}