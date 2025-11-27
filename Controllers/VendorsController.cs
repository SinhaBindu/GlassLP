using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Data;
using System.Security.Claims;

namespace GlassLP.Controllers
{
	public class VendorsController : BaseController
	{
		private readonly GlassDbContext _context;
		private readonly ICompositeViewEngine _viewEngine;
		private readonly SPManager _spManager;
		int result = 0;
		public VendorsController(GlassDbContext context, ICompositeViewEngine viewEngine, SPManager spManager)
		{
			_context = context;
			_viewEngine = viewEngine;
			_spManager = spManager;
		}

		public async Task<IActionResult> Index()
		{
			return View();
		}


		public Result<string> GetVendorsList()
		{
			try
			{
				DataTable tbllist = _spManager.SP_VendorsList();
				if (tbllist.Rows.Count > 0)
				{
					string html = RenderPartialViewToString("_VendorData", tbllist);
					return Result<string>.Success(html, "Record Found!!");
				}
				else
				{
					return Result<string>.Failure("Record Not Found!!");
				}
			}
			catch (Exception ex)
			{
				return Result<string>.Failure(ex.Message, ex);
			}
		}
		private string RenderPartialViewToString(string viewName, object model)
		{
			ViewData.Model = model;

			using (var writer = new StringWriter())
			{
				var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

				if (viewResult.View == null)
					throw new ArgumentNullException($"{viewName} not found");

				var viewContext = new ViewContext(
					ControllerContext,
					viewResult.View,
					ViewData,
					TempData,
					writer,
					new HtmlHelperOptions()
				);

				viewResult.View.RenderAsync(viewContext).Wait();
				return writer.ToString();
			}
		}
		public IActionResult Create(int? CId)
		{
			VendorViewModel model = new VendorViewModel();
			if (CId > 0)
			{
				var tbl = _context.MstVendor.Find(CId);
				if (tbl != null)
				{
					model.BusinessMentorName = tbl.BusinessMentorName;
					model.VEName = tbl.VEName;
					model.CLFId = tbl.CLFId;
					model.AOPName = tbl.AOPName;
					model.ContactNumber = tbl.ContactNumber;
					model.TotalSalesAmount = tbl.TotalSalesAmount;
				}
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<Result<MstVendor>> Create(VendorViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var currentUser = GetSubmittedBy(); // your helper method or identity user
					var tbl = model.pk_VendorsId > 0 ? await _context.MstVendor.FindAsync(model.pk_VendorsId) : new MstVendor();
					if (tbl != null)
					{
						tbl.BusinessMentorName = model.BusinessMentorName;
						tbl.VEName = model.VEName;
						tbl.CLFId = model.CLFId;
						tbl.AOPName = model.AOPName;
						tbl.ContactNumber = model.ContactNumber;
						tbl.TotalSalesAmount = model.TotalSalesAmount;
						if (model.pk_VendorsId == 0)
						{
							tbl.CreatedBy = currentUser;
							tbl.CreatedOn = DateTime.Now;

							tbl.UpdatedBy = currentUser;
							tbl.UpdatedOn = DateTime.Now;
							tbl.IsActive = true;
						}

						_context.MstVendor.Add(tbl);
						result = await _context.SaveChangesAsync();
					}
					if (result > 0)
					{
						var message = model.pk_VendorsId == 0
							? "Vendor added successfully!"
							: "Vendor updated successfully!";
						return Result<MstVendor>.Success(tbl, message);
						
					}
					else
					{
						var errorMessage = model.pk_VendorsId == 0
							? "Error occurred while adding vendor."
							: "Error occurred while updating vendor.";
						return Result<MstVendor>.Failure(errorMessage);
					}
				}
				else
				{
					var errorMessage = model.pk_VendorsId == 0
							? "Error occurred while adding vendor."
							: "Error occurred while updating vendor.";
					return Result<MstVendor>.Failure(errorMessage);
				}
			}
			catch (Exception ex)
			{
				return Result<MstVendor>.Failure(ex.Message, ex);
			}

		}

	}
}
