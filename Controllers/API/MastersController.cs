using GlassLP.Data;
using GlassLP.Data.GlassApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassLP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MastersController : ControllerBase
    {
        private readonly GlassDbContext _context;

        public MastersController(GlassDbContext context)
        {
            _context = context;
        }

        // GET: api/Glasses
        [HttpGet("Glasses")]
        public async Task<ActionResult<IEnumerable<MstGlass>>> GetMstGlass()
        {
            return await _context.MstGlass.ToListAsync();
        }

        [HttpGet("Districts")]
        public async Task<ActionResult<IEnumerable<MstDistrict>>> GetMstDistrict()
        {
            return await _context.MstDistrict.ToListAsync();
        }

        [HttpGet("Blocks")]
        public async Task<ActionResult<IEnumerable<MstBlock>>> GetMstBlock()
        {
            return await _context.MstBlock.ToListAsync();
        }

        [HttpGet("Panchayats")]
        public async Task<ActionResult<IEnumerable<MstPanchayat>>> GetMstPanchayat()
        {
            return await _context.MstPanchayat.ToListAsync();
        }

    }
}
