using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly ISeedRepository _seedRepository;

        public SeedController(ISeedRepository seedRepository)
        {
            _seedRepository = seedRepository;
        }

        [HttpPost]
        [Route("{count}")]
        public IActionResult GenerateQuestions(int count)
        {
            _seedRepository.GenerateQuestions(count);

            return Ok();
        }
    }
}