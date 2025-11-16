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
        int result = 0;
        public CampsController(GlassDbContext context, ICompositeViewEngine viewEngine, SPManager spManager)
        {
            _context = context;
            _viewEngine = viewEngine;
            _spManager = spManager;
        }

        // GET: Camps
        public async Task<IActionResult> Index()
        {
            ////return View(await _context.TblCamp.ToListAsync());
            //var query = from c in _context.TblCamp
            //            join d in _context.MstDistrict
            //            on c.DistrictId equals d.DistrictId_pk
            //            join b in _context.MstBlock
            //            on new { BlockId = c.BlockId, DistrictId = c.DistrictId } equals new { BlockId = b.BlockId_pk, DistrictId = b.DistrictId_fk }
            //            select new
            //            {
            //                Camp = c,
            //                District = d,
            //                Block = b
            //            };

            return View();
        }
        public Result GetCampm1List()
        {
            try
            {
                DataTable tbllist = _spManager.SP_Campm1List();
                if (tbllist.Rows.Count > 0)
                {
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
        public Result<string> GetCampm2List()
        {
            try
            {
                DataTable tbllist = _spManager.SP_Campm1List();
                if (tbllist.Rows.Count > 0)
                {
                    string html = RenderPartialViewToString("_Campm1Data", tbllist);
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
        public IActionResult AddCamp(int? CId)
        {
            CampViewModel model = new CampViewModel();
            if (CId > 0)
            {
                var tbl = _context.TblCamp.Find(CId);
                if (tbl != null)
                {
                    model.CampId_pk = tbl.CampId_pk;
                    model.DistrictId = tbl.DistrictId;
                    model.BlockId = tbl.BlockId;
                    model.CLFId = tbl.CLFId;
                    model.PanchayatId = tbl.PanchayatId;
                    model.VOName = tbl.VOName;
                    model.CampDate = tbl.CampDate;
                    model.Location = tbl.Location;
                    model.CRPName = tbl.CRPName;
                    model.CRPMobileNo = tbl.CRPMobileNo;
                    model.ParticipantMobilized = tbl.ParticipantMobilized;
                    model.TotalScreened = tbl.TotalScreened;
                    model.TotalGlassesDistributed = tbl.TotalGlassesDistributed;
                }
            }
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Result<TblCamp>> AddCamp(CampViewModel model, IFormFile? PhotoFile)
        {
            if (ModelState.IsValid)
            {
                var currentUser = GetSubmittedBy(); // your helper method or identity user
                var tbl = model.CampId_pk > 0 ? await _context.TblCamp.FindAsync(model.CampId_pk) : new TblCamp();
                if (tbl != null)
                {
                    tbl.DistrictId = model.DistrictId;
                    tbl.BlockId = model.BlockId;
                    tbl.CLFId = model.CLFId;
                    tbl.PanchayatId = model.PanchayatId;
                    tbl.VOName = model.VOName;
                    tbl.CampDate = model.CampDate;
                    tbl.Location = model.Location;
                    tbl.CRPName = model.CRPName;
                    tbl.CRPMobileNo = model.CRPMobileNo;
                    tbl.ParticipantMobilized = model.ParticipantMobilized;
                    tbl.TotalScreened = model.TotalScreened;
                    tbl.TotalGlassesDistributed = model.TotalGlassesDistributed;
                    tbl.PowerOfGlassId = model.PowerOfGlassId;

                    tbl.PhotoUploadPath = "na";
                    if (model.CampId_pk == 0)
                    {
                        tbl.CampCode = _spManager.GenerateCode(model.DistrictId, model.BlockId);// Optional: Generate unique CampCode
                        tbl.CreatedBy = currentUser;
                        tbl.CreatedOn = DateTime.Now;

                        tbl.UpdatedBy = currentUser;
                        tbl.UpdatedOn = DateTime.Now;
                        tbl.IsActive = true;
                    }
                    if (model.PhotoUpload != null && model.PhotoUpload.Length > 0)
                    {

                        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "campm1");
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

        // GET: Camps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Camps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblCamp tblCamp)
        {

            if (ModelState.IsValid)
            {
                var currentUser = GetSubmittedBy();

                tblCamp.CreatedBy = currentUser;  // (typo in your model – should be "CreatedBy" ideally)
                tblCamp.CreatedOn = DateTime.Now;
                tblCamp.UpdatedBy = currentUser;
                tblCamp.UpdatedOn = DateTime.Now;

                tblCamp.CampCode = Utility.GenerateCampCode(tblCamp.DistrictName, tblCamp.BlockName, tblCamp.PanchayatName);


                _context.Add(tblCamp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblCamp);
        }

        // GET: Camps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCamp = await _context.TblCamp.FindAsync(id);
            if (tblCamp == null)
            {
                return NotFound();
            }
            return View(tblCamp);
        }

        // POST: Camps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CampId_pk,TypeOfModule,TypeOfVisit,CampCode,DistrictId,BlockId,CLFId,PanchayatId,VOName,CampDate,Location,CRPName,CRPMobileNo,ParticipantMobilized,TotalScreened,TotalGlassesDistributed,PowerOfGlassId,PhotoUploadPath,IsActive,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn")] TblCamp tblCamp)
        {
            if (id != tblCamp.CampId_pk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCamp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblCampExists(tblCamp.CampId_pk))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
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
