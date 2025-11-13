using Microsoft.AspNetCore.Mvc;

namespace GlassLP.Controllers
{
    public class ParticipantM2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
		public IActionResult Create()
		{
			return View();
		}
	}
}
