using Microsoft.AspNetCore.Mvc;

namespace MoneyTransfer.Api.Controllers
{
    [Route("/")]
    public class HomeController
        : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
            => Ok("Hello World, from controller!");
    }
}
