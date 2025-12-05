using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using static Azure.Core.HttpHeader;
using static GlassLP.Data.Service;

namespace GlassLP.Controllers.API
{
	[Route("api/[controller]")]
	[ApiController]
	public class MastersController : ControllerBase
	{
		private readonly GlassDbContext _context;
		private readonly CommonData _commonData;
		private readonly SPManager _sPManager;
		private readonly GlobalDataService _globalData;
		public MastersController(GlassDbContext context, CommonData commonData, SPManager sPManager, GlobalDataService globalData)
		{
			_commonData = commonData;
			_context = context;
			_sPManager = sPManager;
			_globalData = globalData;
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
		public async Task<IActionResult> GetMstDistrict(int ModuleId = 0, string DId = "0")
		{
			var query = _context.MstDistrict
						.Where(x => x.IsActive == true);
			if (ModuleId > 0)
				query = query.Where(x => x.ModelType == ModuleId);
			if (!string.IsNullOrWhiteSpace(DId) && DId!="0")
			{
				var ids = DId.Split(',').Select(int.Parse).ToList();
				query = query.Where(x => ids.Contains(x.DistrictId_pk));
			}
			var districts = await query
				.Select(x => new
				{
					x.DistrictId_pk,
					x.DistrictName
				})
				.ToListAsync();

			return Ok(new ApiResponse<List<object>>(
				true,"OK","Data fetched successfully",
				districts.Cast<object>().ToList()
			));
		}

		[HttpGet("Blocks")]
		public async Task<IActionResult> GetMstBlock(int DistrictId, int ModuleId = 0, int BId = 0)
		{
			var query = _context.MstBlock
					  .Where(x => x.IsActive == true && x.DistrictId_fk == DistrictId);
			if (ModuleId > 0)
				query = query.Where(x => x.ModelType == ModuleId);

			else if (ModuleId > 0 && BId > 0)
				query = query.Where(x => x.BlockId_pk == BId);

			var datalist = await query
				.Select(x => new
				{
					x.BlockId_pk,
					x.BlockName
				})
				.ToListAsync();

			return Ok(new ApiResponse<List<object>>(
				true,
				"OK",
				"Data fetched successfully",
				datalist.Cast<object>().ToList()
			));
		}

		[HttpGet("Panchayats")]
		public async Task<IActionResult> GetMstPanchayat(int DistrictId, int BlockId, int PanchayatId = 0)
		{
			var query = _context.MstPanchayat
					  .Where(x => x.IsActive == true && x.DistrictId_fk == DistrictId && x.BlockId_fk == BlockId);
			if (PanchayatId > 0)
				query = query.Where(x => x.PanchayatId_pk == PanchayatId);
			var datalist = await query
				.Select(x => new
				{
					x.PanchayatId_pk,
					x.PanchayatName
				})
				.ToListAsync();

			return Ok(new ApiResponse<List<object>>(
				true,
				"OK",
				"Data fetched successfully",
				datalist.Cast<object>().ToList()
			));
		}

		[HttpGet("Federations")]
		public async Task<IActionResult> GetMstFederations(int DistrictId = 0, int BlockId = 0, int FId = 0)
		{
			var query = _context.MstFederation
					 .Where(x => x.IsActive == true);
			if (DistrictId > 0 && BlockId > 0)
				query = query.Where(x => x.DistrictId_fk == DistrictId && x.BlockId_fk == BlockId);
			if (FId > 0)
				query = query.Where(x => x.FederationId_pk == FId);
			var datalist = await query
				.Select(x => new
				{
					x.FederationId_pk,
					x.FederationName
				})
				.ToListAsync();

			return Ok(new ApiResponse<List<object>>(
				true,
				"OK",
				"Data fetched successfully",
				datalist.Cast<object>().ToList()
			));
		}

		[HttpGet("CampCode")]
		public async Task<IActionResult> GetCampCode(string code = "", int TypeMId = 0)
		{
			try
			{
				var query = _context.TblCamp.Where(x => x.IsActive == true).AsQueryable();
				// Build dynamic conditions
				if (TypeMId > 0)
					query = query.Where(x => x.TypeOfModule == TypeMId);

				if (!string.IsNullOrEmpty(code))
					query = query.Where(x => x.CampCode == code);

				var camps = await query
					.Select(x => new
					{
						campId_pk = x.CampId_pk,
						campCode = x.CampCode
					}).ToListAsync();

				if (camps.Count > 0)
				{
					return Ok(new ApiResponse<List<object>>(
					true,
					"OK",
					"Data fetched successfully",
					camps.Cast<object>().ToList()));
				}
				return Ok(new ApiResponse<List<object>>(
					   false,
					   "error",
					   "Record not found!",
					   new List<object>()
				   ));
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse<string>(
					false, "error", ex.Message, null
				));
			}
		}

		[HttpGet("CampDetails")]
		public async Task<IActionResult> GetCampDetails(int campId)
		{
			try
			{
				var camp = await _context.TblCamp
					.Where(c => c.CampId_pk == campId && c.IsActive == true)
					.Select(c => new
					{
						campId_pk = c.CampId_pk,
						districtId = c.DistrictId ?? 0,
						blockId = c.BlockId ?? 0,
						clfId = c.CLFId ?? 0,
						panchayatId = c.PanchayatId ?? 0
					})
					.FirstOrDefaultAsync();

				if (camp == null)
				{
					return Ok(new ApiResponse<List<object>>(
						false,
						"not_found",
						"Camp not found.",
						new List<object>()));
				}

				return Ok(new ApiResponse<List<object>>(
					true,
					"OK",
					"Data fetched successfully",
					new List<object> { camp }));
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse<List<object>>(
					false,
					"error",
					ex.Message,
					new List<object>()));
			}
		}

		[HttpGet("Occupations")]
		public async Task<IActionResult> GetOccupations()
		{
			var occupations = await _context.MstOccupation
				.Where(x => x.IsActive == true).ToListAsync();
			return Ok(new ApiResponse<List<object>>(
				true,
				"OK",
				"Data fetched successfully",
				occupations.Cast<object>().ToList()));
		}

		[HttpGet("PowerofGlasses")]
		public async Task<IActionResult> GetPowerofGlasses()
		{
			var powerGlasses = await _context.MstPowerGlasses
				.Where(x => x.IsActive == true)
				.Select(p => new
				{
					pk_PowerGlassId = p.pk_PowerGlassId,
					powerofGlass = p.PowerofGlass ?? string.Empty
				})
				.ToListAsync();

			return Ok(new ApiResponse<List<object>>(
				true,
				"OK",
				"Data fetched successfully",
				powerGlasses.Cast<object>().ToList()));
		}

		//[HttpGet("ClfName")]
		//public async Task<IActionResult> GetClfName(int DistrictId = 0, int BlockId = 0)//
		//{
		//    var clfs = await _context.MstFederation.Where(x => x.IsActive == true && ((x.DistrictId_fk == 0 && BlockId == 0) || (x.DistrictId_fk == DistrictId && x.BlockId_fk == BlockId))).ToListAsync();
		//    return Ok(new ApiResponse<List<object>>(
		//        true,
		//        "OK",
		//        "Data fetched successfully",
		//        clfs.Cast<object>().ToList()));
		//}

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

		[HttpGet("VEData")]
		public async Task<IActionResult> GetVEData(int isSelect = 0)
		{
			//var data = _commonData.GetVE(isSelect);
			//return Ok(new ApiResponse<List<object>>(
			//  true,
			//  "OK",
			//  "Data fetched successfully",
			//  data.Cast<object>().ToList()));
			var data = await _context.MstVendor.Where(x => x.IsActive == true)
			.Select(x => new
			{
				pk_VendorsId = x.pk_VendorsId,
				VEName = x.VEName,
			}).ToListAsync();
			return Ok(new ApiResponse<List<object>>(
				true,
				"OK",
				"Data fetched successfully",
				data.Cast<object>().ToList()));
		}
	}
}
