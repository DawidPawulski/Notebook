using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Application.NoteCategory.Contracts.Request;
using NotebookAPI.Application.Notes.Contracts.Response;
using NotebookAPI.Domain.Entities;
using NotebookAPI.Infrastructure.Persistence;

namespace NotebookAPI.Application.Notes.Commands.CreateNote
{
    public class CreateNoteCommand : IRequest<NoteResponse>
    {
        public string Content { get; set; }
        public List<NoteCategoryRequest> Categories { get; set; }

        public CreateNoteCommand(){}
        
        public CreateNoteCommand(string content, List<NoteCategoryRequest> categories)
        {
            Content = content;
            Categories = categories;
        }
    }

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

                    var noteCategory = new Domain.Entities.NoteCategory
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

            return response;
        }
    }
}