using FluentValidation;
using NotebookAPI.Commands.Notes;

namespace NotebookAPI.Validation.Notes
{
    public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteCommandValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}