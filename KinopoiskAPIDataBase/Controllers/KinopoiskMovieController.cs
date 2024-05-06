using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using KinopoiskAPIDataBase.Attributes;
using KinopoiskAPIDataBase.Constants.Constants;
using KinopoiskAPIDataBase.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;

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

        /*---------------------------*/

        [HttpGet("[action]")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<RestDTO<MovieModel[]>> Get([FromQuery] RequestDTO<MovieDTO> input)
        {
            //_logger.LogInformation(, "Get method started.");
            _logger.LogInformation(CustomLogEvents.KinopoiskMovieController_Get, "Get method started.");

            var q = from m in _context.Movie
                    select new MovieModel
                    {
                        KpId = m.KpId,
                        Name = m.Name,
                        OriginalName = m.OriginalName,
                        Rating = m.Rating,
                        Description = m.Description,
                        Type = m.Type,
                        Length = m.Length,
                        ReleaseYear = m.ReleaseYear,
                        ReleaseDate = m.ReleaseDate,
                        Genres = (from mg in _context.MovieGenre
                                  join g in _context.Genre on mg.GenreId equals g.Id
                                  where mg.MovieId == m.Id
                                  select new GenreModel
                                  {
                                      Value = g.Value,
                                  }).ToList(),
                    };

            if (!string.IsNullOrEmpty(input.FilterQuery))
                q = q.Where(m => m.Name.Contains(input.FilterQuery));
            var recordCount = await q.CountAsync();
            q = q
                .OrderBy($"{input.SortColumn} {input.SortOrder}")
                .Skip(input.PageIndex * input.PageSize)
                .Take(input.PageSize);

            return new RestDTO<MovieModel[]>
            {
                Data = await q.ToArrayAsync(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null, 
                            "KinopoiskMovie", 
                            new {input.PageIndex, input.PageSize},
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        /*---------------------------*/

        [HttpGet("[action]/{kpid:int}")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<RestDTO<MovieModel?>> Get(int kpid)
        {
            var movieModel = await (from m in _context.Movie 
                              where m.KpId == kpid
                              select m).FirstOrDefaultAsync();

            if (movieModel != null)
            {
                var persons = await (from mp in _context.MoviePerson
                                   join p in _context.Person on mp.ActorId equals p.Id
                                   where mp.MovieId == movieModel.Id
                                   select new PersonModel
                                   {
                                       Id = p.Id,
                                       KpId = p.KpId,
                                       Name = p.Name,
                                       EnName = p.EnName,
                                       Description = mp.Character,
                                   }).ToListAsync();

                var genres = await (from mg in _context.MovieGenre
                                    join g in _context.Genre on mg.GenreId equals g.Id
                                    where mg.MovieId == movieModel.Id
                                    select new GenreModel
                                    {
                                        Value = g.Value,
                                    }).ToListAsync();

                movieModel.Persons = persons;
                movieModel.Genres = genres;
            }

            return new RestDTO<MovieModel?>
            {
                Data = movieModel,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null,
                            "KinopoiskMovie",
                            kpid,
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        /*---------------------------*/

        [HttpPut("[action]")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<MovieModel?>> Put(MovieDTO model)
        {
            var movie = await _context.Movie
                .Where(m => m.KpId == model.KpId)
                .FirstOrDefaultAsync();

            if (movie != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    movie.Name = model.Name;
                if (!string.IsNullOrEmpty(model.OriginalName))
                    movie.OriginalName = model.OriginalName;
                if (!string.IsNullOrEmpty(model.Description))
                    movie.Description = model.Description;
                if (model.Rating.HasValue)
                    movie.Rating = model.Rating.Value;
                if (model.Year.HasValue)
                    movie.ReleaseYear = model.Year.Value;
                if (!string.IsNullOrEmpty(model.Type))
                    movie.Type = model.Type;
                if (model.Length.HasValue)
                    movie.Length = model.Length.Value;
                if (model.ReleaseDate.HasValue)
                    movie.ReleaseDate = model.ReleaseDate.Value;

                movie.LastModifiedDate = DateTime.Now;
                _context.Movie.Update(movie);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<MovieModel?>
            {
                Data = movie,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "KinopoiskMovie",
                            model,
                            Request.Scheme)!,
                        "self",
                        "PUT")
                }
            };
        }

        /*---------------------------*/

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
                            "KinopoiskMovie",
                            id,
                            Request.Scheme)!,
                        "self",
                        "DELETE")
                }
            };
        }

        /*---------------------------*/

        [HttpGet("[action]")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<IActionResult> GetMovieTest(int kpid)
        {
            var query = _context.Movie;

            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.KpId == kpid);

            var persons = from mp in _context.MoviePerson
                          join p in _context.Person on mp.ActorId equals p.Id
                          join r in _context.Role on mp.RoleId equals r.Id
                          where mp.MovieId == movie.Id
                          select new PersonDTO { 
                          KpId = p.KpId,
                          Name = p.Name,
                          EnName = p.EnName,
                          };

            return Ok(new MovieDTO
            {
                KpId = movie.KpId,
                Name = movie.Name,
                OriginalName = movie.OriginalName,
                Rating = movie.Rating,
                Description = movie.Description,
                Year = movie.ReleaseYear,
                Type = movie.Type,
                Length = movie.Length,
                ReleaseDate = movie.ReleaseDate,
            });

            /*return new RestDTO<MovieDTO>
            {
                Data = new MovieDTO 
                { 
                    
                },
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null, "Kinopoisk", null, Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };*/
        }

        /*---------------------------*/

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

            //await _context.Movie.AddAsync(movie);
            //await _context.SaveChangesAsync();

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
