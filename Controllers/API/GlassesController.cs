using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlassLP.Data.GlassApp.Models;

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
        public async Task<ActionResult<IEnumerable<MstGlass>>> GetMstGlass()
        {
            return await _context.MstGlass.ToListAsync();
        }

        // GET: api/Glasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MstGlass>> GetMstGlass(int id)
        {
            var mstGlass = await _context.MstGlass.FindAsync(id);

            if (mstGlass == null)
            {
                return NotFound();
            }

            return mstGlass;
        }

        // PUT: api/Glasses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMstGlass(int id, MstGlass mstGlass)
        {
            if (id != mstGlass.pk_Glassid)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Glasses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MstGlass>> PostMstGlass(MstGlass mstGlass)
        {
            _context.MstGlass.Add(mstGlass);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMstGlass", new { id = mstGlass.pk_Glassid }, mstGlass);
        }

        // DELETE: api/Glasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMstGlass(int id)
        {
            var mstGlass = await _context.MstGlass.FindAsync(id);
            if (mstGlass == null)
            {
                return NotFound();
            }

            _context.MstGlass.Remove(mstGlass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MstGlassExists(int id)
        {
            return _context.MstGlass.Any(e => e.pk_Glassid == id);
        }
    }
}
