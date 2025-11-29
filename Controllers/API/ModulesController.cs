using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlassLP.Data;
using GlassLP.Models;
using GlassLP.Controllers;

namespace GlassLP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : BaseController
    {
        private readonly GlassDbContext _context;

        public ModulesController(GlassDbContext context)
        {
            _context = context;
        }

        // ========== ParticipantM1 Methods ==========

        // GET: api/Modules/M1participants
        [HttpGet("M1participants")]
        public async Task<IActionResult> GetM1()
        {
            var participants = await _context.TblPaticipantM1
                .Select(p => new
                {
                    participantId_pk = p.ParticipantId_pk,
                    campId_fk = p.CampId_fk,
                    campCode = _context.TblCamp
                        .Where(c => c.CampId_pk == p.CampId_fk)
                        .Select(c => c.CampCode)
                        .FirstOrDefault() ?? string.Empty,
                    participantName = p.ParticipantName,
                    mobileNo = p.MobileNo,
                    age = p.Age,
                    shgName = p.SHGName,
                    occupationId = p.OccupationId,
                    occupation_Others = p.Occupation_Others,
                    visionIssueIdentifiedId = p.VisionIssueIdentifiedId,
                    typeofVisionIssueId = p.TypeofVisionIssueId,
                    glassesProvidedId = p.GlassesProvidedId,
                    powerofGlassId = p.PowerofGlassId,
                    feedbackonComfort = p.FeedbackonComfort,
                    remarks = p.Remarks,
                    followupRequiredId = p.FollowupRequiredId,
                    digitalConsentId = p.DigitalConsentId,
                    isActive = p.IsActive,
                    createdBy = p.CreatedBy,
                    createdOn = p.CreatedOn,
                    updatedBy = p.UpdatedBy,
                    updatedOn = p.UpdatedOn
                })
                .ToListAsync();

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                participants.Cast<object>().ToList()));
        }

        // GET: api/Modules/M1participants/5
        [HttpGet("M1participants/{id}")]
        public async Task<IActionResult> GetM1(int id)
        {
            var participant = await _context.TblPaticipantM1
                .Where(p => p.ParticipantId_pk == id)
                .Select(p => new
                {
                    participantId_pk = p.ParticipantId_pk,
                    campId_fk = p.CampId_fk,
                    campCode = _context.TblCamp
                        .Where(c => c.CampId_pk == p.CampId_fk)
                        .Select(c => c.CampCode)
                        .FirstOrDefault() ?? string.Empty,
                    participantName = p.ParticipantName,
                    mobileNo = p.MobileNo,
                    age = p.Age,
                    shgName = p.SHGName,
                    occupationId = p.OccupationId,
                    occupation_Others = p.Occupation_Others,
                    visionIssueIdentifiedId = p.VisionIssueIdentifiedId,
                    typeofVisionIssueId = p.TypeofVisionIssueId,
                    glassesProvidedId = p.GlassesProvidedId,
                    powerofGlassId = p.PowerofGlassId,
                    feedbackonComfort = p.FeedbackonComfort,
                    remarks = p.Remarks,
                    followupRequiredId = p.FollowupRequiredId,
                    digitalConsentId = p.DigitalConsentId,
                    isActive = p.IsActive,
                    createdBy = p.CreatedBy,
                    createdOn = p.CreatedOn,
                    updatedBy = p.UpdatedBy,
                    updatedOn = p.UpdatedOn
                })
                .FirstOrDefaultAsync();

            if (participant == null)
            {
                return NotFound(new ApiResponse<List<object>>(
                    false,
                    "not_found",
                    "Participant not found.",
                    new List<object>()));
            }

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                new List<object> { participant }));
        }

        // POST: api/Modules/AddM1Detail
        [HttpPost("AddM1Detail")]
        public async Task<IActionResult> AddM1Detail([FromBody] ParticipantM1ViewModel model)
        {
            int result = 0;
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState
                    .Where(entry => entry.Value?.Errors.Count > 0)  
                    .ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<List<object>>(
                    false,
                    "validation_failed",
                    "Please provide all required fields.",
                    new List<object> { validationErrors }));
            }
            if (_context.TblPaticipantM1.Any(x => x.ParticipantName == model.ParticipantName && x.MobileNo == model.MobileNo) && model.ParticipantId_pk == 0)
            {
                return BadRequest(new ApiResponse<List<object>>(
                false, "server_error",
                "This Record Already Exist!!",
                new List<object>()));
            }
            try
            {
                var currentUser = GetSubmittedBy();
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
                        tbl.CreatedBy = model.CreatedBy;
                        tbl.CreatedOn = DateTime.Now;
                        tbl.UpdatedBy = model.UpdatedBy;
                        tbl.UpdatedOn = DateTime.Now;
                        tbl.IsActive = true;
                        tbl.Version=model.Version;
                        tbl.SynDate=model.SynDate;
                       // tbl.uuid = model.uuid;
                        _context.TblPaticipantM1.Add(tbl);
                    }
                    else
                    {
                        tbl.UpdatedBy = model.UpdatedBy;
                        tbl.UpdatedOn = DateTime.Now;
                    }
                    result = await _context.SaveChangesAsync();
                    // Get created/updated participant with camp code
                    var savedParticipant = await GetParticipantM1WithNamesAsync(tbl.ParticipantId_pk);
                    if (result > 0)
                    {
                        return Ok(new ApiResponse<List<object>>(true, "OK",
                            model.ParticipantId_pk > 0 ? "Participant updated successfully." : "Participant created successfully.",
                            new List<object> { savedParticipant }));
                    }
                }
                return StatusCode(300, new ApiResponse<List<object>>(
                    false, "server_error",
                    "All fields required!!",
                    new List<object>()));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<List<object>>(
                    false,
                    "invalid_request",
                    ex.Message,
                    new List<object>()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<object>>(
                    false,
                    "server_error",
                    "An error occurred while saving the participant: " + ex.Message,
                    new List<object>()));
            }
        }
        // Helper method to get ParticipantM1 with related names
        private async Task<object> GetParticipantM1WithNamesAsync(int id)
        {
            return await _context.TblPaticipantM1
                .Where(p => p.ParticipantId_pk == id)
                .Select(p => new
                {
                    participantId_pk = p.ParticipantId_pk,
                    campId_fk = p.CampId_fk,
                    campCode = _context.TblCamp
                        .Where(c => c.CampId_pk == p.CampId_fk)
                        .Select(c => c.CampCode)
                        .FirstOrDefault() ?? string.Empty,
                    participantName = p.ParticipantName,
                    mobileNo = p.MobileNo,
                    age = p.Age,
                    shgName = p.SHGName,
                    occupationId = p.OccupationId,
                    occupation_Others = p.Occupation_Others,
                    visionIssueIdentifiedId = p.VisionIssueIdentifiedId,
                    typeofVisionIssueId = p.TypeofVisionIssueId,
                    glassesProvidedId = p.GlassesProvidedId,
                    powerofGlassId = p.PowerofGlassId,
                    feedbackonComfort = p.FeedbackonComfort,
                    remarks = p.Remarks,
                    followupRequiredId = p.FollowupRequiredId,
                    digitalConsentId = p.DigitalConsentId,
                    isActive = p.IsActive,
                    createdBy = p.CreatedBy,
                    createdOn = p.CreatedOn,
                    updatedBy = p.UpdatedBy,
                    updatedOn = p.UpdatedOn,
                    version=p.Version,
                    syndate=p.SynDate
                    //uuid=p.uuid
                })
                .FirstOrDefaultAsync();
        }

        // ========== ParticipantM2 Methods ==========

        // GET: api/Modules/M2participants
        [HttpGet("M2participants")]
        public async Task<IActionResult> GetM2()
        {
            var participants = await _context.TblPaticipantM2
                .Select(p => new
                {
                    participantId_pk = p.ParticipantId_pk,
                    typeofParticipantId = p.TypeofParticipantId,
                    campId_fk = p.CampId_fk,
                    campCode = _context.TblCamp
                        .Where(c => c.CampId_pk == p.CampId_fk)
                        .Select(c => c.CampCode)
                        .FirstOrDefault() ?? string.Empty,
                    districtId = p.DistrictId,
                    blockId = p.BlockId,
                    participantName = p.ParticipantName,
                    mobileNo = p.MobileNo,
                    age = p.Age,
                    screeningDate = p.ScreeningDate,
                    shgName = p.SHGName,
                    occupationId = p.OccupationId,
                    occupation_Others = p.Occupation_Others,
                    visionIssueIdentifiedId = p.VisionIssueIdentifiedId,
                    typeofVisionIssueId = p.TypeofVisionIssueId,
                    glassesSold = p.GlassesSold,
                    powerofGlassId = p.PowerofGlassId,
                    feedbackonComfort = p.FeedbackonComfort,
                    followupRequiredId = p.FollowupRequiredId,
                    digitalConsentId = p.DigitalConsentId,
                    location = p.Location,
                    screeningCost = p.ScreeningCost,
                    glassesCost = p.GlassesCost,
                    remarksActionTaken = p.RemarksActionTaken,
                    isActive = p.IsActive,
                    createdBy = p.CreatedBy,
                    createdOn = p.CreatedOn,
                    updatedBy = p.UpdatedBy,
                    updatedOn = p.UpdatedOn,
                    syndate=p.SynDate,
                    version = p.Version
                })
                .ToListAsync();

            return Ok(new ApiResponse<List<object>>(
                true, "OK", "Data fetched successfully",
                participants.Cast<object>().ToList()));
        }

        // GET: api/Modules/M2participants/5
        [HttpGet("M2participants/{id}")]
        public async Task<IActionResult> GetM2(int id)
        {
            var participant = await _context.TblPaticipantM2
                .Where(p => p.ParticipantId_pk == id)
                .Select(p => new
                {
                    participantId_pk = p.ParticipantId_pk,
                    typeofParticipantId = p.TypeofParticipantId,
                    campId_fk = p.CampId_fk,
                    campCode = _context.TblCamp
                        .Where(c => c.CampId_pk == p.CampId_fk)
                        .Select(c => c.CampCode)
                        .FirstOrDefault() ?? string.Empty,
                    districtId = p.DistrictId,
                    blockId = p.BlockId,
                    participantName = p.ParticipantName,
                    mobileNo = p.MobileNo,
                    age = p.Age,
                    screeningDate = p.ScreeningDate,
                    shgName = p.SHGName,
                    occupationId = p.OccupationId,
                    occupation_Others = p.Occupation_Others,
                    visionIssueIdentifiedId = p.VisionIssueIdentifiedId,
                    typeofVisionIssueId = p.TypeofVisionIssueId,
                    glassesSold = p.GlassesSold,
                    powerofGlassId = p.PowerofGlassId,
                    feedbackonComfort = p.FeedbackonComfort,
                    followupRequiredId = p.FollowupRequiredId,
                    digitalConsentId = p.DigitalConsentId,
                    location = p.Location,
                    screeningCost = p.ScreeningCost,
                    glassesCost = p.GlassesCost,
                    remarksActionTaken = p.RemarksActionTaken,
                    isActive = p.IsActive,
                    createdBy = p.CreatedBy,
                    createdOn = p.CreatedOn,
                    updatedBy = p.UpdatedBy,
                    updatedOn = p.UpdatedOn,
                    syndate = p.SynDate,
                    version = p.Version
                })
                .FirstOrDefaultAsync();

            if (participant == null)
            {
                return NotFound(new ApiResponse<List<object>>(
                    false,
                    "not_found",
                    "Participant not found.",
                    new List<object>()));
            }

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                new List<object> { participant }));
        }

        // POST: api/Modules/AddM2Detail
        [HttpPost("AddM2Detail")]
        public async Task<IActionResult> AddM2Detail([FromBody] ParticipantM2ViewModel model)
        {
            int result = 0;
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState
                    .Where(entry => entry.Value?.Errors.Count > 0)
                    .ToDictionary(
                        entry => entry.Key,
                        entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

                return BadRequest(new ApiResponse<List<object>>(
                    false,
                    "validation_failed",
                    "Please provide all required fields.",
                    new List<object> { validationErrors }));
            }
            if (_context.TblPaticipantM2.Any(x => x.ParticipantName == model.ParticipantName && x.MobileNo == model.MobileNo) && model.ParticipantId_pk == 0)
            {
                return BadRequest(new ApiResponse<List<object>>(
                false, "server_error",
                "This Record Already Exist!!",
                new List<object>()));
            }
            try
            {
                var currentUser = GetSubmittedBy();
                var tbl = model.ParticipantId_pk > 0 ? await _context.TblPaticipantM2.FindAsync(model.ParticipantId_pk) : new TblPaticipantM2();
                if (tbl != null)
                {
                    tbl.TypeofParticipantId = model.TypeofParticipantId;
                    tbl.CampId_fk = model.CampId_fk;
                    tbl.DistrictId = model.DistrictId;
                    tbl.BlockId = model.BlockId;
                    tbl.ParticipantName = model.ParticipantName;
                    tbl.MobileNo = model.MobileNo;
                    tbl.Age = model.Age;
                    tbl.ScreeningDate = model.ScreeningDate;
                    tbl.SHGName = model.SHGName;
                    tbl.OccupationId = model.OccupationId;
                    tbl.Occupation_Others = model.Occupation_Others;
                    tbl.VisionIssueIdentifiedId = model.VisionIssueIdentifiedId;
                    tbl.TypeofVisionIssueId = model.TypeofVisionIssueId;
                    tbl.TypeofVisionIssue_Others = model.TypeofVisionIssue_Others;
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
                        tbl.CreatedBy = model.CreatedBy;
                        tbl.CreatedOn = DateTime.Now;
                        tbl.IsActive = true;
                        tbl.Version = model.Version;
                        tbl.SynDate = model.SynDate;
                       // tbl.uuid = model.uuid;
                    }
                    else
                    {
                        tbl.UpdatedBy = model.UpdatedBy;
                        tbl.UpdatedOn = DateTime.Now;
                    }
                    if (model.ParticipantId_pk == 0)
                    {
                        _context.TblPaticipantM2.Add(tbl);
                    }
                    result = await _context.SaveChangesAsync();
                    // Get created/updated participant with camp code
                    var savedParticipant = await GetParticipantM2WithNamesAsync(tbl.ParticipantId_pk);
                    if (result > 0)
                    {
                        return Ok(new ApiResponse<List<object>>(
                            true,
                            "OK",
                            model.ParticipantId_pk > 0 ? "Participant updated successfully." : "Participant created successfully.",
                            new List<object> { savedParticipant }));
                    }
                }
                return StatusCode(300, new ApiResponse<List<object>>(
                    false,
                    "server_error",
                    "All fields required!!",
                    new List<object>()));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<List<object>>(
                    false,
                    "invalid_request",
                    ex.Message,
                    new List<object>()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<object>>(
                    false,
                    "server_error",
                    "An error occurred while saving the participant: " + ex.Message,
                    new List<object>()));
            }
        }

        // Helper method to get ParticipantM2 with related names
        private async Task<object> GetParticipantM2WithNamesAsync(int id)
        {
            return await _context.TblPaticipantM2
                .Where(p => p.ParticipantId_pk == id)
                .Select(p => new
                {
                    participantId_pk = p.ParticipantId_pk,
                    typeofParticipantId = p.TypeofParticipantId,
                    campId_fk = p.CampId_fk,
                    campCode = _context.TblCamp
                        .Where(c => c.CampId_pk == p.CampId_fk)
                        .Select(c => c.CampCode)
                        .FirstOrDefault() ?? string.Empty,
                    districtId = p.DistrictId,
                    blockId = p.BlockId,
                    participantName = p.ParticipantName,
                    mobileNo = p.MobileNo,
                    age = p.Age,
                    screeningDate = p.ScreeningDate,
                    shgName = p.SHGName,
                    occupationId = p.OccupationId,
                    occupation_Others = p.Occupation_Others,
                    visionIssueIdentifiedId = p.VisionIssueIdentifiedId,
                    typeofVisionIssueId = p.TypeofVisionIssueId,
                    glassesSold = p.GlassesSold,
                    powerofGlassId = p.PowerofGlassId,
                    feedbackonComfort = p.FeedbackonComfort,
                    followupRequiredId = p.FollowupRequiredId,
                    digitalConsentId = p.DigitalConsentId,
                    location = p.Location,
                    screeningCost = p.ScreeningCost,
                    glassesCost = p.GlassesCost,
                    remarksActionTaken = p.RemarksActionTaken,
                    isActive = p.IsActive,
                    createdBy = p.CreatedBy,
                    createdOn = p.CreatedOn,
                    updatedBy = p.UpdatedBy,
                    updatedOn = p.UpdatedOn,
                    syndate = p.SynDate,
                    version = p.Version
                })
                .FirstOrDefaultAsync();
        }
    }
}
