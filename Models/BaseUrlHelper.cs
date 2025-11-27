namespace GlassLP.Models
{
    public class BaseUrlHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseUrlHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host.Value}";
        }
    }
}
