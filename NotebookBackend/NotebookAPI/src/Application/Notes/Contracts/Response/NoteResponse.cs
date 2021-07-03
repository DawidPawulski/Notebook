using System.Collections.Generic;
using NotebookAPI.Application.Categories.Contracts.Response;

namespace NotebookAPI.Application.Notes.Contracts.Response
{
    public class NoteResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public IEnumerable<CategoryResponse> Categories { get; set; }
    }
}