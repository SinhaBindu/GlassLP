using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlassLP.Data;
using GlassLP.Models;
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
        public async Task<IActionResult> GetTblCamp()
        {
            var camps = await _context.TblCamp.ToListAsync();
            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                camps.Cast<object>().ToList()));
        }

        // GET: api/Camps/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblCamp(int id)
        {
            var tblCamp = await _context.TblCamp.FindAsync(id);

            if (tblCamp == null)
            {
                return NotFound(new ApiResponse<List<object>>(
                    false,
                    "not_found",
                    "Camp not found.",
                    new List<object>()));
            }

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                new List<object> { tblCamp }));
        }

        // PUT: api/Camps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblCamp(int id, TblCamp tblCamp)
        {
            if (id != tblCamp.CampId_pk)
            {
                return BadRequest(new ApiResponse<List<object>>(
                    false,
                    "bad_request",
                    "ID mismatch.",
                    new List<object>()));
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
                    return NotFound(new ApiResponse<List<object>>(
                        false,
                        "not_found",
                        "Camp not found.",
                        new List<object>()));
                }
                else
                {
                    throw;
                }
            }

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Camp updated successfully.",
                new List<object> { tblCamp }));
        }

        // POST: api/Camps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostTblCamp(TblCamp tblCamp)
        {
            var currentUser = GetSubmittedBy();

            tblCamp.CreatedBy = currentUser;  // (typo in your model – should be "CreatedBy" ideally)
            tblCamp.CreatedOn = DateTime.Now;
            tblCamp.UpdatedBy = currentUser;
            tblCamp.UpdatedOn = DateTime.Now;

            tblCamp.CampCode = Utility.GenerateCampCode(tblCamp.DistrictName,tblCamp.BlockName,tblCamp.PanchayatName);

            _context.TblCamp.Add(tblCamp);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Camp created successfully.",
                new List<object> { tblCamp }));
        }

        // DELETE: api/Camps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblCamp(int id)
        {
            var tblCamp = await _context.TblCamp.FindAsync(id);
            if (tblCamp == null)
            {
                return NotFound(new ApiResponse<List<object>>(
                    false,
                    "not_found",
                    "Camp not found.",
                    new List<object>()));
            }

            _context.TblCamp.Remove(tblCamp);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Camp deleted successfully.",
                new List<object>()));
        }

        private bool TblCampExists(int id)
        {
            return _context.TblCamp.Any(e => e.CampId_pk == id);
        }
    }
}
