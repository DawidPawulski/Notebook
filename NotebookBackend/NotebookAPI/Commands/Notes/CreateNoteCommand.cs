using System.Collections.Generic;
using MediatR;
using NotebookAPI.Contracts.Requests;
using NotebookAPI.Contracts.Responses;

namespace NotebookAPI.Commands.Notes
{
    public class CreateNoteCommand : IRequest<NoteResponse>
    {
        public string Content { get; set; }
        public List<NoteCategoryRequest> Categories { get; set; }

        public CreateNoteCommand(){}
        
        public CreateNoteCommand(string content, List<NoteCategoryRequest> categories)
        {
            Content = content;
            Categories = categories;
        }
    }
}