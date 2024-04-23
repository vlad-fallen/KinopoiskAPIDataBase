using Asp.Versioning;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KinopoiskApiVersion.Controllers.v2
{
    [Route("v{version:ApiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CodeOnDemandController : ControllerBase
    {
        [HttpGet("[action]", Name = "Test")]
        [EnableCors("AnyOrigin")]
        [ResponseCache(NoStore = true)]
        public ContentResult Test()
        {
            return Content("<script>" +
                    "window.alert('Your client supports JavaScript!" +
                    "\\r\\n\\r\\n" +
                    $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" +
                    "\\r\\n" +
                    "Client time (UTC): ' + new Date().toISOString());" +
                    "</script>" +
                    "<noscript>Your client does not support JavaScript</noscript>",
                    "text/html");
        }

        [HttpGet("[action]",Name = "Test2")]
        [EnableCors("AnyOrigin")]
        [ResponseCache(NoStore = true)]
        public ContentResult Test2(int? addMinutes = null)
        {
            var dateTime = DateTime.UtcNow;
            if (addMinutes.HasValue)
                dateTime = dateTime.AddMinutes(addMinutes.Value);

            return Content("<script>" +
                    "window.alert('Your client supports JavaScript!" +
                    "\\r\\n\\r\\n" +
                    $"Server time (UTC): {dateTime.ToString("o")}" +
                    "\\r\\n" +
                    "Client time (UTC): ' + new Date().toISOString());" +
                    "</script>" +
                    "<noscript>Your client does not support JavaScript</noscript>",
                    "text/html");
        }
    }
}
