using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Commands.Notes;
using NotebookAPI.Contracts.Responses;
using NotebookAPI.Data;
using NotebookAPI.Models;

namespace NotebookAPI.Handlers.Notes
{
    public class UpdateNoteHandler : IRequestHandler<UpdateNoteCommand, NoteResponse>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UpdateNoteHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<NoteResponse> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var note = await _context.Notes.FindAsync(request.Id);
            if (note == null)
            {
                return null;
            }

            var noteCategoriesToDelete = _context.NoteCategories
                .Where(x => x.NoteId == request.Id);
            foreach (var noteCategory in noteCategoriesToDelete)
            {
                _context.NoteCategories.Remove(noteCategory);
                _context.Entry(noteCategory).State = EntityState.Deleted;
            }

            if (request.Categories != null)
            {
                foreach (var category in request.Categories)
                {
                    var categoryToAdd = await _context.Categories.FindAsync(category.Id);

                    if (categoryToAdd == null)
                    {
                        throw new Exception("Category not found");
                    }

                    var noteCategory = new NoteCategory
                    {
                        NoteId = note.Id,
                        Note = note,
                        CategoryId = categoryToAdd.Id,
                        Category = categoryToAdd
                    };
                    _context.NoteCategories.Add(noteCategory);
                    _context.Entry(categoryToAdd).State = EntityState.Detached;
                    _context.Entry(noteCategory).State = EntityState.Added;
                }
            }

            note.Content = request.Content;
            _context.Entry(note).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            NoteResponse response = _mapper.Map<NoteResponse>(note);

            return await Task.FromResult(response);
        }
    }
}