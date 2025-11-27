using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GlassLP.Controllers
{
    public class ParticipantM1Controller : BaseController
    {
        private readonly GlassDbContext _context;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly SPManager _spManager;
        private readonly CommonData _commonData;
        int result = 0;
        public ParticipantM1Controller(GlassDbContext context, ICompositeViewEngine viewEngine, SPManager spManager,CommonData commonData)
        {
            _context = context;
            _viewEngine = viewEngine;
            _spManager = spManager;
            _commonData = commonData;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ActivateSelected([FromBody] List<int> participantIds)
        {
            if (participantIds == null || !participantIds.Any())
            {
                return Json(Result.Failure("No participants selected."));
            }

            // Ensure distinct, positive IDs only
            var ids = participantIds.Where(id => id > 0).Distinct().ToList();
            if (!ids.Any())
            {
                return Json(Result.Failure("No valid participants selected."));
            }

            try
            {
                var currentUser = GetSubmittedBy() ?? "System";
                var now = DateTime.Now;

                // Build a comma-separated list of IDs for the SQL IN clause
                var idList = string.Join(",", ids);

                var sql = $@"
                            UPDATE tbl_PaticipantM1 
                            SET IsApproved = 1,
                            ApprovedBy = @p0,
                            ApprovedOn = @p1
                            WHERE ParticipantId_pk IN ({idList})";

                await _context.Database.ExecuteSqlRawAsync(sql, currentUser, now);

                return Json(Result.Success("Selected participants activated successfully."));
            }
            catch (Exception ex)
            {
                return Json(Result.Failure("Failed to activate participants: " + ex.Message));
            }
        }

        public Result<string> GetParticipantM1List(Filtermodel filtermodel)
        {
            try
            {
                DataTable tbllist = _spManager.SP_ParticipantM1List(filtermodel);
                if (tbllist.Rows.Count > 0)
                {
                    string html = RenderPartialViewToString("_ParticipantM1Data", tbllist);
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

        public IActionResult Create(int? PId,string Code="")
        {
            ParticipantM1ViewModel model = new ParticipantM1ViewModel();
            model.VSIdList = _commonData.GetTypeofVisionIssue();
            if (PId > 0)
            {
                var tbl = _context.TblPaticipantM1.Find(PId);
                if (tbl != null)
                {
                    model.ParticipantId_pk = tbl.ParticipantId_pk;
                    model.CampId_fk = tbl.CampId_fk;
                    model.ParticipantName = tbl.ParticipantName;
                    model.MobileNo = tbl.MobileNo;
                    model.Age = tbl.Age;
                    model.SHGName = tbl.SHGName;
                    model.OccupationId = tbl.OccupationId;
                    model.Occupation_Others = tbl.Occupation_Others;
                    model.VisionIssueIdentifiedId = tbl.VisionIssueIdentifiedId;
                    model.TypeofVisionIssueId = tbl.TypeofVisionIssueId;
                    model.GlassesProvidedId = tbl.GlassesProvidedId;
                    model.PowerofGlassId = tbl.PowerofGlassId;
                    model.FeedbackonComfort = tbl.FeedbackonComfort;
                    model.Remarks = tbl.Remarks;
                    model.FollowupRequiredId = tbl.FollowupRequiredId;
                    model.DigitalConsentId = tbl.DigitalConsentId;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Result<TblPaticipantM1>> Create(ParticipantM1ViewModel model)
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

                        var duplicateExists = await _context.TblPaticipantM1
                            .AnyAsync(p => p.IsActive == true &&
                                p.ParticipantName != null && p.ParticipantName.Trim() == participantName &&
                                p.MobileNo != null && p.MobileNo.Trim() == mobileNo);

                        if (duplicateExists)
                        {
                            return Result<TblPaticipantM1>.Failure("A participant with the same name and mobile number already exists.");
                        }
                    }
                    else
                    {
                        // For updates, check if another participant (excluding current one) has the same name and mobile
                        var participantName = model.ParticipantName?.Trim();
                        var mobileNo = model.MobileNo?.Trim();

                        var duplicateExists = await _context.TblPaticipantM1
                            .AnyAsync(p => p.IsActive == true &&
                                p.ParticipantId_pk != model.ParticipantId_pk &&
                                p.ParticipantName != null && p.ParticipantName.Trim() == participantName &&
                                p.MobileNo != null && p.MobileNo.Trim() == mobileNo);

                        if (duplicateExists)
                        {
                            return Result<TblPaticipantM1>.Failure("A participant with the same name and mobile number already exists.");
                        }
                    }

                    var currentUser = GetSubmittedBy(); // your helper method or identity user
                    var tbl = model.ParticipantId_pk > 0 ? await _context.TblPaticipantM1.FindAsync(model.ParticipantId_pk) : new TblPaticipantM1();
                    if (tbl != null)
                    {
                        tbl.CampId_fk = model.CampId_fk;
                        tbl.ParticipantName = model.ParticipantName;
                        tbl.MobileNo = model.MobileNo;
                        tbl.Age = model.Age;
                        tbl.SHGName = model.SHGName;
                        tbl.OccupationId = model.OccupationId;
                        tbl.Occupation_Others = model.Occupation_Others;
                        tbl.VisionIssueIdentifiedId = model.VisionIssueIdentifiedId;
                        tbl.TypeofVisionIssueId = model.TypeofVisionIssueId;
                        tbl.GlassesProvidedId = model.GlassesProvidedId;
                        tbl.PowerofGlassId = model.PowerofGlassId;
                        tbl.FeedbackonComfort = model.FeedbackonComfort;
                        tbl.Remarks = model.Remarks;
                        tbl.FollowupRequiredId = model.FollowupRequiredId;
                        tbl.DigitalConsentId = model.DigitalConsentId;

                        if (model.ParticipantId_pk == 0)
                        {
                            tbl.CreatedBy = currentUser;
                            tbl.CreatedOn = DateTime.Now;
                           
                            tbl.IsActive = true;
                            _context.TblPaticipantM1.Add(tbl);
                        }
                        else {
                            tbl.UpdatedBy = currentUser;
                            tbl.UpdatedOn = DateTime.Now;
                        }
                        result = await _context.SaveChangesAsync();
                    }
                    if (result > 0)
                    {
                        var message = model.ParticipantId_pk == 0 
                            ? "Paticipant model one added successfully!" 
                            : "Paticipant model one updated successfully!";
                        return Result<TblPaticipantM1>.Success(tbl, message);
                    }
                    else
                    {
                        var errorMessage = model.ParticipantId_pk == 0 
                            ? "Error occurred while adding paticipant model one." 
                            : "Error occurred while updating paticipant model one.";
                        return Result<TblPaticipantM1>.Failure(errorMessage);
                    }
                }
                else
                {
                    var errorMessage = model.ParticipantId_pk == 0 
                        ? "Error occurred while adding paticipant model one." 
                        : "Error occurred while updating paticipant model one.";
                    return Result<TblPaticipantM1>.Failure(errorMessage);
                }
            }
            catch (Exception ex)
            {

                return Result<TblPaticipantM1>.Failure(ex.Message, ex);
            }
        }
        // GET: ParticipantM1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var participant = await _context.TblPaticipantM1
                .FirstOrDefaultAsync(m => m.ParticipantId_pk == id);
            
            if (participant == null)
            {
                return NotFound();
            }

            // Create view model with related data
            var model = new ParticipantM1ViewModel
            {
                ParticipantId_pk = participant.ParticipantId_pk,
                CampId_fk = participant.CampId_fk,
                ParticipantName = participant.ParticipantName,
                MobileNo = participant.MobileNo,
                Age = participant.Age,
                SHGName = participant.SHGName,
                OccupationId = participant.OccupationId,
                Occupation_Others = participant.Occupation_Others,
                VisionIssueIdentifiedId = participant.VisionIssueIdentifiedId,
                TypeofVisionIssueId = participant.TypeofVisionIssueId,
                GlassesProvidedId = participant.GlassesProvidedId,
                PowerofGlassId = participant.PowerofGlassId,
                FeedbackonComfort = participant.FeedbackonComfort,
                Remarks = participant.Remarks,
                FollowupRequiredId = participant.FollowupRequiredId,
                DigitalConsentId = participant.DigitalConsentId,
                IsActive = participant.IsActive
            };

            // Get related names for display
            ViewBag.CampCode = await _context.TblCamp
                .Where(c => c.CampId_pk == participant.CampId_fk)
                .Select(c => c.CampCode)
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





