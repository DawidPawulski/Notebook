using FluentValidation;
using NotebookAPI.Commands.Categories;

namespace NotebookAPI.Validation.Categories
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9_ ]*$");
        }
    }
}