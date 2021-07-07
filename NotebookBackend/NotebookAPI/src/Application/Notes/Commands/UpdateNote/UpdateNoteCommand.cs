using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Application.NoteCategory.Contracts.Request;
using NotebookAPI.Application.Notes.Contracts.Response;
using NotebookAPI.Infrastructure.Persistence;

namespace NotebookAPI.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommand : IRequest<NoteResponse>
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<NoteCategoryRequest> Categories { get; set; }

        public UpdateNoteCommand(){}

        public UpdateNoteCommand(string content, List<NoteCategoryRequest> categories = null)
        {
            Content = content;
            Categories = categories;
        }
    }
    
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

                    var noteCategory = new Domain.Entities.NoteCategory
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

            return response;
        }
    }
}