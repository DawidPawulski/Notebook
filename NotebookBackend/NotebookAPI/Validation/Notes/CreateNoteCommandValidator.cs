using FluentValidation;
using NotebookAPI.Commands.Notes;

namespace NotebookAPI.Validation.Notes
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}