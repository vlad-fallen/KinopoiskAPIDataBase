using KinopoiskApiVersion.DTO.v1;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace KinopoiskApiVersion.Controllers.v1
{
    [Route("v{version:ApiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class KinopoiskController : ControllerBase
    {
        private readonly ILogger<KinopoiskController> _logger;

        public KinopoiskController(ILogger<KinopoiskController> logger)
        {
            _logger = logger;
        }

        /*[HttpGet]
        public async Task<IActionResult> GetMovieById(int id)
        {

            return Ok();
        }*/

        [Route("getmovietest")]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public DTO.v1.RestDTO<MovieModel[]> GetMovieTest()
        {
            return new DTO.v1.RestDTO<MovieModel[]>
            {
                Data = new MovieModel[] {
                    new MovieModel()
                    {
                        Id = 1,
                        Name = "Avatar",
                        ReleaseYear = 2009
                    }
                },
                Links = new List<DTO.v1.LinkDTO>
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
