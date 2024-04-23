using KinopoiskApiVersion.DTO.v2;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace KinopoiskApiVersion.Controllers.v2
{
    [Route("v{version:ApiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
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

        //[Route("getmovietest")]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 120)]
        public RestDTO<MovieModel[]> GetMovieTest()
        {
            return new RestDTO<MovieModel[]>
            {
                Items = new MovieModel[] {
                    new MovieModel()
                    {
                        Id = 1,
                        Name = "Avatar",
                        ReleaseYear = 2009
                    }
                },
                Links = new List<DTO.v1.LinkDTO>
                {
                    new DTO.v1.LinkDTO(
                        Url.Action(null, "Kinopoisk", null, Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }
    }
}
