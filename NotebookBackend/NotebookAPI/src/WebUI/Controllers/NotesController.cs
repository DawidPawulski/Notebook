using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotebookAPI.Application.Notes.Commands.CreateNote;
using NotebookAPI.Application.Notes.Commands.DeleteNote;
using NotebookAPI.Application.Notes.Commands.UpdateNote;
using NotebookAPI.Application.Notes.Queries.GetAllNotes;
using NotebookAPI.Application.Notes.Queries.GetNoteById;

namespace NotebookAPI.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/notes
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var query = new GetAllNotesQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        
        // GET: api/notes/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote(int id)
        {
            var query = new GetNoteByIdQuery(id);
            var result = await _mediator.Send(query);
            
            return result != null ? (IActionResult) Ok(result) : NotFound("Sorry, we couldn't find your note");
        }

        // POST api/notes
        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteCommand command)
        {
            var result = await _mediator.Send(command);
            
            return CreatedAtAction("GetNote", new {id = result.Id}, result);
        }

        // PUT: api/notes/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] UpdateNoteCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            
            return result != null ? (IActionResult) CreatedAtAction("GetNote", new {id = result.Id}, result) 
                : NotFound("Sorry, we couldn't find the note you wanted to update");
        }
        
        // DELETE: api/notes/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var result = await _mediator.Send(new DeleteNoteCommand(id));
            
            return result != null ? (IActionResult) Ok("Note deleted") 
                : NotFound("Sorry, we couldn't find the note you wanted to delete");
        }
    }
}