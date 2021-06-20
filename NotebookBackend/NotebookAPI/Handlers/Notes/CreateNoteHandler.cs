using System;
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
    public class CreateNoteHandler : IRequestHandler<CreateNoteCommand, NoteResponse>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CreateNoteHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<NoteResponse> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            var newNote = new Note {Content = request.Content};
            await _context.Notes.AddAsync(newNote, cancellationToken);
            _context.Entry(newNote).State = EntityState.Added;
            await _context.SaveChangesAsync(cancellationToken);
            
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
                        NoteId = newNote.Id,
                        Note = newNote,
                        CategoryId = categoryToAdd.Id,
                        Category = categoryToAdd
                    };
                    _context.NoteCategories.Add(noteCategory);
                    _context.Entry(categoryToAdd).State = EntityState.Detached;
                    _context.Entry(noteCategory).State = EntityState.Added;
                }
            }
            
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<NoteResponse>(newNote);
            
            return await Task.FromResult(response);
        }
    }
}