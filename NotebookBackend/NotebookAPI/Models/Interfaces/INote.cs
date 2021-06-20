using System.Collections.Generic;

namespace NotebookAPI.Models.Interfaces
{
    public interface INote
    {
        int Id { get; set; }
        string Content { get; set; }
        List<NoteCategory> NoteCategories { get; set; }
    }
}