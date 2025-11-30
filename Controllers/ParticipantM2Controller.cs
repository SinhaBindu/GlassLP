using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GlassLP.Controllers
{
    public class ParticipantM2Controller : BaseController
    {
		private readonly GlassDbContext _context;
		private readonly ICompositeViewEngine _viewEngine;
		private readonly SPManager _spManager;
		int result = 0;
		public ParticipantM2Controller(GlassDbContext context, ICompositeViewEngine viewEngine, SPManager spManager)
		{
			_context = context;
			_viewEngine = viewEngine;
			_spManager = spManager;
		}

		public async Task<IActionResult> Index()
		{
			Filtermodel filtermodel = new Filtermodel();
			return View(filtermodel);
		}

		public Result<string> GetParticipantM2List(Filtermodel filtermodel)
		{
			try
			{
				DataTable tbllist = _spManager.SP_ParticipantM2List(filtermodel);
				if (tbllist.Rows.Count > 0)
				{
					string html = RenderPartialViewToString("_ParticipantM2Data", tbllist);
					return Result<string>.Success(html, "Record Found!!");
				}
				else
				{
					return Result<string>.Failure("Record Not Found!!");
				}
			}
			catch (Exception ex)
			{
				return Result<string>.Failure(ex.Message, ex);
			}
		}

		[HttpPost]
		public Result<string> ActivateSelected([FromBody] List<int> participantIds)
		{
			try
			{
				if (participantIds == null || !participantIds.Any())
				{
					return Result<string>.Failure("No participants selected.");
				}

				var ids = participantIds.Where(id => id > 0).Distinct().ToList();
				if (!ids.Any())
				{
					return Result<string>.Failure("No valid participants selected.");
				}

				var currentUser = GetSubmittedBy() ?? "System";
				var now = DateTime.Now;

				// Build a comma-separated list of IDs for the SQL IN clause
				var idList = string.Join(",", ids);

				var sql = $@"
							UPDATE tbl_PaticipantM2 
							SET IsApproved = 1,
							ApprovedBy = @p0,
							ApprovedOn = @p1
							WHERE ParticipantId_pk IN ({idList})";

				_context.Database.ExecuteSqlRaw(sql, currentUser, now);

				return Result<string>.Success(string.Empty, "Selected participants activated successfully.");
			}
			catch (Exception ex)
			{
				return Result<string>.Failure("Failed to activate participants: " + ex.Message);
			}
		}
		private string RenderPartialViewToString(string viewName, object model)
		{
			ViewData.Model = model;

			using (var writer = new StringWriter())
			{
				var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

				if (viewResult.View == null)
					throw new ArgumentNullException($"{viewName} not found");

				var viewContext = new ViewContext(
					ControllerContext,
					viewResult.View,
					ViewData,
					TempData,
					writer,
					new HtmlHelperOptions()
				);

				viewResult.View.RenderAsync(viewContext).Wait();
				return writer.ToString();
			}
		}
        public IActionResult Create(int? CId)
        {
            ParticipantM2ViewModel model = new ParticipantM2ViewModel();

            try
            {
                if (CId > 0)
                {
                    var tbl = _context.TblPaticipantM2.Find(CId);

                    if (tbl != null)
                    {
                        model.ParticipantId_pk = tbl.ParticipantId_pk;
                        model.TypeofParticipantId = tbl.TypeofParticipantId;
                        model.CampId_fk = tbl.CampId_fk;
                        model.DistrictId = tbl.DistrictId;
                        model.BlockId = tbl.BlockId;
                        model.CLFId = tbl.CLFId;
                        model.PanchayatId = tbl.PanchayatId;
                        model.ParticipantName = tbl.ParticipantName;
                        model.MobileNo = tbl.MobileNo;
                        model.Age = tbl.Age;
                        model.ScreeningDate = tbl.ScreeningDate;
                        model.SHGName = tbl.SHGName;
                        model.OccupationId = tbl.OccupationId;
                        model.Occupation_Others = tbl.Occupation_Others;
                        model.VisionIssueIdentifiedId = tbl.VisionIssueIdentifiedId;
                        model.TypeofVisionIssueId = tbl.TypeofVisionIssueId;
                        model.GlassesSold = tbl.GlassesSold;
                        model.PowerofGlassId = tbl.PowerofGlassId;
                        model.FeedbackonComfort = tbl.FeedbackonComfort;
                        model.FollowupRequiredId = tbl.FollowupRequiredId;
                        model.DigitalConsentId = tbl.DigitalConsentId;
                        model.Location = tbl.Location;
                        model.ScreeningCost = tbl.ScreeningCost;
                        model.GlassesCost = tbl.GlassesCost;
                        model.RemarksActionTaken = tbl.RemarksActionTaken;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

            }

            return View(model);
        }


        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<Result<TblPaticipantM2>> Create(ParticipantM2ViewModel model)
		{
		try
		{
			if (ModelState.IsValid)
			{
				// Check for duplicate participant (only for new records)
				if (model.ParticipantId_pk == 0)
				{
					var participantName = model.ParticipantName?.Trim();
					var mobileNo = model.MobileNo?.Trim();

					var duplicateExists = await _context.TblPaticipantM2
						.AnyAsync(p => p.IsActive == true &&
							p.ParticipantName != null && p.ParticipantName.Trim() == participantName &&
							p.MobileNo != null && p.MobileNo.Trim() == mobileNo);

					if (duplicateExists)
					{
						return Result<TblPaticipantM2>.Failure("A participant with the same name and mobile number already exists.");
					}
				}
				else
				{
					// For updates, check if another participant (excluding current one) has the same name and mobile
					var participantName = model.ParticipantName?.Trim();
					var mobileNo = model.MobileNo?.Trim();

					var duplicateExists = await _context.TblPaticipantM2
						.AnyAsync(p => p.IsActive == true &&
							p.ParticipantId_pk != model.ParticipantId_pk &&
							p.ParticipantName != null && p.ParticipantName.Trim() == participantName &&
							p.MobileNo != null && p.MobileNo.Trim() == mobileNo);

					if (duplicateExists)
					{
						return Result<TblPaticipantM2>.Failure("A participant with the same name and mobile number already exists.");
					}
				}

				var currentUser = GetSubmittedBy(); // your helper method or identity user
				var tbl = model.ParticipantId_pk > 0 ? await _context.TblPaticipantM2.FindAsync(model.ParticipantId_pk) : new TblPaticipantM2();
				if (tbl != null)
				{
						tbl.TypeofParticipantId = model.TypeofParticipantId;
						tbl.CampId_fk = model.CampId_fk;
					tbl.DistrictId = model.DistrictId;
					tbl.BlockId = model.BlockId;
					tbl.CLFId = model.CLFId;
					tbl.PanchayatId = model.PanchayatId;
					tbl.ParticipantName = model.ParticipantName;
						tbl.MobileNo = model.MobileNo;
						tbl.Age = model.Age;
						tbl.ScreeningDate = model.ScreeningDate;
						tbl.SHGName = model.SHGName;
						tbl.OccupationId = model.OccupationId;
						tbl.Occupation_Others = model.Occupation_Others;
						tbl.VisionIssueIdentifiedId = model.VisionIssueIdentifiedId;
						tbl.TypeofVisionIssueId = model.TypeofVisionIssueId;
						tbl.GlassesSold = model.GlassesSold;
						tbl.PowerofGlassId = model.PowerofGlassId;
						tbl.FeedbackonComfort = model.FeedbackonComfort;
						tbl.FollowupRequiredId = model.FollowupRequiredId;
						tbl.DigitalConsentId = model.DigitalConsentId;
						tbl.Location = model.Location;
					    tbl.ScreeningCost = model.ScreeningCost;
					    tbl.GlassesCost = model.GlassesCost;
					    tbl.RemarksActionTaken = model.RemarksActionTaken;

					if (model.ParticipantId_pk == 0)
					{
						tbl.CreatedBy = currentUser;
						tbl.CreatedOn = DateTime.Now;
						tbl.IsActive = true;
						_context.TblPaticipantM2.Add(tbl);
					}
					else
					{
						tbl.UpdatedBy = currentUser;
						tbl.UpdatedOn = DateTime.Now;
					}
					result = await _context.SaveChangesAsync();
					}
					if (result > 0)
					{
						var message = model.ParticipantId_pk == 0 
							? "Participant Model 2 added successfully!" 
							: "Participant Model 2 updated successfully!";
						return Result<TblPaticipantM2>.Success(tbl, message);
					}
					else
					{
						var errorMessage = model.ParticipantId_pk == 0 
							? "Error occurred while adding Participant Model 2." 
							: "Error occurred while updating Participant Model 2.";
						return Result<TblPaticipantM2>.Failure(errorMessage);
					}
				}
				else
				{
					return Result<TblPaticipantM2>.ValidationFailure(ModelState);
				}
			}
			catch (Exception ex)
			{
				return Result<TblPaticipantM2>.Failure(ex.Message, ex);
			}

		}

		// GET: ParticipantM2/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var participant = await _context.TblPaticipantM2
				.FirstOrDefaultAsync(m => m.ParticipantId_pk == id);

			if (participant == null)
			{
				return NotFound();
			}

			// Create view model with related data
			var model = new ParticipantM2ViewModel
			{
				ParticipantId_pk = participant.ParticipantId_pk,
				TypeofParticipantId = participant.TypeofParticipantId,
				CampId_fk = participant.CampId_fk,
				DistrictId = participant.DistrictId,
				BlockId = participant.BlockId,
				CLFId = participant.CLFId,
				PanchayatId = participant.PanchayatId,
				ParticipantName = participant.ParticipantName,
				MobileNo = participant.MobileNo,
				Age = participant.Age,
				ScreeningDate = participant.ScreeningDate,
				SHGName = participant.SHGName,
				OccupationId = participant.OccupationId,
				Occupation_Others = participant.Occupation_Others,
				VisionIssueIdentifiedId = participant.VisionIssueIdentifiedId,
				TypeofVisionIssueId = participant.TypeofVisionIssueId,
				GlassesSold = participant.GlassesSold,
				PowerofGlassId = participant.PowerofGlassId,
				FeedbackonComfort = participant.FeedbackonComfort,
				FollowupRequiredId = participant.FollowupRequiredId,
				DigitalConsentId = participant.DigitalConsentId,
				Location = participant.Location,
				ScreeningCost = participant.ScreeningCost,
				GlassesCost = participant.GlassesCost,
				RemarksActionTaken = participant.RemarksActionTaken,
				IsActive = participant.IsActive
			};

			// Get related names for display
			ViewBag.CampCode = await _context.TblCamp
				.Where(c => c.CampId_pk == participant.CampId_fk)
				.Select(c => c.CampCode)
				.FirstOrDefaultAsync() ?? "N/A";

			ViewBag.DistrictName = await _context.MstDistrict
				.Where(d => d.DistrictId_pk == participant.DistrictId)
				.Select(d => d.DistrictName)
				.FirstOrDefaultAsync() ?? "N/A";

			ViewBag.BlockName = await _context.MstBlock
				.Where(b => b.BlockId_pk == participant.BlockId)
				.Select(b => b.BlockName)
				.FirstOrDefaultAsync() ?? "N/A";

			ViewBag.CLFName = await _context.MstCLF
				.Where(c => c.pk_CLFId == participant.CLFId)
				.Select(c => c.CLFName)
				.FirstOrDefaultAsync() ?? "N/A";

			ViewBag.PanchayatName = await _context.MstPanchayat
				.Where(p => p.PanchayatId_pk == participant.PanchayatId)
				.Select(p => p.PanchayatName)
				.FirstOrDefaultAsync() ?? "N/A";

			ViewBag.OccupationName = await _context.MstOccupation
				.Where(o => o.pk_OccupationId == participant.OccupationId)
				.Select(o => o.OccupatioName)
				.FirstOrDefaultAsync() ?? "N/A";

			ViewBag.PowerOfGlassName = await _context.MstPowerGlasses
				.Where(p => p.pk_PowerGlassId == participant.PowerofGlassId)
				.Select(p => p.PowerofGlass)
				.FirstOrDefaultAsync() ?? "N/A";

			return View(model);
		}
	}
}
