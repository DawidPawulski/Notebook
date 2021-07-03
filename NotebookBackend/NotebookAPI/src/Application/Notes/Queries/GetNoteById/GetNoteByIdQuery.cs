using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using NotebookAPI.Application.Notes.Contracts.Response;
using NotebookAPI.Domain.Entities;
using Npgsql;

namespace NotebookAPI.Application.Notes.Queries.GetNoteById
{
    public class GetNoteByIdQuery : IRequest<NoteResponse>
    {
        public int Id { get; set; }

        public GetNoteByIdQuery(int id)
        {
            Id = id;
        }
    }
    
    public class GetNoteByIdHandler : IRequestHandler<GetNoteByIdQuery, NoteResponse>
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public GetNoteByIdHandler(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<NoteResponse> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            Note result;

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var sql = "select * from \"Notes\" n left join \"NoteCategories\" nc on nc.\"NoteId\" = n.\"Id\"" +
                          $"left join \"Categories\" c on c.\"Id\" = nc.\"CategoryId\" WHERE n.\"Id\" = {request.Id}";

                var categories = connection.Query<Category>("SELECT * FROM \"Categories\"").AsList();
                
                var notes = await connection
                    .QueryAsync<Note, Domain.Entities.NoteCategory, Note>(sql, (note, noteCategory) =>
                {
                    if (noteCategory != null)
                    {
                        noteCategory.Note = note;
                        noteCategory.Category = categories.FirstOrDefault(x => x.Id == noteCategory.CategoryId);
                        note.NoteCategories.Add(noteCategory);
                    }
                    
                    return note;
                }, splitOn: "categoryId");


                result = notes.GroupBy(n => n.Id).Select(g =>
                {
                    var groupedNote = g.First();
                    if (groupedNote.NoteCategories.Count > 0)
                    {
                        groupedNote.NoteCategories = g.Select(n => n.NoteCategories.Single()).ToList();
                    }
                    
                    return groupedNote;
                }).FirstOrDefault();
            }

            var response = _mapper.Map<NoteResponse>(result);
            return response;
        }
    }
}