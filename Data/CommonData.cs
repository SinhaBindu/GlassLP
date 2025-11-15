using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Sockets;
using System.Net;

namespace GlassLP.Data
{
    public class CommonData
    {
        #region BaseUrl
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

      
        #endregion
    }
}
