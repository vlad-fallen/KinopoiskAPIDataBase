using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using KinopoiskAPIDataBase.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KinopoiskAPIDataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KinopoiskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<KinopoiskController> _logger;
        
        public KinopoiskController(ApplicationDbContext context ,ILogger<KinopoiskController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /*[HttpGet]
        public async Task<IActionResult> GetMovieById(int id)
        {

            return Ok();
        }*/

        [HttpGet("[action]")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<RestDTO<MovieModel[]>> Get(int pageIndex = 0, int pageSize = 0)
        {
            var query = _context.Movie
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            return new RestDTO<MovieModel[]>
            {
                Data = await query.ToArrayAsync(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = await _context.Movie.CountAsync(),
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null, 
                            "Kinopoisk", 
                            new {pageIndex, pageSize},
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpGet("[action]")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<RestDTO<MovieModel[]>> GetMovieTest()
        {
            var query = _context.Movie;

            return new RestDTO<MovieModel[]>
            {
                Data = await query.ToArrayAsync(),
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null, "Kinopoisk", null, Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpPut("[action]")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<MovieModel[]>> PutMovieTest()
        {
            var movie = new MovieModel()
            {
                KpId = 1,
                Name = "Тест",
                OriginalName = "Test",
                Rating = 1,
                Description = "Description",
                ReleaseYear = 2000,
                ReleaseDate = DateTime.Now,
                Type = "movie",
                CreatedDate = DateTime.Now,
            };

            await _context.Movie.AddAsync(movie);
            await _context.SaveChangesAsync();

            return new RestDTO<MovieModel[]>
            {
                Data = new MovieModel[] {movie},
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null, "Kinopoisk", null, Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

    }
}
