using System.Threading.Tasks;
using NotebookAPI.Commands.Notes;
using NotebookAPI.Validation.Notes;
using NUnit.Framework;

namespace NotebookAPITests.ValidationTests.Notes
{
    [TestFixture]
    public class UpdateNoteCommandValidatorTests
    {
        [Test]
        public async Task UpdateNoteCommandValidator_UpdateNoteWithContent_ShouldBeValidatedToTrue()
        {
            var validator = new UpdateNoteCommandValidator();
            var updateNoteCommand = new UpdateNoteCommand
            {
                Content = "note content"
            };

            var validationResult = await validator.ValidateAsync(updateNoteCommand);

            Assert.True(validationResult.IsValid);
        }
        
        [Test]
        public async Task UpdateNoteCommandValidator_UpdateNoteToEmptyContent_ShouldBeValidatedToFalse()
        {
            var validator = new UpdateNoteCommandValidator();
            var updateNoteCommand = new UpdateNoteCommand
            {
                Content = ""
            };

            var validationResult = await validator.ValidateAsync(updateNoteCommand);

            Assert.False(validationResult.IsValid);
        }
    }
}