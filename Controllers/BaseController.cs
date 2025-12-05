using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace GlassLP.Controllers
{
	public abstract class BaseController : Controller
	{
		//protected CommonData _commonData;
		protected string? UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		protected string? UserEmail => User.FindFirst(ClaimTypes.Email)?.Value;
		protected string? UserName => User.FindFirst(ClaimTypes.Name)?.Value;
		protected string? FirstName => User.FindFirst(ClaimTypes.GivenName)?.Value;
		protected string? LastName => User.FindFirst(ClaimTypes.Surname)?.Value;
		protected string? FullName => User.FindFirst("FullName")?.Value;
		protected string? DistrictId => User.FindFirst("DistrictId")?.Value;
		protected string? BlockId => User.FindFirst("BlockId")?.Value;
		protected string? CLFId => User.FindFirst("CLFId")?.Value;
		protected string? DistrictIds => User.FindFirst("DistrictIds")?.Value;
		protected List<string> UserRoles => User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
		protected bool IsAuthenticated => User.Identity?.IsAuthenticated == true;
		protected string DateFormat => User.FindFirst("date_format")?.Value ?? "dd-MMM-yyyy";
		public override async void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			// Set global ViewBag properties for all views

			ViewBag.IsAuthenticated = IsAuthenticated;

			ViewBag.UserEmail = UserEmail;

			ViewBag.UserName = UserName;

			ViewBag.FirstName = FirstName;

			ViewBag.LastName = LastName;

			ViewBag.FullName = FullName;

			ViewBag.UserRoles = UserRoles;

			ViewBag.UserId = UserId;

			ViewBag.DateFormat = DateFormat;

			// Set user display name (prefer FullName, fallback to FirstName + LastName, then UserName)

			ViewBag.UserDisplayName = !string.IsNullOrEmpty(FullName) ? FullName :

									 (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)) ? $"{FirstName} {LastName}" :

									 UserName ?? "User";
		}
		protected IActionResult RedirectToLogin(string? returnUrl = null)
		{
			if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
			{
				return RedirectToAction("Login", "Account", new { returnUrl });
			}
			return RedirectToAction("Login", "Account");
		}
		protected bool HasRole(string role)
		{
			return UserRoles.Contains(role);
		}
		protected bool HasAnyRole(params string[] roles)
		{
			return roles.Any(role => UserRoles.Contains(role));
		}
		// Authorization helper methods

		//protected async Task<bool> HasPermissionAsync(string permissionName)

		//{

		//    if (string.IsNullOrEmpty(UserId)) return false;

		//    var authorizationService = HttpContext.RequestServices

		//        .GetRequiredService<GMS.Application.Interfaces.IAuthorizationService>();

		//    return await authorizationService.HasPermissionAsync(UserId, permissionName);

		//}

		//protected async Task<bool> HasPermissionAsync(string moduleName, string actionName)

		//{

		//    if (string.IsNullOrEmpty(UserId)) return false;

		//    var authorizationService = HttpContext.RequestServices

		//        .GetRequiredService<GMS.Application.Interfaces.IAuthorizationService>();

		//    return await authorizationService.HasPermissionAsync(UserId, moduleName, actionName);

		//}

		//protected async Task<IEnumerable<string>> GetUserPermissionsAsync()

		//{

		//    if (string.IsNullOrEmpty(UserId)) return new List<string>();

		//    var authorizationService = HttpContext.RequestServices

		//        .GetRequiredService<GMS.Application.Interfaces.IAuthorizationService>();

		//    return await authorizationService.GetUserPermissionsAsync(UserId);

		//}

		//protected async Task<IEnumerable<MenuDto>> GetUserMenusAsync()

		//{

		//    if (string.IsNullOrEmpty(UserId)) return new List<MenuDto>();

		//    var authorizationService = HttpContext.RequestServices

		//        .GetRequiredService<GMS.Application.Interfaces.IAuthorizationService>();

		//    return await authorizationService.GetMenusByUserAsync(UserId);

		//}

		// Helper extension method to render view to string

		protected async Task<string> RenderViewToStringAsync(Controller controller, string viewName, object model)
		{
			controller.ViewData.Model = model;
			using (var writer = new StringWriter())
			{
				var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

				var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

				if (viewResult.View == null)
				{
					throw new ArgumentNullException($"{viewName} does not match any available view");
				}
				var viewContext = new ViewContext(

					controller.ControllerContext,

					viewResult.View,

					controller.ViewData,

					controller.TempData,

					writer,

					new HtmlHelperOptions()
				);
				await viewResult.View.RenderAsync(viewContext);
				return writer.GetStringBuilder().ToString();
			}
		}
		protected string GetSubmittedBy()
		{
			if (string.IsNullOrEmpty(UserId)) return "System";
			else
				return UserId;
		}
		protected string GetCurrentDistricts()
		{
			if (string.IsNullOrEmpty(DistrictIds)) return "System";
			else
				return DistrictIds;
		}

		public string SubmittedBy
		{
			get
			{
				if (string.IsNullOrEmpty(UserId)) return "System";

				else
					return UserId;
			}
		}
	}

}
