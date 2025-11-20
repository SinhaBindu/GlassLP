using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlassLP.Data;
using GlassLP.Models;

namespace GlassLP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : BaseController
    {
        private readonly GlassDbContext _context;

        public CampsController(GlassDbContext context)
        {
            _context = context;
        }

        // GET: api/Camps
        [HttpGet]
        public async Task<IActionResult> GetTblCamp()
        {
            var camps = await _context.TblCamp
                .Select(c => new
                {
                    campId_pk = c.CampId_pk,
                    typeOfModule = c.TypeOfModule,
                    typeOfVisit = c.TypeOfVisit,
                    campCode = c.CampCode,
                    districtId = c.DistrictId,
                    districtName = _context.MstDistrict
                        .Where(d => d.DistrictId_pk == c.DistrictId)
                        .Select(d => d.DistrictName)
                        .FirstOrDefault() ?? string.Empty,
                    blockId = c.BlockId,
                    blockName = _context.MstBlock
                        .Where(b => b.BlockId_pk == c.BlockId)
                        .Select(b => b.BlockName)
                        .FirstOrDefault() ?? string.Empty,
                    clfId = c.CLFId,
                    panchayatId = c.PanchayatId,
                    panchayatName = _context.MstPanchayat
                        .Where(p => p.PanchayatId_pk == c.PanchayatId)
                        .Select(p => p.PanchayatName)
                        .FirstOrDefault() ?? string.Empty,
                    voName = c.VOName,
                    campDate = c.CampDate,
                    location = c.Location,
                    crpName = c.CRPName,
                    crpMobileNo = c.CRPMobileNo,
                    participantMobilized = c.ParticipantMobilized,
                    totalScreened = c.TotalScreened,
                    totalGlassesDistributed = c.TotalGlassesDistributed,
                    powerOfGlassId = c.PowerOfGlassId,
                    photoUploadPath = c.PhotoUploadPath,
                    isActive = c.IsActive,
                    createdBy = c.CreatedBy,
                    createdOn = c.CreatedOn,
                    updatedBy = c.UpdatedBy,
                    updatedOn = c.UpdatedOn
                })
                .ToListAsync();

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Data fetched successfully",
                camps.Cast<object>().ToList()));
        }

        // GET: api/Camps/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblCamp(int id)
        {
            var camp = await _context.TblCamp
                .Where(c => c.CampId_pk == id)
                .Select(c => new
                {
                    campId_pk = c.CampId_pk,
                    typeOfModule = c.TypeOfModule,
                    typeOfVisit = c.TypeOfVisit,
                    campCode = c.CampCode,
                    districtId = c.DistrictId,
                    districtName = _context.MstDistrict
                        .Where(d => d.DistrictId_pk == c.DistrictId)
                        .Select(d => d.DistrictName)
                        .FirstOrDefault() ?? string.Empty,
                    blockId = c.BlockId,
                    blockName = _context.MstBlock
                        .Where(b => b.BlockId_pk == c.BlockId)
                        .Select(b => b.BlockName)
                        .FirstOrDefault() ?? string.Empty,
                    clfId = c.CLFId,
                    panchayatId = c.PanchayatId,
                    panchayatName = _context.MstPanchayat
                        .Where(p => p.PanchayatId_pk == c.PanchayatId)
                        .Select(p => p.PanchayatName)
                        .FirstOrDefault() ?? string.Empty,
                    voName = c.VOName,
                    campDate = c.CampDate,
                    location = c.Location,
                    crpName = c.CRPName,
                    crpMobileNo = c.CRPMobileNo,
                    participantMobilized = c.ParticipantMobilized,
                    totalScreened = c.TotalScreened,
                    totalGlassesDistributed = c.TotalGlassesDistributed,
                    powerOfGlassId = c.PowerOfGlassId,
                    photoUploadPath = c.PhotoUploadPath,
                    isActive = c.IsActive,
                    createdBy = c.CreatedBy,
                    createdOn = c.CreatedOn,
                    updatedBy = c.UpdatedBy,
                    updatedOn = c.UpdatedOn
                })
                .FirstOrDefaultAsync();

            if (camp == null)
            {
                return NotFound(new ApiResponse<List<object>>(
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

        // PUT: api/Camps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblCamp(int id, [FromBody] TblCamp tblCamp)
        {
            if (id != tblCamp.CampId_pk)
            {
                return BadRequest(new ApiResponse<List<object>>(
                    false,
                    "bad_request",
                    "ID mismatch.",
                    new List<object>()));
            }

            var existingCamp = await _context.TblCamp.FindAsync(id);
            if (existingCamp == null)
            {
                return NotFound(new ApiResponse<List<object>>(
                    false,
                    "not_found",
                    "Camp not found.",
                    new List<object>()));
            }

            try
            {
                // Update properties
                existingCamp.TypeOfModule = tblCamp.TypeOfModule;
                existingCamp.TypeOfVisit = tblCamp.TypeOfVisit;
                existingCamp.DistrictId = tblCamp.DistrictId;
                existingCamp.BlockId = tblCamp.BlockId;
                existingCamp.CLFId = tblCamp.CLFId;
                existingCamp.PanchayatId = tblCamp.PanchayatId;
                existingCamp.VOName = tblCamp.VOName;
                existingCamp.CampDate = tblCamp.CampDate;
                existingCamp.Location = tblCamp.Location;
                existingCamp.CRPName = tblCamp.CRPName;
                existingCamp.CRPMobileNo = tblCamp.CRPMobileNo;
                existingCamp.ParticipantMobilized = tblCamp.ParticipantMobilized;
                existingCamp.TotalScreened = tblCamp.TotalScreened;
                existingCamp.TotalGlassesDistributed = tblCamp.TotalGlassesDistributed;
                existingCamp.PowerOfGlassId = tblCamp.PowerOfGlassId;
                existingCamp.PhotoUploadPath = tblCamp.PhotoUploadPath;
                existingCamp.IsActive = tblCamp.IsActive;
                existingCamp.UpdatedBy = GetSubmittedBy();
                existingCamp.UpdatedOn = DateTime.Now;

                _context.Entry(existingCamp).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Get updated camp with names
                var updatedCamp = await GetCampWithNamesAsync(id);

                return Ok(new ApiResponse<List<object>>(
                    true,
                    "OK",
                    "Camp updated successfully.",
                    new List<object> { updatedCamp }));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CampExistsAsync(id))
                {
                    return NotFound(new ApiResponse<List<object>>(
                        false,
                        "not_found",
                        "Camp not found.",
                        new List<object>()));
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Camps
        [HttpPost]
        public async Task<IActionResult> PostTblCamp([FromBody] TblCamp tblCamp)
        {
            try
            {
                var currentUser = GetSubmittedBy();

                tblCamp.CreatedBy = currentUser;
                tblCamp.CreatedOn = DateTime.Now;
                tblCamp.UpdatedBy = currentUser;
                tblCamp.UpdatedOn = DateTime.Now;

                // Generate camp code if not provided
                if (string.IsNullOrEmpty(tblCamp.CampCode))
                {
                    if (tblCamp.DistrictId.HasValue && tblCamp.BlockId.HasValue && tblCamp.PanchayatId.HasValue)
                    {
                        tblCamp.CampCode = await GenerateCampCodeWithIdsAsync(
                            tblCamp.DistrictId.Value,
                            tblCamp.BlockId.Value,
                            tblCamp.PanchayatId.Value);
                    }
                }

                _context.TblCamp.Add(tblCamp);
                await _context.SaveChangesAsync();

                // Get created camp with names
                var createdCamp = await GetCampWithNamesAsync(tblCamp.CampId_pk);

                return Ok(new ApiResponse<List<object>>(
                    true,
                    "OK",
                    "Camp created successfully.",
                    new List<object> { createdCamp }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse<List<object>>(
                    false,
                    "server_error",
                    "An error occurred while creating the camp.",
                    new List<object>()));
            }
        }

        // POST: api/Camps/AddCamp
        [HttpPost("AddCamp")]
        public async Task<IActionResult> AddCamp([FromBody] CampViewModel model)
        {
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

            try
            {
                var currentUser = GetSubmittedBy();

                var camp = new TblCamp
                {
                    TypeOfModule = model.TypeOfModule,
                    TypeOfVisit = model.TypeOfVisit,
                    DistrictId = model.DistrictId,
                    BlockId = model.BlockId,
                    CLFId = model.CLFId,
                    PanchayatId = model.PanchayatId,
                    VOName = model.VOName,
                    CampDate = model.CampDate,
                    Location = model.Location,
                    CRPName = model.CRPName,
                    CRPMobileNo = model.CRPMobileNo,
                    ParticipantMobilized = model.ParticipantMobilized,
                    TotalScreened = model.TotalScreened,
                    TotalGlassesDistributed = model.TotalGlassesDistributed,
                    PowerOfGlassId = model.PowerOfGlassId,
                    PhotoUploadPath = model.PhotoUploadPath ?? "na",
                    IsActive = true,
                    CreatedBy = currentUser,
                    CreatedOn = DateTime.Now,
                    UpdatedBy = currentUser,
                    UpdatedOn = DateTime.Now
                };

                // Generate camp code using new format: DDD_BBB_PPP_XXX
                if (model.DistrictId.HasValue && model.BlockId.HasValue && model.PanchayatId.HasValue)
                {
                    camp.CampCode = await GenerateCampCodeWithIdsAsync(
                        model.DistrictId.Value,
                        model.BlockId.Value,
                        model.PanchayatId.Value);
                }

                _context.TblCamp.Add(camp);
                await _context.SaveChangesAsync();

                // Get created camp with names
                var createdCamp = await GetCampWithNamesAsync(camp.CampId_pk);

                return Ok(new ApiResponse<List<object>>(
                    true,
                    "OK",
                    "Camp created successfully.",
                    new List<object> { createdCamp }));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse<List<object>>(
                    false,
                    "invalid_request",
                    ex.Message,
                    new List<object>()));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse<List<object>>(
                    false,
                    "server_error",
                    "An error occurred while creating the camp.",
                    new List<object>()));
            }
        }

        // DELETE: api/Camps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblCamp(int id)
        {
            var tblCamp = await _context.TblCamp.FindAsync(id);
            if (tblCamp == null)
            {
                return NotFound(new ApiResponse<List<object>>(
                    false,
                    "not_found",
                    "Camp not found.",
                    new List<object>()));
            }

            _context.TblCamp.Remove(tblCamp);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<List<object>>(
                true,
                "OK",
                "Camp deleted successfully.",
                new List<object>()));
        }

        // Helper method to get camp with names
        private async Task<object> GetCampWithNamesAsync(int id)
        {
            return await _context.TblCamp
                .Where(c => c.CampId_pk == id)
                .Select(c => new
                {
                    campId_pk = c.CampId_pk,
                    typeOfModule = c.TypeOfModule,
                    typeOfVisit = c.TypeOfVisit,
                    campCode = c.CampCode,
                    districtId = c.DistrictId,
                    districtName = _context.MstDistrict
                        .Where(d => d.DistrictId_pk == c.DistrictId)
                        .Select(d => d.DistrictName)
                        .FirstOrDefault() ?? string.Empty,
                    blockId = c.BlockId,
                    blockName = _context.MstBlock
                        .Where(b => b.BlockId_pk == c.BlockId)
                        .Select(b => b.BlockName)
                        .FirstOrDefault() ?? string.Empty,
                    clfId = c.CLFId,
                    panchayatId = c.PanchayatId,
                    panchayatName = _context.MstPanchayat
                        .Where(p => p.PanchayatId_pk == c.PanchayatId)
                        .Select(p => p.PanchayatName)
                        .FirstOrDefault() ?? string.Empty,
                    voName = c.VOName,
                    campDate = c.CampDate,
                    location = c.Location,
                    crpName = c.CRPName,
                    crpMobileNo = c.CRPMobileNo,
                    participantMobilized = c.ParticipantMobilized,
                    totalScreened = c.TotalScreened,
                    totalGlassesDistributed = c.TotalGlassesDistributed,
                    powerOfGlassId = c.PowerOfGlassId,
                    photoUploadPath = c.PhotoUploadPath,
                    isActive = c.IsActive,
                    createdBy = c.CreatedBy,
                    createdOn = c.CreatedOn,
                    updatedBy = c.UpdatedBy,
                    updatedOn = c.UpdatedOn
                })
                .FirstOrDefaultAsync();
        }

        // Generate camp code using district name, block name, panchayat name + incrementing sequence
        // Format: DDD_BBB_PPP_XXX (first 3 chars of each name + 3-digit incrementing sequence)
        private async Task<string> GenerateCampCodeWithIdsAsync(int districtId, int blockId, int panchayatId)
        {
            // Get names from IDs
            var district = await _context.MstDistrict.FindAsync(districtId);
            var block = await _context.MstBlock.FindAsync(blockId);
            var panchayat = await _context.MstPanchayat.FindAsync(panchayatId);

            if (district == null || block == null || panchayat == null)
            {
                throw new ArgumentException("District, Block, or Panchayat not found.");
            }

            // Get first 3 characters from each name (uppercase)
            string districtName = district.DistrictName ?? string.Empty;
            string blockName = block.BlockName ?? string.Empty;
            string panchayatName = panchayat.PanchayatName ?? string.Empty;

            string districtPart = districtName.Trim().ToUpper().Substring(0, Math.Min(3, districtName.Length));
            string blockPart = blockName.Trim().ToUpper().Substring(0, Math.Min(3, blockName.Length));
            string panchayatPart = panchayatName.Trim().ToUpper().Substring(0, Math.Min(3, panchayatName.Length));

            // Find the last camp code with the same District/Block/Panchayat combination
            var lastCamp = await _context.TblCamp
                .Where(c => c.DistrictId == districtId &&
                           c.BlockId == blockId &&
                           c.PanchayatId == panchayatId &&
                           c.CampCode != null &&
                           c.CampCode.StartsWith($"{districtPart}{blockPart}{panchayatPart}"))
                .OrderByDescending(c => c.CampCode)
                .FirstOrDefaultAsync();

            int sequenceNumber = 0;

            if (lastCamp != null && !string.IsNullOrEmpty(lastCamp.CampCode))
            {
                // Extract the last 3 digits from the camp code
                if (lastCamp.CampCode.Length >= 3)
                {
                    string lastThreeDigits = lastCamp.CampCode.Substring(lastCamp.CampCode.Length - 3);
                    if (int.TryParse(lastThreeDigits, out int lastSequence))
                    {
                        sequenceNumber = lastSequence + 1;
                    }
                }
            }

            // Format sequence number as 3 digits
            string sequencePart = sequenceNumber.ToString("D3");

            // Combine: DDD_BBB_PPP_XXX (first 3 chars of names + 3-digit incrementing sequence)
            return $"{districtPart}{blockPart}{panchayatPart}{sequencePart}";
        }

        private async Task<bool> CampExistsAsync(int id)
        {
            return await _context.TblCamp.AnyAsync(e => e.CampId_pk == id);
        }
    }
}
