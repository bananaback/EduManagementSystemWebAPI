using Microsoft.AspNetCore.Mvc;

namespace ClassManagement.API.Controllers
{
    [ApiController]
    [Route("/api/classes")]
    public class ClassController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Endpoint reached!");
        }
    }
}
