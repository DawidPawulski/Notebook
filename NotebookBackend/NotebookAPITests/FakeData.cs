using NotebookAPI.Domain.Entities;
using NotebookAPI.Infrastructure.Persistence;

namespace NotebookAPITests
{
    public class FakeData
    {
        public void SeedData(DataContext context)
        {
            var firstNote = new Note
            {
                Content = "first note"
            };
            var secondNote = new Note
            {
                Content = "second note"
            };
            var thirdNote = new Note
            {
                Content = "third note"
            };
            var homeCategory = new Category
            {
                Name = "Home"
            };
            var workCategory = new Category
            {
                Name = "Work"
            };
            var relaxCategory = new Category
            {
                Name = "Relax"
            };
            var schoolCategory = new Category
            {
                Name = "School"
            };
            
            context.Notes.Add(firstNote);
            context.Notes.Add(secondNote);
            context.Notes.Add(thirdNote);
            context.Categories.Add(homeCategory);
            context.Categories.Add(workCategory);
            context.Categories.Add(relaxCategory);
            context.Categories.Add(schoolCategory);
            context.NoteCategories.Add(new NoteCategory {Note = firstNote, Category = homeCategory});
            context.NoteCategories.Add(new NoteCategory {Note = firstNote, Category = workCategory});
            context.NoteCategories.Add(new NoteCategory {Note = firstNote, Category = relaxCategory});
            context.NoteCategories.Add(new NoteCategory {Note = thirdNote, Category = workCategory});
            context.SaveChanges();
        }
    }
}