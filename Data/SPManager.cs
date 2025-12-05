using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using SubSonic.Schema;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using GlassLP.Models;
using static GlassLP.Data.Service;

namespace GlassLP.Data
{
	public class SPManager
	{
		private readonly IConfiguration _configuration;
		private readonly GlassDbContext _context;
		private int _lastNumber;
		private DateTime _lastDate;
		//private readonly GlobalDataService _globalData;
		public SPManager(GlassDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
			_lastNumber = 0;
			_lastDate = DateTime.MinValue;
			//_globalData = globalData;
		}
		//public string GenerateCode(int? DistrictId, int? BlockId)
		//{
		//    var district = _context.MstDistrict.Find(DistrictId);
		//    var disname = district != null && district.DistrictName != null
		//        ? district.DistrictName.Substring(0, 3).ToUpper()  // also "005"
		//        : "XXX";
		//    var block = _context.MstBlock.Find(BlockId);
		//    var blockname = block != null && block.BlockName != null
		//                  ? block.BlockName.Substring(0, 3).ToUpper()
		//                  : "XXX";

		//    DateTime today = DateTime.Today;
		//    // Reset counter if new day
		//    if (today != _lastDate)
		//    {
		//        _lastNumber = 1;
		//        _lastDate = today;
		//    }
		//    else
		//    {
		//        _lastNumber++;
		//    }
		//    string datePart = (disname + blockname) + today.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
		//    string numberPart = _lastNumber.ToString("D3"); // 3 digits with leading zeros

		//    string code = datePart + numberPart;
		//    return code;
		//}

		public string GenerateCode(int? DistrictId, int? BlockId)
		{
			if (_lastNumber == 0)
				_lastNumber = 1;
			else
				_lastNumber++;
			var district = _context.MstDistrict.Find(DistrictId);
			var disname = district?.DistrictName?.Substring(0, 3).ToUpper() ?? "XXX";

			var block = _context.MstBlock.Find(BlockId);
			var blockname = block?.BlockName?.Substring(0, 3).ToUpper() ?? "XXX";

			_lastNumber++; // हर बार increment +1

			string datePart = disname + blockname + DateTime.Today.ToString("ddMMyyyy");
			string numberPart = _lastNumber.ToString("D3");

			return datePart + numberPart;
		}
		public DataTable SP_CampList(Filtermodel filtermodel)
		{
			//    StoredProcedure sp = new StoredProcedure("SP_Campm1List");
			//    var dbCommand = sp.Command.ToDbCommand();
			//    dbCommand.CommandTimeout = 500; // Set timeout to 180 seconds-120//300 sec-5 mints
			//    DataTable dt = sp.ExecuteDataSet().Tables[0];
			//    return dt;
			DataTable dt = new DataTable();
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			using var conn = new SqlConnection(connectionString);
			using var cmd = new SqlCommand("SP_CampList", conn);
			cmd.Parameters.AddWithValue("@DistrictId", filtermodel.DistrictIdsstr);
			cmd.Parameters.AddWithValue("@BlockId", filtermodel.BlockId);
			cmd.Parameters.AddWithValue("@VISId", filtermodel.VisionIssueId);
			cmd.Parameters.AddWithValue("@PWId", filtermodel.PowerOfGlassId);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 500;
			using var da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			return dt;
		}

		public DataTable SP_VendorsList()
		{
			DataTable dt = new DataTable();
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			using var conn = new SqlConnection(connectionString);
			using var cmd = new SqlCommand("SP_VendorsList", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 500;
			using var da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			return dt;
		}

		public DataTable SP_ParticipantM1List(Filtermodel filtermodel)
		{
			DataTable dt = new DataTable();
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			using var conn = new SqlConnection(connectionString);
			using var cmd = new SqlCommand("SP_ParticipantM1List", conn);
			cmd.Parameters.AddWithValue("@DistrictId", filtermodel.DistrictId);
			cmd.Parameters.AddWithValue("@BlockId", filtermodel.BlockId);
			cmd.Parameters.AddWithValue("@VISId", filtermodel.VisionIssueId);
			cmd.Parameters.AddWithValue("@PWId", filtermodel.PowerOfGlassId);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 500;
			using var da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			return dt;
		}

		public DataTable SP_ParticipantM2List(Filtermodel filtermodel)
		{
			DataTable dt = new DataTable();
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			using var conn = new SqlConnection(connectionString);
			using var cmd = new SqlCommand("SP_ParticipantM2List", conn);
			cmd.Parameters.AddWithValue("@DistrictId", filtermodel.DistrictId);
			cmd.Parameters.AddWithValue("@BlockId", filtermodel.BlockId);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 500;
			using var da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			return dt;
		}

		public DataTable SP_MaindashboardTopCount()
		{
			DataTable dt = new DataTable();
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			using var conn = new SqlConnection(connectionString);
			using var cmd = new SqlCommand("MaindashboardTopCount", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 500;
			using var da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			return dt;
		}
		public DataTable SP_ClassMasterList()
		{
			DataTable dt = new DataTable();
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			using var conn = new SqlConnection(connectionString);
			using var cmd = new SqlCommand("SP_ClassMaster", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 500;
			using var da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			return dt;
		}
		public DataTable SP_UserList()
		{
			DataTable dt = new DataTable();
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			using var conn = new SqlConnection(connectionString);
			using var cmd = new SqlCommand("SP_UserList", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 500;
			using var da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			return dt;
		}
		public GlobalDataService GetLoggedInUser(string userId)
		{
			var VM = new GlobalDataService();
			string connectionString = _configuration.GetConnectionString("DefaultConnection");

			using var conn = new SqlConnection(connectionString);
			using var cmd = new SqlCommand("SP_UserList", conn);
			cmd.Parameters.AddWithValue("@UserId", userId);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 500;

			conn.Open();
			using var reader = cmd.ExecuteReader();

			if (reader.HasRows)
			{
				while (reader.Read())
				{
					VM.UserId = userId;
					VM.UserName = reader["LoginName"].ToString() ?? "";
					VM.Name = reader["LoginName"].ToString() ?? "";
					VM.PhoneNumber = reader["PhoneNumber"].ToString() ?? "";
					VM.Email = reader["Email"].ToString() ?? "";
					VM.Role = reader["RoleName"].ToString() ?? "";
					VM.RoleId = reader["RoleId"].ToString() ?? "";
					VM.DistrictIds = reader["DistrictIds"].ToString() ?? "";
					VM.BlockId = reader["BlockId"].ToString() ?? "";
					VM.CLFId = reader["CLFId"].ToString() ?? "";
					VM.DistrictName = reader["DistrictName"].ToString() ?? "";
					VM.BlockName = reader["BlockName"].ToString() ?? "";
					VM.CLFName = reader["FederationName"].ToString() ?? "";

					// Optional: Store current login time
					VM.LoginTime = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
				}
			}

			return VM;
		}
	}
}
