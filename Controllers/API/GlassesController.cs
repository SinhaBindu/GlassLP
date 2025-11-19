using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GlassLP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlassesController : ControllerBase
    {
        private readonly GlassDbContext _context;

        public GlassesController(GlassDbContext context)
        {
            _context = context;
        }

        // GET: api/Glasses
        [HttpGet]
        public async Task<IActionResult> GetMstGlass()
        {
            var glasses = await _context.MstGlass.ToListAsync();
            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                glasses.Cast<object>().ToList()));
        }

        // GET: api/Glasses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMstGlass(int id)
        {
            var mstGlass = await _context.MstGlass.FindAsync(id);

            if (mstGlass == null)
            {
                return NotFound(new ApiResponse<List<object>>(
                    false,
                    "not_found",
                    "Glass not found.",
                    new List<object>()));
            }

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                new List<object> { mstGlass }));
        }

        // PUT: api/Glasses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMstGlass(int id, MstGlass mstGlass)
        {
            if (id != mstGlass.pk_Glassid)
            {
                return BadRequest(new ApiResponse<List<object>>(
                    false,
                    "bad_request",
                    "ID mismatch.",
                    new List<object>()));
            }

            _context.Entry(mstGlass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MstGlassExists(id))
                {
                    return NotFound(new ApiResponse<List<object>>(
                        false,
                        "not_found",
                        "Glass not found.",
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
                "Glass updated successfully.",
                new List<object> { mstGlass }));
        }

        // POST: api/Glasses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostMstGlass(MstGlass mstGlass)
        {
            _context.MstGlass.Add(mstGlass);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Glass created successfully.",
                new List<object> { mstGlass }));
        }

        // DELETE: api/Glasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMstGlass(int id)
        {
            var mstGlass = await _context.MstGlass.FindAsync(id);
            if (mstGlass == null)
            {
                return NotFound(new ApiResponse<List<object>>(
                    false,
                    "not_found",
                    "Glass not found.",
                    new List<object>()));
            }

            _context.MstGlass.Remove(mstGlass);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Glass deleted successfully.",
                new List<object>()));
        }

        private bool MstGlassExists(int id)
        {
            return _context.MstGlass.Any(e => e.pk_Glassid == id);
        }
    }
}
