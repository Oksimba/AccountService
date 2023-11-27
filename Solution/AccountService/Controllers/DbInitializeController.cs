using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbInitializeController: ControllerBase
    {
        IDbInitializeService dbInitializeService;
        public DbInitializeController(IDbInitializeService dbInitializeService)
        {
            this.dbInitializeService = dbInitializeService;
        }

        [HttpGet(Name = "Initialize")]
        public IActionResult Initialize()
        {
            dbInitializeService.Initialize();
            return Ok();
        }
    }
}
