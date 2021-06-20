using System.Collections.Generic;

namespace NotebookAPI.Contracts.Responses
{
    public class NoteResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public IEnumerable<CategoryResponse> Categories { get; set; }
    }
}