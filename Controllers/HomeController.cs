using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace GlassLP.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GlassDbContext _context;
        private readonly SPManager _spManager;

        public HomeController(ILogger<HomeController> logger, GlassDbContext context, SPManager spManager)
        {
            _logger = logger;
            _context = context;
            _spManager = spManager;
        }

        public IActionResult Index()
        {
            GetMaindashboardTopCount();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public IActionResult Dashboard()
        //{
        //    var result = _dashboardService.GetMaindashboardTopCount();
        //    return View();
        //}

        public Result GetMaindashboardTopCount()
        {
            try
            {
                DataTable tbllist = _spManager.SP_MaindashboardTopCount();
                if (tbllist.Rows.Count > 0)
                {
                    var row = tbllist.Rows[0];

                    // Counts ko ViewBag me assign karna
                    ViewBag.CampCount = Convert.ToInt32(row["CampCount"]);
                    ViewBag.ParticipantCount = Convert.ToInt32(row["ParticipantCount"]);
                    ViewBag.VECount = Convert.ToInt32(row["VECount"]);
                    ViewBag.Availableglassinstock = Convert.ToInt32(row["Availableglassinstock"]);

                    return Result.Success("Dashboard count loaded");
                }
                else
                {
                    return Result.Failure("Record Not Found!!");
                }
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, ex);
            }
        }
    }
}
