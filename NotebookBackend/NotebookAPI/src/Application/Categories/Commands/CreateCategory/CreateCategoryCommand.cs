using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NotebookAPI.Application.Categories.Contracts.Response;
using NotebookAPI.Domain.Entities;
using NotebookAPI.Infrastructure.Persistence;

namespace NotebookAPI.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryResponse>
    {
        public string Name { get; set; }

        public CreateCategoryCommand(){}
        
        public CreateCategoryCommand(string name)
        {
            Name = name;
        }
    }
    
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var newCategory = new Category {Name = request.Name};
            await _context.Categories.AddAsync(newCategory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = _mapper.Map<CategoryResponse>(newCategory);
            
            return response;
        }
    }
}