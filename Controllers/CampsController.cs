using GlassLP.Data;
using GlassLP.Models;
using GlassLP.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GlassLP.Controllers
{
    public class CampsController : BaseController
    {
        private readonly GlassDbContext _context;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly SPManager _spManager;
        private readonly CommonData _commonData;
        int result = 0;
        public CampsController(GlassDbContext context, ICompositeViewEngine viewEngine, SPManager spManager, CommonData commonData)
        {
            _context = context;
            _viewEngine = viewEngine;
            _spManager = spManager;
            _commonData = commonData;
        }

        // GET: Camps
        public async Task<IActionResult> Index()
        {
            Filtermodel filtermodel = new Filtermodel();
			return View(filtermodel);
        }
        public Result GetCampList(Filtermodel filtermodel)
        {
            try
            {
                DataTable tbllist = _spManager.SP_CampList(filtermodel);
                if (tbllist.Rows.Count > 0)
                {
                    // Pass base URL to view via ViewData
                    var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
                    ViewData["BaseUrl"] = baseUrl;
                    string html = RenderPartialViewToString("_Campm1Data", tbllist);
                    return Result.Success(html, "Record Found!!");
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
        //public Result<string> GetCampm2List()
        //{
        //    try
        //    {
        //        DataTable tbllist = _spManager.SP_Campm1List();
        //        if (tbllist.Rows.Count > 0)
        //        {
        //            string html = RenderPartialViewToString("_Campm1Data", tbllist);
        //            return Result<string>.Success(html, "Record Found!!");
        //        }
        //        else
        //        {
        //            return Result<string>.Failure("Record Not Found!!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<string>.Failure(ex.Message, ex);
        //    }
        //}
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
        public IActionResult AddCamp(int? CId)
        {
            CampViewModel model = new CampViewModel();
            model.TypeOfModuleList = _commonData.GetTypeOfModule();
          //  model.VEList = _commonData.GetVE();
            if (CId > 0)
            {
                var tbl = _context.TblCamp.Find(CId);
                if (tbl != null)
                {
                    model.TypeOfModule = tbl.TypeOfModule;
                    model.CampId_pk = tbl.CampId_pk;
                    model.DistrictId = tbl.DistrictId;
                    model.BlockId = tbl.BlockId;
                    model.CLFId = tbl.CLFId;
                    model.PanchayatId = tbl.PanchayatId;
                    model.VOName = tbl.VOName;
                    model.CampDate = tbl.CampDate;
                    model.Location = tbl.Location;
                    model.CRPName = tbl.CRPName;
                    model.VEId = tbl.VEId;
                    model.CRPMobileNo = tbl.CRPMobileNo;
                    model.ParticipantMobilized = tbl.ParticipantMobilized;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Result<TblCamp>> AddCamp(CampViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = GetSubmittedBy(); // your helper method or identity user
                    var tbl = model.CampId_pk > 0 ? await _context.TblCamp.FindAsync(model.CampId_pk) : new TblCamp();
                    if (tbl != null)
                    {
                        tbl.TypeOfModule = model.TypeOfModule;
                        tbl.DistrictId = model.DistrictId;
                        tbl.BlockId = model.BlockId;
                        tbl.CLFId = model.CLFId;
                        tbl.PanchayatId = model.PanchayatId;
                        tbl.VOName = model.VOName;
                        tbl.CampDate = model.CampDate;
                        tbl.Location = model.Location;
                        if (model.TypeOfModule == 1)
                        {
                            tbl.CRPName = model.CRPName;
                            tbl.CRPMobileNo = model.CRPMobileNo;
                            tbl.VEId = null;
                        }
                        else if (model.TypeOfModule == 2)
                        {
                            tbl.VEId = model.VEId;
                            tbl.CRPName = null;
                            tbl.CRPMobileNo = null;
                        }
                        tbl.ParticipantMobilized = model.ParticipantMobilized;
                        //tbl.TotalScreened = model.TotalScreened;
                        //tbl.TotalGlassesDistributed = model.TotalGlassesDistributed;
                        //tbl.PowerOfGlassId = model.PowerOfGlassId;
                        if (model.CampId_pk == 0)
                        {

                            tbl.CampCode = _spManager.GenerateCode(model.DistrictId, model.BlockId);// Optional: Generate unique CampCode
                            tbl.CreatedBy = currentUser;
                            tbl.CreatedOn = DateTime.Now;
                            tbl.IsActive = true;
                            tbl.PhotoUploadPath = "na";
                        }
                        else
                        {
                            tbl.UpdatedBy = currentUser;
                            tbl.UpdatedOn = DateTime.Now;
                        }
                        if (model.PhotoUpload != null && model.PhotoUpload.Length > 0)
                        {
                            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "campm" + model.TypeOfModule);
                            if (!Directory.Exists(uploadsDir))
                                Directory.CreateDirectory(uploadsDir);
                            var uniqueFileName = $"{tbl.CampCode}_{model.PhotoUpload.FileName}";
                            var filePath = Path.Combine(uploadsDir, uniqueFileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await model.PhotoUpload.CopyToAsync(stream);
                            }
                            tbl.PhotoUploadPath = "\\uploads" + "\\campm1" + "\\" + uniqueFileName;
                        }
                        //tbl.PhotoUploadPath = "a";
                        _context.TblCamp.Add(tbl);
                        result = await _context.SaveChangesAsync();
                    }
                    if (result > 0)
                    {
                        return Result<TblCamp>.Success(tbl, $"Camp added successfully! Camp Code is <b>{tbl.CampCode}</b>");
                    }
                    else
                    {
                        return Result<TblCamp>.Failure("Error occurred while adding camp.");
                    }
                }
                else
                {
                    return Result<TblCamp>.ValidationFailure(ModelState);
                }
            }
            catch (Exception ex)
            {
                return Result<TblCamp>.Failure("Failed to create camp", ex);
            }
        }
        // GET: Camps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCamp = await _context.TblCamp
                .FirstOrDefaultAsync(m => m.CampId_pk == id);
            if (tblCamp == null)
            {
                return NotFound();
            }

            return View(tblCamp);
        }
        // GET: Camps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCamp = await _context.TblCamp
                .FirstOrDefaultAsync(m => m.CampId_pk == id);
            if (tblCamp == null)
            {
                return NotFound();
            }

            return View(tblCamp);
        }

        // POST: Camps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblCamp = await _context.TblCamp.FindAsync(id);
            if (tblCamp != null)
            {
                _context.TblCamp.Remove(tblCamp);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblCampExists(int id)
        {
            return _context.TblCamp.Any(e => e.CampId_pk == id);
        }
    }
}
