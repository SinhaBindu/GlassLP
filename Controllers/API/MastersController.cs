using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly CommonData _commonData;
        public MastersController(GlassDbContext context, CommonData commonData)
        {
            _commonData = commonData;
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
        public async Task<IActionResult> GetMstDistrict(int ModuleId = 0)
        {
            var districts = ModuleId > 0 ? await _context.MstDistrict.Where(x => x.IsActive == true && (x.ModelType == ModuleId || x.ModelType == null)).ToListAsync() : await _context.MstDistrict.Where(x => x.IsActive == true).ToListAsync();
            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                districts.Cast<object>().ToList()));
        }

        [HttpGet("Blocks")]
        public async Task<IActionResult> GetMstBlock(int DistrictId, int ModuleId=0)
        {
            var blocks = ModuleId > 0 ? await _context.MstBlock.Where(x => x.DistrictId_fk == DistrictId && x.IsActive == true && (x.ModelType == ModuleId || x.ModelType == null)).ToListAsync(): await _context.MstBlock.Where(x => x.DistrictId_fk == DistrictId && x.IsActive == true).ToListAsync();
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
        [HttpGet("DataYesNo")]
        public async Task<IActionResult> GetYesNo(int isSelect = 0)
        {
            var data = _commonData.GetYesNo(isSelect);
            return await Task.FromResult(
                Ok(new ApiResponse<List<SelectListItem>>(true, "OK", "Data fetched successfully", data))
            );
        }
        [HttpGet("DataTypeofVisionIssue")]
        public async Task<IActionResult> GetTypeofVisionIssue(int isSelect = 0)
        {
            var data = _commonData.GetTypeofVisionIssue(isSelect);
            return await Task.FromResult(
                Ok(new ApiResponse<List<SelectListItem>>(true, "OK", "Data fetched successfully", data))
            );
        }
        [HttpGet("DataFeedbackonComfort")]
        public async Task<IActionResult> GetFeedbackonComfort(int isSelect = 0)
        {
            var data = _commonData.GetFeedbackonComfort(isSelect);
            return await Task.FromResult(
                Ok(new ApiResponse<List<SelectListItem>>(true, "OK", "Data fetched successfully", data))
            );
        }
        [HttpGet("DataTypeOfPaticipant")]
        public async Task<IActionResult> GetTypeOfPaticipant(int isSelect = 0)
        {
            var data = _commonData.GetTypeOfPaticipant(isSelect);
            return await Task.FromResult(
                Ok(new ApiResponse<List<SelectListItem>>(true, "OK", "Data fetched successfully", data))
            );
        }
        [HttpGet("DataTypeOfModule")]
        public async Task<IActionResult> GetTypeOfModule(int isSelect = 0)
        {
            var data = _commonData.GetTypeOfModule(isSelect);
            return await Task.FromResult(
                Ok(new ApiResponse<List<SelectListItem>>(true, "OK", "Data fetched successfully", data))
            );
        }
    }
}
