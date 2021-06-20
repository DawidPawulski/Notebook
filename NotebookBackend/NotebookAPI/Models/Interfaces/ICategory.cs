using System.Collections.Generic;

namespace NotebookAPI.Models.Interfaces
{
    public interface ICategory
    {
        int Id { get; set; }
        string Name { get; set; }
        List<NoteCategory> NoteCategories { get; set; }
    }
}