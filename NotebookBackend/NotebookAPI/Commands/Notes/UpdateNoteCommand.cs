using System.Collections.Generic;
using MediatR;
using NotebookAPI.Contracts.Requests;
using NotebookAPI.Contracts.Responses;

namespace NotebookAPI.Commands.Notes
{
    public class UpdateNoteCommand : IRequest<NoteResponse>
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<NoteCategoryRequest> Categories { get; set; }

        public UpdateNoteCommand(){}

        public UpdateNoteCommand(string content, List<NoteCategoryRequest> categories = null)
        {
            Content = content;
            Categories = categories;
        }
    }
}