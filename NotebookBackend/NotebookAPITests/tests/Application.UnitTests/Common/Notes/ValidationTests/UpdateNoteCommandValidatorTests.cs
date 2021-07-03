using System.Threading.Tasks;
using NotebookAPI.Application.Notes.Commands.UpdateNote;
using NUnit.Framework;

namespace NotebookAPITests.tests.Application.UnitTests.Common.Notes.ValidationTests
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