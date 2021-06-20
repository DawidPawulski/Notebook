using System.Linq;
using AutoMapper;
using NotebookAPI.Contracts.Responses;
using NotebookAPI.Models;

namespace NotebookAPI.Mapping
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