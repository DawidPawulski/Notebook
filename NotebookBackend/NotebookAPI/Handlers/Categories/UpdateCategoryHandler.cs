using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Commands.Categories;
using NotebookAPI.Contracts.Responses;
using NotebookAPI.Data;

namespace NotebookAPI.Handlers.Categories
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(request.Id);
            
            if (category == null)
            {
                return null;
            }
            
            category.Name = request.Name;
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<CategoryResponse>(category);

            return await Task.FromResult(response);
        }
    }
}