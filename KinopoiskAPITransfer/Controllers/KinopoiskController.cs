using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KinopoiskAPITransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KinopoiskController : ControllerBase
    {
        [HttpGet]
        public IActionResult get()
        {
            return Ok(1);
        }
    }
}
