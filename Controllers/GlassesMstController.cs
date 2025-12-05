using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace GlassLP.Controllers
{
    public class GlassesMstController : BaseController
    {
        private readonly GlassDbContext _context;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly CommonData _commonData;
        private readonly SPManager _spManager;
        int result = 0;

        public GlassesMstController(GlassDbContext context, ICompositeViewEngine viewEngine, CommonData commonData, SPManager spManager)
        {
            _context = context;
            _viewEngine = viewEngine;
            _commonData = commonData;
            _spManager = spManager;
        }
        // GET: GlassesMst
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public Result<string> GetGlassList()
        {
            try
            {
                DataTable tbllist = _spManager.SP_ClassMasterList();
                if (tbllist.Rows.Count > 0)
                {
                    string html = RenderPartialViewToString("_GlassData", tbllist);
                    return Result<string>.Success(html, "Record Found!!");
                }
                else
                {
                    return Result<string>.Failure("Record Not Found!!");
                }
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(ex.Message, ex);
            }
        }
        private string RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

                if (viewResult.View == null)
                    throw new ArgumentNullException($"{viewName} not found");

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                viewResult.View.RenderAsync(viewContext).Wait();
                return writer.ToString();
            }
        }
        public IActionResult Create(int? GId)
        {
            GlassViewModel model = new GlassViewModel();
            model.TypeOfModuleList = _commonData.GetTypeOfModule();
            model.PowerOfGlassList = _commonData.GetPowerOfGlass();

            if (GId > 0)
            {
                var tbl = _context.MstGlass.Find(GId);
                if (tbl != null)
                {
                    model.pk_Glassid = tbl.pk_Glassid;
                    model.TypeOfModuleId = tbl.TypeOfModuleId;
                    model.PowerOfGlassId = tbl.PowerOfGlassId;
                    model.NoofGlasses = tbl.NoofGlasses;
                    model.PerGlassCost = tbl.PerGlassCost;
                    model.TotalGlassCost = tbl.TotalGlassCost;
                    model.AdvertisingCost = tbl.AdvertisingCost;
                    model.Storagemiscellaneouscost = tbl.Storagemiscellaneouscost;
                    model.Availableclassesinstock = tbl.Availableclassesinstock;
                   // model.TotalDistributedGlass = tbl.TotalDistributedGlass;
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Result<MstGlass>> Create(GlassViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = GetSubmittedBy();
                    var tbl = model.pk_Glassid > 0 ? await _context.MstGlass.FindAsync(model.pk_Glassid) : new MstGlass();
                    if (tbl != null)
                    {
                        tbl.TypeOfModuleId = model.TypeOfModuleId;
                        tbl.PowerOfGlassId = model.PowerOfGlassId;
                        tbl.NoofGlasses = model.NoofGlasses;
                        tbl.PerGlassCost = model.PerGlassCost;
                        tbl.TotalGlassCost = model.TotalGlassCost;
                        tbl.AdvertisingCost = model.AdvertisingCost;
                        tbl.Storagemiscellaneouscost = model.Storagemiscellaneouscost;

                        if (model.pk_Glassid == 0)
                        {
                            tbl.CreatedBy = currentUser;
                            tbl.CreatedOn = DateTime.Now;
                            tbl.IsActive = true;
                            tbl.Availableclassesinstock = model.NoofGlasses;
                            _context.MstGlass.Add(tbl);
                        }
                        else
                        {
                            tbl.UpdatedBy = currentUser;
                            tbl.UpdatedOn = DateTime.Now;
                            _context.Entry(tbl).State = EntityState.Modified;
                        }

                        result = await _context.SaveChangesAsync();
                    }
                    if (result > 0)
                    {
                        string message = model.pk_Glassid == 0 ? "Glass added successfully!" : "Glass updated successfully!";
                        return Result<MstGlass>.Success(tbl, message);
                    }
                    else
                    {
                        string errorMessage = model.pk_Glassid == 0 ? "Error occurred while adding glass." : "Error occurred while updating glass.";
                        return Result<MstGlass>.Failure(errorMessage);
                    }
                }
                else
                {
                    return Result<MstGlass>.ValidationFailure(ModelState);
                }
            }
            catch (Exception ex)
            {
                return Result<MstGlass>.Failure("Failed to save glass", ex);
            }
        }
        // GET: GlassesMst/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mstGlass = await _context.MstGlass
                .FirstOrDefaultAsync(m => m.pk_Glassid == id);
            if (mstGlass == null)
            {
                return NotFound();
            }

            // Populate ViewBag with display names
            if (mstGlass.TypeOfModuleId > 0)
            {
                ViewBag.TypeOfModuleName =
                mstGlass.TypeOfModuleId == 1 ? "Model One" :
                mstGlass.TypeOfModuleId == 2 ? "Model Two" : "";
            }

            var powerOfGlass = await _context.MstPowerGlasses
            .Where(p => p.pk_PowerGlassId == mstGlass.PowerOfGlassId)
            .Select(p => new { Value = p.PowerofGlass })
            .FirstOrDefaultAsync();

            ViewBag.PowerOfGlassName = powerOfGlass?.Value ?? "";

            return View(mstGlass);
        }
        private bool MstGlassExists(int id)
        {
            return _context.MstGlass.Any(e => e.pk_Glassid == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableGlassStock(int typeOfModuleId, int powerOfGlassId)
        {
            try
            {
                var availableStock = await _context.MstGlass
                    .Where(g => g.TypeOfModuleId == typeOfModuleId
                        && g.PowerOfGlassId == powerOfGlassId
                        && g.IsActive == true)
                    .SumAsync(g => g.Availableclassesinstock ?? 0);

                int soldCount1 = 0;
                int soldCount2 = 0;

                if (typeOfModuleId == 1)
                {
                    soldCount1 = await _context.TblPaticipantM1
                    .Where(p => p.PowerofGlassId == powerOfGlassId
                    && p.IsApproved == 1)
                    .CountAsync();
                }
                else if (typeOfModuleId == 2)
                {
                    soldCount2 = await _context.TblPaticipantM2
                   .Where(p => p.PowerofGlassId == powerOfGlassId
                   && p.IsApproved == 1)
                   .CountAsync();
                }

                var finalStock = availableStock - (soldCount1 + soldCount2);
                if (finalStock < 0) finalStock = 0;

                return Json(new
                {
                    status = true,
                    data = new
                    {
                        availableStock = finalStock
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

    }
}
