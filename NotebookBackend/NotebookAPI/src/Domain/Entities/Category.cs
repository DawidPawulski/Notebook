using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotebookAPI.Domain.Common.Interfaces;

namespace NotebookAPI.Domain.Entities
{
    public class Category : ICategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<NoteCategory> NoteCategories { get; set; }

        public Category()
        {
            NoteCategories = new List<NoteCategory>();
        }
    }
}