using GlassLP.Data;
using GlassLP.Models;
using GlassLP.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GlassLP.Services
{
    public class CampService : ICampService
    {
        private readonly GlassDbContext _context;
        private readonly SPManager _spManager;

        public CampService(GlassDbContext context, SPManager spManager)
        {
            _context = context;
            _spManager = spManager;
        }

        public async Task<List<TblCamp>> GetAllCampsAsync()
        {
            return await _context.TblCamp.ToListAsync();
        }

        public async Task<TblCamp?> GetCampByIdAsync(int id)
        {
            return await _context.TblCamp.FindAsync(id);
        }

        public async Task<TblCamp> CreateCampAsync(TblCamp camp, string currentUser)
        {
            camp.CreatedBy = currentUser;
            camp.CreatedOn = DateTime.Now;
            camp.UpdatedBy = currentUser;
            camp.UpdatedOn = DateTime.Now;

            if (string.IsNullOrEmpty(camp.CampCode))
            {
                // Try to generate code from names first (if provided), otherwise use IDs
                if (!string.IsNullOrEmpty(camp.DistrictName) && 
                    !string.IsNullOrEmpty(camp.BlockName) && 
                    !string.IsNullOrEmpty(camp.PanchayatName))
                {
                    camp.CampCode = GenerateCampCodeFromNames(camp.DistrictName, camp.BlockName, camp.PanchayatName);
                }
                else if (camp.DistrictId.HasValue && camp.BlockId.HasValue)
                {
                    camp.CampCode = GenerateCampCode(camp.DistrictId, camp.BlockId);
                }
            }

            _context.TblCamp.Add(camp);
            await _context.SaveChangesAsync();
            return camp;
        }

        public async Task<TblCamp> UpdateCampAsync(TblCamp camp, string currentUser)
        {
            camp.UpdatedBy = currentUser;
            camp.UpdatedOn = DateTime.Now;

            _context.Entry(camp).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return camp;
        }

        public async Task<bool> DeleteCampAsync(int id)
        {
            var camp = await _context.TblCamp.FindAsync(id);
            if (camp == null)
            {
                return false;
            }

            _context.TblCamp.Remove(camp);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CampExistsAsync(int id)
        {
            return await _context.TblCamp.AnyAsync(e => e.CampId_pk == id);
        }

        public async Task<Result<TblCamp>> AddOrUpdateCampAsync(CampViewModel model, IFormFile? photoFile, string currentUser)
        {
            try
            {
                var camp = model.CampId_pk > 0 
                    ? await _context.TblCamp.FindAsync(model.CampId_pk) 
                    : new TblCamp();

                if (camp == null)
                {
                    return Result<TblCamp>.Failure("Camp not found.");
                }

                // Map properties from view model to entity
                camp.DistrictId = model.DistrictId;
                camp.BlockId = model.BlockId;
                camp.CLFId = model.CLFId;
                camp.PanchayatId = model.PanchayatId;
                camp.VOName = model.VOName;
                camp.CampDate = model.CampDate;
                camp.Location = model.Location;
                camp.CRPName = model.CRPName;
                camp.CRPMobileNo = model.CRPMobileNo;
                camp.ParticipantMobilized = model.ParticipantMobilized;
                camp.TotalScreened = model.TotalScreened;
                camp.TotalGlassesDistributed = model.TotalGlassesDistributed;
                camp.PowerOfGlassId = model.PowerOfGlassId;

                camp.PhotoUploadPath = "na";

                if (model.CampId_pk == 0)
                {
                    // New camp
                    camp.CampCode = GenerateCampCode(model.DistrictId, model.BlockId);
                    camp.CreatedBy = currentUser;
                    camp.CreatedOn = DateTime.Now;
                    camp.UpdatedBy = currentUser;
                    camp.UpdatedOn = DateTime.Now;
                    camp.IsActive = true;
                }
                else
                {
                    // Update existing camp
                    camp.UpdatedBy = currentUser;
                    camp.UpdatedOn = DateTime.Now;
                }

                // Handle file upload
                if (photoFile != null && photoFile.Length > 0)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "campm1");
                    if (!Directory.Exists(uploadsDir))
                        Directory.CreateDirectory(uploadsDir);

                    var uniqueFileName = $"{camp.CampCode}_{photoFile.FileName}";
                    var filePath = Path.Combine(uploadsDir, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoFile.CopyToAsync(stream);
                    }

                    camp.PhotoUploadPath = "\\uploads" + "\\campm1" + "\\" + uniqueFileName;
                }

                if (model.CampId_pk == 0)
                {
                    _context.TblCamp.Add(camp);
                }
                else
                {
                    _context.Entry(camp).State = EntityState.Modified;
                }

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Result<TblCamp>.Success(camp, $"Camp added successfully! Camp Code is <b>{camp.CampCode}</b>");
                }
                else
                {
                    return Result<TblCamp>.Failure("Error occurred while saving camp.");
                }
            }
            catch (Exception ex)
            {
                return Result<TblCamp>.Failure("Failed to save camp", ex);
            }
        }

        public async Task<TblCamp?> GetCampForEditAsync(int? id)
        {
            if (id == null || id <= 0)
            {
                return null;
            }

            return await _context.TblCamp.FindAsync(id);
        }

        public DataTable GetCampm1List()
        {
            return _spManager.SP_Campm1List();
        }

        public DataTable GetCampm2List()
        {
            return _spManager.SP_Campm1List(); // Note: This seems to be the same as Campm1List based on the controller
        }

        public string GenerateCampCode(int? districtId, int? blockId)
        {
            return _spManager.GenerateCode(districtId, blockId);
        }

        public string GenerateCampCodeFromNames(string districtName, string blockName, string panchayatName)
        {
            return Utilities.Utility.GenerateCampCode(districtName, blockName, panchayatName);
        }
    }
}

