using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NotebookAPI.Commands.Categories;
using NotebookAPI.Contracts.Responses;
using NotebookAPI.Data;

namespace NotebookAPI.Handlers.Categories
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, CategoryResponse>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DeleteCategoryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(request.Id);

            if (category == null)
            {
                return null;
            }
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CategoryResponse>(category);

            return await Task.FromResult(response);
        }
    }
}