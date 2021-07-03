using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotebookAPI.Application.Categories.Commands.CreateCategory;
using NotebookAPI.Application.Categories.Commands.DeleteCategory;
using NotebookAPI.Application.Categories.Commands.UpdateCategory;
using NotebookAPI.Application.Categories.Queries.GetAllCategories;
using NotebookAPI.Application.Categories.Queries.GetCategoryById;

namespace NotebookAPI.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        // GET: api/categories/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var query = new GetCategoryByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult) Ok(result) : NotFound("Sorry, we couldn't find your category");
        }

        // POST api/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction("GetCategory", new {id = result.Id}, result);
        }

        // PUT: api/categories/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result != null ? (IActionResult) CreatedAtAction("GetCategory", new {id = result.Id}, result) 
                : NotFound("Sorry, we couldn't find the category you wanted to update");
        }
        
        // DELETE: api/categories/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));
            return result != null ? (IActionResult) Ok("Category deleted") 
                : NotFound("Sorry, we couldn't find the category you wanted to delete");
        }
    }
}