using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlassLP.Data;
using GlassLP.Utilities;

namespace GlassLP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : BaseController
    {
        private readonly GlassDbContext _context;

        public CampsController(GlassDbContext context)
        {
            _context = context;
        }

        // GET: api/Camps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblCamp>>> GetTblCamp()
        {
            return await _context.TblCamp.ToListAsync();
        }

        // GET: api/Camps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblCamp>> GetTblCamp(int id)
        {
            var tblCamp = await _context.TblCamp.FindAsync(id);

            if (tblCamp == null)
            {
                return NotFound();
            }

            return tblCamp;
        }

        // PUT: api/Camps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblCamp(int id, TblCamp tblCamp)
        {
            if (id != tblCamp.CampId_pk)
            {
                return BadRequest();
            }

            _context.Entry(tblCamp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblCampExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Camps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblCamp>> PostTblCamp(TblCamp tblCamp)
        {
            var currentUser = GetSubmittedBy();

            tblCamp.CreatedBy = currentUser;  // (typo in your model – should be "CreatedBy" ideally)
            tblCamp.CreatedOn = DateTime.Now;
            tblCamp.UpdatedBy = currentUser;
            tblCamp.UpdatedOn = DateTime.Now;

            tblCamp.CampCode = Utility.GenerateCampCode(tblCamp.DistrictName,tblCamp.BlockName,tblCamp.PanchayatName);

            _context.TblCamp.Add(tblCamp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblCamp", new { id = tblCamp.CampId_pk }, tblCamp);
        }

        // DELETE: api/Camps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblCamp(int id)
        {
            var tblCamp = await _context.TblCamp.FindAsync(id);
            if (tblCamp == null)
            {
                return NotFound();
            }

            _context.TblCamp.Remove(tblCamp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblCampExists(int id)
        {
            return _context.TblCamp.Any(e => e.CampId_pk == id);
        }
    }
}
