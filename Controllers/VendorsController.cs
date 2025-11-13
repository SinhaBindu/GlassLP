using GlassLP.Data;
using Microsoft.AspNetCore.Mvc;

namespace GlassLP.Controllers
{
    public class VendorsController : BaseController
    {
        private readonly GlassDbContext _context;

        public VendorsController(GlassDbContext context)
        {
            _context = context;
        }

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
