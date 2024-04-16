using Microsoft.AspNetCore.Mvc;
using QuickLinker.API.Models;
using System;

namespace QuickLinker.API.Controllers
{
    /// <summary>
    /// Controller for accessing root API endpoints.
    /// </summary>
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        /// <summary>
        /// Get the root of the API.
        /// </summary>
        /// <returns>An IActionResult containing links to different API endpoints.</returns> 
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>
            {
                new(Url.Link("GetRoot", new { }),
                    "self",
                    "GET"),
                new(Url.Link("GetOriginalLink", new { Shortcode="short_code" }),
                    "get_original_link",
                    "GET"),
                new(Url.Link("CreateShortLink", new { }),
                    "create_short_link",
                    "POST")
            };

            return Ok(links);
        }
    }
}
