using MediatR;
using NotebookAPI.Contracts.Responses;

namespace NotebookAPI.Queries.Notes
{
    public class GetNoteByIdQuery : IRequest<NoteResponse>
    {
        public int Id { get; set; }

        public GetNoteByIdQuery(int id)
        {
            Id = id;
        }
    }
}