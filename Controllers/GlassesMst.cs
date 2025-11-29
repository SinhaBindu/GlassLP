using GlassLP.Data;
using GlassLP.Models;
using GlassLP.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GlassLP.Controllers
{
	public class GlassesMstController : BaseController
	{
		private readonly GlassDbContext _context;
		private readonly ICompositeViewEngine _viewEngine;
		private readonly CommonData _commonData;
		int result = 0;

		public GlassesMstController(GlassDbContext context, ICompositeViewEngine viewEngine, CommonData commonData)
		{
			_context = context;
			_viewEngine = viewEngine;
			_commonData = commonData;
		}

		// GET: GlassesMst
		public async Task<IActionResult> Index()
		{
			return View();
		}

		public Result GetGlassList()
		{
			try
			{
				var glasses = _context.MstGlass
					.Where(x => x.IsActive == true)
					.Select(g => new
					{
						pk_Glassid = g.pk_Glassid,
						TypeOfModuleId = g.TypeOfModuleId,
						TypeOfModuleName = g.TypeOfModuleId == 1 ? "Module One" : g.TypeOfModuleId == 2 ? "Module Two" : "",
						PowerOfGlassId = g.PowerOfGlassId,
						PowerOfGlassName = _context.MstPowerGlasses
							.Where(p => p.pk_PowerGlassId == g.PowerOfGlassId)
							.Select(p => p.PowerofGlass)
							.FirstOrDefault() ?? "",
						NoofGlasses = g.NoofGlasses,
						PerGlassCost = g.PerGlassCost,
						TotalGlassCost = g.TotalGlassCost,
						AdvertisingCost = g.AdvertisingCost,
						Storagemiscellaneouscost = g.Storagemiscellaneouscost,
						IsActive = g.IsActive
						
					})
					.ToList();

				if (glasses.Any())
				{
					// Convert to DataTable for consistency with other controllers
					DataTable dt = new DataTable();
					dt.Columns.Add("pk_Glassid", typeof(int));
					dt.Columns.Add("TypeOfModuleId", typeof(object));
					dt.Columns.Add("TypeOfModuleName", typeof(string));
					dt.Columns.Add("PowerOfGlassId", typeof(object));
					dt.Columns.Add("PowerOfGlassName", typeof(string));
					dt.Columns.Add("NoofGlasses", typeof(object));
					dt.Columns.Add("PerGlassCost", typeof(decimal));
					dt.Columns.Add("TotalGlassCost", typeof(decimal));
					dt.Columns.Add("AdvertisingCost", typeof(decimal));
					dt.Columns.Add("Storagemiscellaneouscost", typeof(decimal));
					dt.Columns.Add("IsActive", typeof(bool));

					foreach (var g in glasses)
					{
						DataRow row = dt.NewRow();
						row["pk_Glassid"] = g.pk_Glassid;
						row["TypeOfModuleId"] = g.TypeOfModuleId ;
						row["TypeOfModuleName"] = g.TypeOfModuleName ?? "";
						row["PowerOfGlassId"] = g.PowerOfGlassId ;
						row["PowerOfGlassName"] = g.PowerOfGlassName ?? "";
						row["NoofGlasses"] = g.NoofGlasses;
						row["PerGlassCost"] = g.PerGlassCost ?? null;
						row["TotalGlassCost"] = g.TotalGlassCost ?? null;
						if (g.AdvertisingCost != null)
							row["AdvertisingCost"] = g.AdvertisingCost != null ? g.AdvertisingCost : null;
						else
							row["AdvertisingCost"] = 0;
                        if (g.Storagemiscellaneouscost != null)
                            row["Storagemiscellaneouscost"] = g.Storagemiscellaneouscost!=null? g.Storagemiscellaneouscost : null;
                        else
                            row["Storagemiscellaneouscost"] = 0;
                        row["IsActive"] = g.IsActive ?? false;
						dt.Rows.Add(row);
					}

					string html = RenderPartialViewToString("_GlassData", dt);
					return Result.Success(html, "Record Found!!");
				}
				else
				{
					return Result.Failure("Record Not Found!!");
				}
			}
			catch (Exception ex)
			{
				return Result.Failure(ex.Message, ex);
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

		public IActionResult Create(int? GId)
		{
			GlassViewModel model = new GlassViewModel();
			model.TypeOfModuleList = _commonData.GetTypeOfModule();
			model.PowerOfGlassList = _commonData.GetPowerOfGlass();

			if (GId > 0)
			{
				var tbl = _context.MstGlass.Find(GId);
				if (tbl != null)
				{
					model.pk_Glassid = tbl.pk_Glassid;
					model.TypeOfModuleId = tbl.TypeOfModuleId ;
                    model.PowerOfGlassId = tbl.PowerOfGlassId ;
                    model.NoofGlasses = tbl.NoofGlasses ;
					model.PerGlassCost = tbl.PerGlassCost ;
                    model.TotalGlassCost = tbl.TotalGlassCost ;
                    model.AdvertisingCost = tbl.AdvertisingCost ;
                    model.Storagemiscellaneouscost = tbl.Storagemiscellaneouscost ;
                }
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<Result<MstGlass>> Create(GlassViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var currentUser = GetSubmittedBy();
					var tbl = model.pk_Glassid > 0 ? await _context.MstGlass.FindAsync(model.pk_Glassid) : new MstGlass();
					if (tbl != null)
					{
						tbl.TypeOfModuleId = model.TypeOfModuleId;
						tbl.PowerOfGlassId = model.PowerOfGlassId;
						tbl.NoofGlasses = model.NoofGlasses;
						tbl.PerGlassCost = model.PerGlassCost;
						tbl.TotalGlassCost = model.TotalGlassCost;
						tbl.AdvertisingCost = model.AdvertisingCost;
						tbl.Storagemiscellaneouscost = model.Storagemiscellaneouscost;

						if (model.pk_Glassid == 0)
						{
							tbl.CreatedBy = currentUser;
							tbl.CreatedOn = DateTime.Now;
							tbl.IsActive = true;
                            tbl.Availableglassinstock = model.NoofGlasses;
                            _context.MstGlass.Add(tbl);
						}
						else
						{
							tbl.UpdatedBy = currentUser;
							tbl.UpdatedOn = DateTime.Now;
							_context.Entry(tbl).State = EntityState.Modified;
						}

						result = await _context.SaveChangesAsync();
					}
					if (result > 0)
					{
						string message = model.pk_Glassid == 0 ? "Glass added successfully!" : "Glass updated successfully!";
						return Result<MstGlass>.Success(tbl, message);
					}
					else
					{
						string errorMessage = model.pk_Glassid == 0 ? "Error occurred while adding glass." : "Error occurred while updating glass.";
						return Result<MstGlass>.Failure(errorMessage);
					}
				}
				else
				{
					return Result<MstGlass>.ValidationFailure(ModelState);
				}
			}
			catch (Exception ex)
			{
				return Result<MstGlass>.Failure("Failed to save glass", ex);
			}
		}

		// GET: GlassesMst/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var mstGlass = await _context.MstGlass
				.FirstOrDefaultAsync(m => m.pk_Glassid == id);
			if (mstGlass == null)
			{
				return NotFound();
			}

			// Populate ViewBag with display names
			if (mstGlass.TypeOfModuleId > 0)
{
			ViewBag.TypeOfModuleName =
			mstGlass.TypeOfModuleId == 1 ? "Module One" :
			mstGlass.TypeOfModuleId == 2 ? "Module Two" : "";
}

            var powerOfGlass = await _context.MstPowerGlasses
			.Where(p => p.pk_PowerGlassId == mstGlass.PowerOfGlassId)
			.Select(p => new { Value = p.PowerofGlass })
			.FirstOrDefaultAsync();

            ViewBag.PowerOfGlassName = powerOfGlass?.Value ?? "";

            return View(mstGlass);
		}

		private bool MstGlassExists(int id)
		{
			return _context.MstGlass.Any(e => e.pk_Glassid == id);
		}

		[HttpGet]
		public async Task<IActionResult> GetAvailableGlassStock(int typeOfModuleId, int powerOfGlassId)
		{
			try
			{
				var availableStock = await _context.MstGlass
					.Where(g => g.TypeOfModuleId == typeOfModuleId 
						&& g.PowerOfGlassId == powerOfGlassId 
						&& g.IsActive == true)
					.SumAsync(g => g.Availableglassinstock ?? 0);

				return Json(new { status = true, data = new { availableStock = availableStock } });
			}
			catch (Exception ex)
			{
				return Json(new { status = false, message = ex.Message });
			}
		}
	}
}
