using System.Collections.Generic;
using NotebookAPI.Domain.Entities;

namespace NotebookAPI.Domain.Common.Interfaces
{
    public interface ICategory
    {
        int Id { get; set; }
        string Name { get; set; }
        List<NoteCategory> NoteCategories { get; set; }
    }
}