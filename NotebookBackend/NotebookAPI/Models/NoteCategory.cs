using NotebookAPI.Models.Interfaces;

namespace NotebookAPI.Models
{
    public class NoteCategory : INoteCategory
    {
        public int NoteId { get; set; }
        public Note Note { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}