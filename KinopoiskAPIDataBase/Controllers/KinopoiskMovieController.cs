using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using KinopoiskAPIDataBase.Attributes;
using KinopoiskAPIDataBase.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;

namespace KinopoiskAPIDataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KinopoiskMovieController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<KinopoiskMovieController> _logger;
        
        public KinopoiskMovieController(ApplicationDbContext context ,ILogger<KinopoiskMovieController> logger)
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
        public async Task<RestDTO<MovieModel[]>> Get([FromQuery] RequestDTO<MovieDTO> input)
        {
            var query = _context.Movie.AsQueryable();
            if (!string.IsNullOrEmpty(input.FilterQuery))
                query = query.Where(m => m.Name.Contains(input.FilterQuery));
            var recordCount = await query.CountAsync();
            query = query
                .OrderBy($"{input.SortColumn} {input.SortOrder}")
                .Skip(input.PageIndex * input.PageSize)
                .Take(input.PageSize);

            return new RestDTO<MovieModel[]>
            {
                Data = await query.ToArrayAsync(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null, 
                            "Kinopoisk", 
                            new {input.PageIndex, input.PageSize},
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpPost("[action]")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<MovieModel?>> Post(MovieDTO model)
        {
            var movie = await _context.Movie
                .Where(m => m.Id == model.Id)
                .FirstOrDefaultAsync();

            if (movie != null)
            {

            }

            return new RestDTO<MovieModel?>
            {
                Data = movie,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Movie",
                            model,
                            Request.Scheme)!,
                        "self",
                        "POST")
                }
            };
        }

        [HttpDelete("[action]")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<MovieModel?>> Delete(int id)
        {
            var movie = await _context.Movie
                .Where (m => m.Id == id)
                .FirstOrDefaultAsync();

            if ( movie != null )
            {
                _context.Movie.Remove(movie);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<MovieModel?>
            {
                Data = movie,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null,
                            "Movie",
                            id,
                            Request.Scheme)!,
                        "self",
                        "DELETE")
                }
            };
        }

        [HttpGet("[action]")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<IActionResult> GetMovieTest(int kpid)
        {
            var query = _context.Movie;

            var movie = _context.Movie.FirstOrDefault(m => m.KpId == kpid);

            var persons = from mp in _context.MovieActor
                          join p in _context.Person on mp.ActorId equals p.Id
                          join r in _context.Role on mp.RoleId equals r.Id
                          where mp.MovieId == movie.Id
                          select p;

            return new JsonResult(new
            {
                Name = movie.Name,
                KpId = movie.KpId,
                Description = movie.Description,
                Rating = movie.Rating,
                Type = movie.Type,
                Length = movie.Length,
                ReleaseYear = movie.ReleaseYear,
                ReleaseDate = movie.ReleaseDate,
                Person = persons,
            });
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
