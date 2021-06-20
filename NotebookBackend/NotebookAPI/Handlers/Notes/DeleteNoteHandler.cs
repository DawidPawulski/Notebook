using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NotebookAPI.Commands.Notes;
using NotebookAPI.Contracts.Responses;
using NotebookAPI.Data;

namespace NotebookAPI.Handlers.Notes
{
    public class DeleteNoteHandler : IRequestHandler<DeleteNoteCommand, NoteResponse>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeleteNoteHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<NoteResponse> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var note = await _context.Notes.FindAsync(request.Id);
            if (note == null)
            {
                return null;
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<NoteResponse>(note);

            return await Task.FromResult(response);
        }
    }
}