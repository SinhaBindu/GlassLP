using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassLP.Controllers
{
    public class ParticipantM1Controller : BaseController
    {
        private readonly GlassDbContext _context;

        public ParticipantM1Controller(GlassDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
			// LINQ query projecting directly into DTO class
			var result = (from p in _context.TblPaticipantM1
						  join o in _context.MstOccupation
						  on p.OccupationId equals o.pk_OccupationId
						  select new DTO.ParticipantDTO
						  {
                              ParticipantId_pk = p.ParticipantId_pk,
							  ParticipantName = p.ParticipantName,
							  MobileNo = p.MobileNo,
							  Age = p.Age,
							  SHGName = p.SHGName,
							  OccupatioName = o.OccupatioName
						  }).ToList();

			// Return list to the view
			return View(result);

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
                    model.SHGName=tbl.SHGName;
                    model.OccupationId = tbl.OccupationId;
                    model.Occupation_Others = tbl.Occupation_Others;
                    model.VisionIssueIdentifiedId=tbl.VisionIssueIdentifiedId;
                    model.TypeofVisionIssueId=tbl.TypeofVisionIssueId;
                    model.GlassesProvidedId=tbl.GlassesProvidedId;
                    model.PowerofGlassId=tbl.PowerofGlassId;
                    model.FeedbackonComfort=tbl.FeedbackonComfort;
                    model.Remarks = tbl.Remarks;
                    model.FollowupRequiredId=tbl.FollowupRequiredId;
                    model.DigitalConsentId=tbl.DigitalConsentId;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParticipantM1ViewModel model)
        {
            try
            {
				if (ModelState.IsValid)
				{
					var currentUser = GetSubmittedBy(); // your helper method or identity user
					TblPaticipantM1 tbl = new TblPaticipantM1();

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

					tbl.CreatedBy = currentUser;
					tbl.CreatedOn = DateTime.Now;
					tbl.UpdatedBy = currentUser;
					tbl.UpdatedOn = DateTime.Now;
					tbl.IsActive = true;

					_context.TblPaticipantM1.Add(tbl);
					await _context.SaveChangesAsync();

					TempData["Success"] = "PaticipantM1 added successfully!";
					return RedirectToAction(nameof(Index));
				}
			}
            catch (Exception)
            {

                throw;
            }
            return View(model);
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





