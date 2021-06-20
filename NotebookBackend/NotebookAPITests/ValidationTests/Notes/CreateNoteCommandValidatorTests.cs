using System.Threading.Tasks;
using NotebookAPI.Commands.Notes;
using NotebookAPI.Validation.Notes;
using NUnit.Framework;

namespace NotebookAPITests.ValidationTests.Notes
{
    [TestFixture]
    public class CreateNoteCommandValidatorTests
    {
        [Test]
        public async Task CreateNoteCommandValidator_CreateNoteWithContent_ShouldBeValidatedToTrue()
        {
            var validator = new CreateNoteCommandValidator();
            var createNoteCommand = new CreateNoteCommand
            {
               Content = "note content"
            };

            var validationResult = await validator.ValidateAsync(createNoteCommand);

            Assert.True(validationResult.IsValid);
        }
        
        [Test]
        public async Task reateNoteCommandValidator_CreateNoteWithoutContent_ShouldBeValidatedToFalse()
        {
            var validator = new CreateNoteCommandValidator();
            var createNoteCommand = new CreateNoteCommand
            {
                Content = ""
            };

            var validationResult = await validator.ValidateAsync(createNoteCommand);

            Assert.False(validationResult.IsValid);
        }
    }
}