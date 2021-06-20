using FluentValidation;
using NotebookAPI.Commands.Categories;

namespace NotebookAPI.Validation.Categories
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9_ ]*$");
        }
    }
}