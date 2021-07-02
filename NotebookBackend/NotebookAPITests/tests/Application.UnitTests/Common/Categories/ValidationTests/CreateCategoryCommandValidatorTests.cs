using System.Threading.Tasks;
using NotebookAPI.Commands.Categories;
using NotebookAPI.Validation.Categories;
using NUnit.Framework;

namespace NotebookAPITests.tests.Application.UnitTests.Common.Categories.ValidationTests
{
    [TestFixture]
    public class CreateCategoryCommandValidatorTests
    {
        [Test]
        public async Task CreateCategoryCommandValidator_CreateCategoryWithName_ShouldBeValidatedToTrue()
        {
            var validator = new CreateCategoryCommandValidator();
            var createCategoryCommand = new CreateCategoryCommand
            {
                Name = "category name"
            };

            var validationResult = await validator.ValidateAsync(createCategoryCommand);

            Assert.True(validationResult.IsValid);
        }
        
        [Test]
        public async Task CreateCategoryCommandValidator_CreateCategoryToEmptyName_ShouldBeValidatedToFalse()
        {
            var validator = new CreateCategoryCommandValidator();
            var createCategoryCommand = new CreateCategoryCommand
            {
                Name = ""
            };

            var validationResult = await validator.ValidateAsync(createCategoryCommand);

            Assert.False(validationResult.IsValid);
        }
        
        [Test]
        public async Task CreateCategoryCommandValidator_CreateCategoryNameWithForbiddenCharacters_ShouldBeValidatedToFalse()
        {
            var validator = new CreateCategoryCommandValidator();
            var createCategoryCommand = new CreateCategoryCommand
            {
                Name = "Home!"
            };

            var validationResult = await validator.ValidateAsync(createCategoryCommand);

            Assert.False(validationResult.IsValid);
        }
    }
}