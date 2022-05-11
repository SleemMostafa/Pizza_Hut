using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Pizza_Hut.Hubs;

namespace Pizza_Hut.Controllers
{
    public class HubsController : Controller
    {
        private readonly IHubContext<ProductHub> hubContext;

        public HubsController(IHubContext<ProductHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
