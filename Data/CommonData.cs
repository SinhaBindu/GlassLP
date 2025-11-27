using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Sockets;
using System.Net;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GlassLP.Data
{
    public class CommonData
    {
        #region BaseUrl
        private readonly GlassDbContext _context;
        public CommonData(GlassDbContext context, IConfiguration configuration)
        {
            _context = context;
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            // throw new Exception("No network adapters with an IPv4 address in the system!");
            return "Error";
        }
        public static string GetPublicIPAddress()
        {
            using (HttpClient client = new HttpClient())
            {
                string publicIPAddress = client.GetStringAsync("https://api.ipify.org").Result;
                return publicIPAddress;
            }
        }
        //public static string GetBaseUrl(HttpContext httpContext)
        //{
        //    var request = httpContext.Request;
        //    return $"{request.Scheme}://{request.Host.Value}";
        //}
        #endregion
        public List<SelectListItem> GetVE(int IsSelect = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.MstVendor.Where(SS => SS.IsActive == true).Select(SS => new SelectListItem { Value = SS.pk_VendorsId.ToString(), Text = SS.VEName }).ToList();
            if (IsSelect == 0)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }
            else if (IsSelect == 1)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "All" });
            }
            return list;
        }
        public List<SelectListItem> GetTypeOfModule(int IsSelect = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsSelect == 0)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }
            else if (IsSelect == 1)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "All" });
            }
            list.Add(new SelectListItem { Value = "1", Text = "Module One" });
            list.Add(new SelectListItem { Value = "2", Text = "Module Two" });
            return list;
        }
        public List<SelectListItem> GetPowerOfGlass(int IsSelect = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = _context.MstPowerGlasses.Where(SS => SS.IsActive == true).Select(SS => new SelectListItem { Value = SS.pk_PowerGlassId.ToString(), Text = SS.PowerofGlass }).ToList();
            if (IsSelect == 0)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }
            else if (IsSelect == 1)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "All" });
            }

            return list;
        }
        public List<SelectListItem> GetOccupation(int IsSelect = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsSelect == 0)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }
            else if (IsSelect == 1)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "All" });
            }
            list.Add(new SelectListItem { Value = "1", Text = "Tailoring" });
            list.Add(new SelectListItem { Value = "2", Text = "Farming" });
            list.Add(new SelectListItem { Value = "3", Text = "Embroidery" });
            list.Add(new SelectListItem { Value = "4", Text = "Household" });
            list.Add(new SelectListItem { Value = "5", Text = "Reading/writing" });
            list.Add(new SelectListItem { Value = "6", Text = "Others" });
            return list;
        }
        public List<SelectListItem> GetTypeofVisionIssue(int IsSelect = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsSelect == 0)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }
            else if (IsSelect == 1)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "All" });
            }
            list.Add(new SelectListItem { Value = "1", Text = "Presbyopia" });
            list.Add(new SelectListItem { Value = "2", Text = "Other" });
            return list;
        }
        public List<SelectListItem> GetFeedbackonComfort(int IsSelect = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsSelect == 0)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }
            else if (IsSelect == 1)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "All" });
            }
            list.Add(new SelectListItem { Value = "1", Text = "Satisfied" });
            list.Add(new SelectListItem { Value = "2", Text = "Not Satisfied" });
            list.Add(new SelectListItem { Value = "3", Text = "Adjustment Needed" });
            return list;
        }
        public List<SelectListItem> GetYesNo(int IsSelect = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsSelect == 0)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }
            else if (IsSelect == 1)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "All" });
            }
            list.Add(new SelectListItem { Value = "1", Text = "Yes" });
            list.Add(new SelectListItem { Value = "2", Text = "No" });
            return list;
        }
        public List<SelectListItem> GetTypeOfPaticipant(int IsSelect = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsSelect == 0)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }
            else if (IsSelect == 1)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "All" });
            }
            list.Add(new SelectListItem { Value = "1", Text = "New" });
            list.Add(new SelectListItem { Value = "2", Text = "Repeat Customers" });
            return list;
        }
        public List<SelectListItem> GetTypeOfVisit(int IsSelect = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsSelect == 0)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            }
            else if (IsSelect == 1)
            {
                list.Insert(0, new SelectListItem { Value = "", Text = "All" });
            }
            list.Add(new SelectListItem { Value = "1", Text = "Camp" });
            list.Add(new SelectListItem { Value = "2", Text = "Home Visit" });
            list.Add(new SelectListItem { Value = "3", Text = "Federation" });
            return list;
        }
        public class LoginModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string ISMobile { get; set; }
            public string Version { get; set; }
            public string JsonData { get; set; }
            public bool RememberMe { get; set; }
            public int RowAfected { get; set; }
        }
    }
}
