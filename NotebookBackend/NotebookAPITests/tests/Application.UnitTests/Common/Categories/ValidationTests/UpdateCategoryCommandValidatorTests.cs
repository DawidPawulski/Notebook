using System.Threading.Tasks;
using NotebookAPI.Commands.Categories;
using NotebookAPI.Validation.Categories;
using NUnit.Framework;

namespace NotebookAPITests.tests.Application.UnitTests.Common.Categories.ValidationTests
{
    [TestFixture]
    public class UpdateCategoryCommandValidatorTests
    {
        [Test]
        public async Task UpdateCategoryCommandValidator_UpdateCategoryWithName_ShouldBeValidatedToTrue()
        {
            var validator = new UpdateCategoryCommandValidator();
            var updateCategoryCommand = new UpdateCategoryCommand
            {
                Name = "category name"
            };

            var validationResult = await validator.ValidateAsync(updateCategoryCommand);

            Assert.True(validationResult.IsValid);
        }
        
        [Test]
        public async Task UpdateCategoryCommandValidator_UpdateCategoryToEmptyName_ShouldBeValidatedToFalse()
        {
            var validator = new UpdateCategoryCommandValidator();
            var updateCategoryCommand = new UpdateCategoryCommand
            {
                Name = ""
            };

            var validationResult = await validator.ValidateAsync(updateCategoryCommand);

            Assert.False(validationResult.IsValid);
        }
        
        [Test]
        public async Task UpdateCategoryCommandValidator_UpdateCategoryToNameWithForbiddenCharacters_ShouldBeValidatedToFalse()
        {
            var validator = new UpdateCategoryCommandValidator();
            var updateCategoryCommand = new UpdateCategoryCommand
            {
                Name = "Home!"
            };

            var validationResult = await validator.ValidateAsync(updateCategoryCommand);

            Assert.False(validationResult.IsValid);
        }
    }
}