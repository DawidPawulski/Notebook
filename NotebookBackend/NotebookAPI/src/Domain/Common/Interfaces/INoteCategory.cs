using NotebookAPI.Domain.Entities;

namespace NotebookAPI.Domain.Common.Interfaces
{
    public interface INoteCategory
    {
        int NoteId { get; set; }
        public Note Note { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}