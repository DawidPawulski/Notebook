using System.Linq;
using AutoMapper;
using NotebookAPI.Application.Categories.Contracts.Response;
using NotebookAPI.Application.Notes.Contracts.Response;
using NotebookAPI.Domain.Entities;

namespace NotebookAPI.Application.Common.Mappings
{
    public class DomainToResponse : Profile
    {
        public DomainToResponse()
        {
            CreateMap<Category, CategoryResponse>();
            CreateMap<Note, NoteResponse>()
                .ForMember(dest => dest.Categories, opt =>
                    opt.MapFrom(src => src.NoteCategories.Select(x => new CategoryResponse
                        {Id = x.CategoryId, Name = x.Category.Name})));
        }
    }
}