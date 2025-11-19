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
    public class MastersController : ControllerBase
    {
        private readonly GlassDbContext _context;

        public MastersController(GlassDbContext context)
        {
            _context = context;
        }

        // GET: api/Glasses
        [HttpGet("Glasses")]
        public async Task<IActionResult> GetMstGlass()
        {
            var glasses = await _context.MstGlass.ToListAsync();
            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                glasses.Cast<object>().ToList()));
        }

        [HttpGet("Districts")]
        public async Task<IActionResult> GetMstDistrict()
        {
            var districts = await _context.MstDistrict.Where(x => x.IsActive == true).ToListAsync();
            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                districts.Cast<object>().ToList()));
        }

        [HttpGet("Blocks")]
        public async Task<IActionResult> GetMstBlock(int DistrictId)
        {
            var blocks = await _context.MstBlock.Where(x => x.DistrictId_fk == DistrictId && x.IsActive == true).ToListAsync();
            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                blocks.Cast<object>().ToList()));
        }

        [HttpGet("Panchayats")]
        public async Task<IActionResult> GetMstPanchayat(int DistrictId, int BlockId)
        {
            var panchayats = await _context.MstPanchayat.Where(x => x.DistrictId_fk == DistrictId && x.BlockId_fk == BlockId && x.IsActive == true).ToListAsync();
            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                panchayats.Cast<object>().ToList()));
        }
        [HttpGet("Federations")]
        public async Task<IActionResult> GetMstFederations(int DistrictId, int BlockId)
        {
            var federations = await _context.MstFederation.Where(x => x.DistrictId_fk == DistrictId && x.BlockId_fk == BlockId && x.IsActive == true).ToListAsync();
            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                federations.Cast<object>().ToList()));
        }

		[HttpGet("CampCode")]
		public async Task<IActionResult> GetCampCode()
		{
			var camps = await _context.TblCamp.Where(x => x.IsActive == true).ToListAsync();
			return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                camps.Cast<object>().ToList()));
		}

        [HttpGet("Occupations")]
        public async Task<IActionResult> GetOccupations()
        {
            var occupations = await _context.MstOccupation.Where(x => x.IsActive == true).ToListAsync();
            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                occupations.Cast<object>().ToList()));
        }

		[HttpGet("PowerofGlasses")]
		public async Task<IActionResult> GetPowerofGlasses()
		{
			var powerGlasses = await _context.MstPowerGlasses.Where(x => x.IsActive == true).ToListAsync();
			return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                powerGlasses.Cast<object>().ToList()));
		}

        [HttpGet("ClfName")]
        public async Task<IActionResult> GetClfName()
        {
            var clfs = await _context.MstCLF.Where(x => x.IsActive == true).ToListAsync();
            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                clfs.Cast<object>().ToList()));
        }
    }
}
