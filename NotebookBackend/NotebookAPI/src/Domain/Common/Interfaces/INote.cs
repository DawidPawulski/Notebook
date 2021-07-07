using System.Collections.Generic;
using NotebookAPI.Domain.Entities;

namespace NotebookAPI.Domain.Common.Interfaces
{
    public interface INote
    {
        int Id { get; set; }
        string Content { get; set; }
        List<NoteCategory> NoteCategories { get; set; }
    }
}