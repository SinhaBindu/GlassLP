using GlassLP.Data;
using GlassLP.Models;
using System.Data;

namespace GlassLP.Services.Interface
{
    public interface ICampService
    {
        Task<List<TblCamp>> GetAllCampsAsync();
        Task<TblCamp?> GetCampByIdAsync(int id);
        Task<TblCamp> CreateCampAsync(TblCamp camp, string currentUser);
        Task<TblCamp> UpdateCampAsync(TblCamp camp, string currentUser);
        Task<bool> DeleteCampAsync(int id);
        Task<bool> CampExistsAsync(int id);
        Task<Result<TblCamp>> AddOrUpdateCampAsync(CampViewModel model, IFormFile? photoFile, string currentUser);
        Task<TblCamp?> GetCampForEditAsync(int? id);
        DataTable GetCampm1List();
        DataTable GetCampm2List();
        string GenerateCampCode(int? districtId, int? blockId);
        string GenerateCampCodeFromNames(string districtName, string blockName, string panchayatName);
    }
}

