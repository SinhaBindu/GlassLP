using GlassLP.Data;
using GlassLP.Models;
using GlassLP.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;

namespace GlassLP.Controllers
{
    public class CampsController : BaseController
    {
        private readonly GlassDbContext _context;

        public CampsController(GlassDbContext context)
        {
            _context = context;
        }

        // GET: Camps
        public async Task<IActionResult> Index()
        {
            return View(await _context.TblCamp.ToListAsync());
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
        public async Task<IActionResult> AddCamp(CampViewModel model, IFormFile? PhotoFile)
        {
            if (ModelState.IsValid)
            {
                var currentUser = GetSubmittedBy(); // your helper method or identity user
                TblCamp tbl = new TblCamp();

                //tbl.TypeOfModule = model.TypeOfModule;
                //tbl.TypeOfVisit
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
                tbl.DistrictName = model.DistrictName;
                tbl.BlockName = model.BlockName;
                tbl.PanchayatName = model.PanchayatName;
                tbl.CampCode = model.DistrictId.ToString() + model.PanchayatId.ToString(); //Utility.GenerateCampCode(tbl.DistrictName, tbl.BlockName, tbl.PanchayatName);

                tbl.PhotoUploadPath = "na";
                tbl.CreatedBy = currentUser;
                tbl.CreatedOn = DateTime.Now;
                tbl.UpdatedBy = currentUser;
                tbl.UpdatedOn = DateTime.Now;
                tbl.IsActive = true;
                // Optional: Generate unique CampCode

                if (PhotoFile != null && PhotoFile.Length > 0)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "camp");
                    if (!Directory.Exists(uploadsDir))
                        Directory.CreateDirectory(uploadsDir);

                    var uniqueFileName = $"{Guid.NewGuid()}_{PhotoFile.FileName}";
                    var filePath = Path.Combine(uploadsDir, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoFile.CopyToAsync(stream);
                    }
                    model.PhotoUploadPath = "/uploads/camp/" + uniqueFileName;
                }

                _context.TblCamp.Add(tbl);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Camp added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
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
