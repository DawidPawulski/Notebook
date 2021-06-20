using MediatR;
using NotebookAPI.Contracts.Responses;

namespace NotebookAPI.Commands.Categories
{
    public class CreateCategoryCommand : IRequest<CategoryResponse>
    {
        public string Name { get; set; }

        public CreateCategoryCommand(){}
        
        public CreateCategoryCommand(string name)
        {
            Name = name;
        }
    }
}