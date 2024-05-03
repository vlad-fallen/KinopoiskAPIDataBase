using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using KinopoiskAPIDataBase.Attributes;
using KinopoiskAPIDataBase.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace KinopoiskAPIDataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KinopoiskPersonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<KinopoiskMovieController> _logger;

        public KinopoiskPersonController(ApplicationDbContext context, ILogger<KinopoiskMovieController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        [ManualValidationFilter]
        public async Task<ActionResult<RestDTO<PersonModel[]>>> Get([FromQuery] RequestDTO<PersonDTO> input)
        {
            if (!ModelState.IsValid)
            {
                var details = new ValidationProblemDetails(ModelState);
                details.Extensions["traceId"] = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier;

                if (ModelState.Keys.Any(k => k == "PageSize"))
                {
                    details.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.2";

                    details.Status = StatusCodes.Status501NotImplemented;

                    return new ObjectResult(details)
                    {
                        StatusCode = StatusCodes.Status501NotImplemented,
                    };
                }
                else
                {
                    details.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    details.Status = StatusCodes.Status400BadRequest;
                    return new BadRequestObjectResult(details);

                }
            }

            var q = from p in _context.Person
                    select new PersonModel
                    {
                        Id = p.Id,
                        KpId = p.KpId,
                        Name = p.Name,
                        EnName = p.EnName,
                        Birthday = p.Birthday,
                    };

            if (!string.IsNullOrEmpty(input.FilterQuery) )
                q = q.Where(p => p.Name.Contains(input.FilterQuery));

            var recordCount = await q.CountAsync();

            q = q
                .OrderBy($"{input.SortColumn} {input.SortOrder}")
                .Skip(input.PageIndex * input.PageSize)
                .Take(input.PageSize);

            return new RestDTO<PersonModel[]>
            {
                Data = await q.ToArrayAsync(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null,
                            "KinopoiskPerson",
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
        public async Task<RestDTO<PersonModel?>> Get(int kpid)
        {
            var personModel = await (from p in _context.Person
                                     where p.KpId == kpid
                                     select p).FirstOrDefaultAsync();

            if (personModel != null)
            {
                var movies = await (from mp in  _context.MoviePerson
                                    join m in _context.Movie on mp.MovieId equals m.Id
                                    where mp.ActorId == personModel.Id
                                    select new MovieModel
                                    {
                                        Id = m.Id,
                                        KpId = m.KpId,
                                        Name = m.Name,
                                        Role = mp.Character,
                                    }).ToListAsync();

                personModel.Movies = movies;
            }

            return new RestDTO<PersonModel?>
            {
                Data = personModel,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null,
                            "KinopoiskPerson",
                            kpid,
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpPut("[action]")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<PersonModel?>> Put(PersonDTO model)
        {
            var person = await _context.Person
                .Where(p => p.KpId == model.KpId)
                .FirstOrDefaultAsync();

            if (person != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    person.Name = model.Name;
                if (!string.IsNullOrEmpty(model.EnName))
                    person.EnName = model.EnName;
                if (model.Birthday.HasValue)
                    person.Birthday = model.Birthday.Value;

                person.LastModifiedDate = DateTime.Now;

                _context.Person.Update(person);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<PersonModel?>
            {
                Data = person,
                Links = new List<LinkDTO>
                    {
                    new LinkDTO(
                        Url.Action(null,
                            "KinopoiskPerson",
                            model,
                            Request.Scheme)!,
                        "self",
                        "PUT"),
                    }
            };
        }

        [HttpDelete("[action]")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<PersonModel?>> Delete(int id)
        {
            var person = await _context.Person
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (person != null )
            {
                _context.Person.Remove(person);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<PersonModel?>
            {
                Data = person,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(null,
                            "KinopoiskPerson",
                            id,
                            Request.Scheme)!,
                        "self",
                        "DELETE")
                }
            };
        }
    }
}
