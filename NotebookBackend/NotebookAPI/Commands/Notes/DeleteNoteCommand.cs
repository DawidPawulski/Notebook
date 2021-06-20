using MediatR;
using NotebookAPI.Contracts.Responses;

namespace NotebookAPI.Commands.Notes
{
    public class DeleteNoteCommand : IRequest<NoteResponse>
    {
        public int Id { get; set; }

        public DeleteNoteCommand(int id)
        {
            Id = id;
        }
    }
}