namespace GlassLP.Data
{
    public class UrlUtility
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }
        public static string GetBaseUrl()
        {
            var request = _httpContextAccessor?.HttpContext?.Request;

            if (request == null)
                return "";

            var scheme = request.Scheme;              // http / https
            var host = request.Host.Value;            // localhost:5001 or domain.com

            return $"{scheme}://{host}";
        }
    }
}
