using Microsoft.EntityFrameworkCore;
using NotebookAPI.Domain.Entities;

namespace NotebookAPI.Infrastructure.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<Note> Notes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<NoteCategory> NoteCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NoteCategory>().HasKey(nc => new { nc.NoteId, nc.CategoryId });

            modelBuilder.Entity<NoteCategory>()
                .HasOne<Note>(nc => nc.Note)
                .WithMany(n => n.NoteCategories)
                .HasForeignKey(nc => nc.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NoteCategory>()
                .HasOne<Category>(nc => nc.Category)
                .WithMany(c => c.NoteCategories)
                .HasForeignKey(nc => nc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}