using System.Collections.Generic;
using MediatR;
using NotebookAPI.Contracts.Responses;

namespace NotebookAPI.Queries.Notes
{
    public class GetAllNotesQuery : IRequest<List<NoteResponse>>
    {
    }
}