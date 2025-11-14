using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SubSonic.Schema;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GlassLP.Data
{
    public class SPManager
    {
        private readonly IConfiguration _configuration;
        public SPManager(IConfiguration configuration)
        {
            _configuration = configuration;
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
    }
}
