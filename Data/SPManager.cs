using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using SubSonic.Schema;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace GlassLP.Data
{
    public class SPManager
    {
        private readonly IConfiguration _configuration;
        private readonly GlassDbContext _context;
        private int _lastNumber;
        private DateTime _lastDate;
        public SPManager(GlassDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _lastNumber = 0;
            _lastDate = DateTime.MinValue;
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
        public DataTable SP_Campm1List()
        {
            //    StoredProcedure sp = new StoredProcedure("SP_Campm1List");
            //    var dbCommand = sp.Command.ToDbCommand();
            //    dbCommand.CommandTimeout = 500; // Set timeout to 180 seconds-120//300 sec-5 mints
            //    DataTable dt = sp.ExecuteDataSet().Tables[0];
            //    return dt;
            DataTable dt = new DataTable();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand("SP_Campm1List", conn);
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

		public DataTable SP_ParticipantM1List()
		{
			DataTable dt = new DataTable();
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			using var conn = new SqlConnection(connectionString);
			using var cmd = new SqlCommand("SP_ParticipantM1List", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 500;
			using var da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			return dt;
		}

		public DataTable SP_ParticipantM2List()
		{
			DataTable dt = new DataTable();
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			using var conn = new SqlConnection(connectionString);
			using var cmd = new SqlCommand("SP_ParticipantM2List", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 500;
			using var da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			return dt;
		}
	}
}
