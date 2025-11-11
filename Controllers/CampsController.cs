using GlassLP.Data;
using GlassLP.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
