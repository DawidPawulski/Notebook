using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using NotebookAPI.Contracts.Responses;
using NotebookAPI.Data;
using NotebookAPI.Models;
using NotebookAPI.Queries.Notes;
using Npgsql;

namespace NotebookAPI.Handlers.Notes
{
    public class GetAllNotesHandler : IRequestHandler<GetAllNotesQuery, List<NoteResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public GetAllNotesHandler(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }
        
        public async Task<List<NoteResponse>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
        {
            List<Note> result;

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var sql = "select * from \"Notes\" n left join \"NoteCategories\" nc on nc.\"NoteId\" = n.\"Id\"" +
                            "left join \"Categories\" c on c.\"Id\" = nc.\"CategoryId\"";

                var categories = connection.Query<Category>("SELECT * FROM \"Categories\"").AsList();
                
                var notes = await connection
                    .QueryAsync<Note, NoteCategory, Note>(sql, (note, noteCategory) =>
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
                }).ToList();
            }

            var response = _mapper.Map<List<NoteResponse>>(result);
            
            return response;
        }
    }
} 