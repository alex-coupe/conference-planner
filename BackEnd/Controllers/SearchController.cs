using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackEnd.Data;
using ConferenceDTO;
using Microsoft.EntityFrameworkCore;
using BackEnd.Infrastructure;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<SearchResult>>> Search(SearchTerm term)
        {
            var query = term.Query;

            var sessionResults = await _context.Sessions.Include(s => s.Track)
                                .Include(ss => ss.SessionSpeakers)
                                    .ThenInclude(s => s.Speaker)
                                    .Where(s => s.Title.Contains(query) ||
                                    s.Track.Name.Contains(query))
                                    .ToListAsync();

            var speakerResults = await _context.Speakers.Include(ss => ss.SessionSpeakers)
                                .ThenInclude(s => s.Session)
                                .Where(s => s.Name.Contains(query) ||
                                s.Bio.Contains(query) || s.Website.Contains(query))
                                .ToListAsync();

            var results = sessionResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Session,
                Session = s.MapSessionResponse()
            })
            .Concat(speakerResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Speaker,
                Speaker = s.MapSpeakerResponse()
            }));

            return results.ToList();                
        }
    }
}