using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotebookAPI.Application.Categories.Contracts.Response;
using NotebookAPI.Infrastructure.Persistence;

namespace NotebookAPI.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<CategoryResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UpdateCategoryCommand(){}
        
        public UpdateCategoryCommand(string name)
        {
            Name = name;
        }
    }
    
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

            return response;
        }
    }
}