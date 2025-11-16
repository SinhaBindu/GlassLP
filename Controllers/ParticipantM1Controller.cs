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
        int result = 0;
        public ParticipantM1Controller(GlassDbContext context, ICompositeViewEngine viewEngine, SPManager spManager)
        {
            _context = context;
            _viewEngine = viewEngine;
            _spManager = spManager;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public Result<string> GetParticipantM1List()
        {
            try
            {
                DataTable tbllist = _spManager.SP_ParticipantM1List();
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

        public IActionResult Create(int? PId)
        {
            ParticipantM1ViewModel model = new ParticipantM1ViewModel();
            if (PId > 0 && PId > 0)
            {
                var tbl = _context.TblPaticipantM1.Find(PId);
                if (tbl != null)
                {
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

                        //if (model.ParticipantId_pk == 0)
                        //{
                        //    tbl.CreatedBy = currentUser;
                        //    tbl.CreatedOn = DateTime.Now;

                        //    tbl.UpdatedBy = currentUser;
                        //    tbl.UpdatedOn = DateTime.Now;
                        //    tbl.IsActive = true;
                        //}
                        _context.TblPaticipantM1.Add(tbl);
                        result = await _context.SaveChangesAsync();
                    }
                    if (result > 0)
                    {
                        return Result<TblPaticipantM1>.Success(tbl, $"Paticipant model one  added successfully!");
                    }
                    else
                    {
                        return Result<TblPaticipantM1>.Failure("Error occurred while adding paticipant model one.");
                    }
                }
                else
                {
                    return Result<TblPaticipantM1>.Failure("Error occurred while adding paticipant model one.");
                }
            }
            catch (Exception ex)
            {

                return Result<TblPaticipantM1>.Failure(ex.Message, ex);
            }
        }


        // GET: Camps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCamp = await _context.TblCamp
                .FirstOrDefaultAsync(m => m.CampId_pk == id);
            if (tblCamp == null)
            {
                return NotFound();
            }

            return View(tblCamp);
        }

    }
}





