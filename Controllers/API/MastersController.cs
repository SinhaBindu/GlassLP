using GlassLP.Data;
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
            return await _context.MstDistrict.Where(x => x.IsActive == true).ToListAsync();
        }

        [HttpGet("Blocks")]
        public async Task<ActionResult<IEnumerable<MstBlock>>> GetMstBlock(int DistrictId)
        {
            return await _context.MstBlock.Where(x => x.DistrictId_fk == DistrictId && x.IsActive == true).ToListAsync();
        }

        [HttpGet("Panchayats")]
        public async Task<ActionResult<IEnumerable<MstPanchayat>>> GetMstPanchayat(int DistrictId, int BlockId)
        {
            return await _context.MstPanchayat.Where(x => x.DistrictId_fk == DistrictId && x.BlockId_fk == BlockId && x.IsActive == true).ToListAsync();
        }
        [HttpGet("Federations")]
        public async Task<ActionResult<IEnumerable<MstFederation>>> GetMstFederations(int DistrictId, int BlockId)
        {
            return await _context.MstFederation.Where(x => x.DistrictId_fk == DistrictId && x.BlockId_fk == BlockId && x.IsActive == true).ToListAsync();
        }

		[HttpGet("CampCode")]
		public async Task<ActionResult<IEnumerable<TblCamp>>> GetCampCode()
		{
			return await _context.TblCamp.Where(x => x.IsActive == true).ToListAsync();
		}

        [HttpGet("Occupations")]
        public async Task<ActionResult<IEnumerable<MstOccupation>>> GetOccupations()
        {
            return await _context.MstOccupation.Where(x => x.IsActive == true).ToListAsync();
        }

		[HttpGet("PowerofGlasses")]
		public async Task<ActionResult<IEnumerable<MstPowerGlasses>>> GetPowerofGlasses()
		{
			return await _context.MstPowerGlasses.Where(x => x.IsActive == true).ToListAsync();
		}

        [HttpGet("ClfName")]
        public async Task<ActionResult<IEnumerable<MstCLF>>> GetClfName()
        {
            return await _context.MstCLF.Where(x => x.IsActive == true).ToListAsync();
        }
    }
}
