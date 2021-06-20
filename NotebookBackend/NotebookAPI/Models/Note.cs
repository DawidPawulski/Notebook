using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotebookAPI.Models.Interfaces;

namespace NotebookAPI.Models
{
    public class Note : INote
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public List<NoteCategory> NoteCategories { get; set; }

        public Note()
        {
            NoteCategories = new List<NoteCategory>();
        }
    }
}