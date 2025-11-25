using GlassLP.Data;
using GlassLP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace GlassLP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly GlassDbContext _context;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly CommonData _commonData;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            GlassDbContext context,
            ICompositeViewEngine viewEngine,
            CommonData commonData)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _viewEngine = viewEngine;
            _commonData = commonData;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Create(string? id)
        {
            UserViewModel model = new UserViewModel();
            
            // Load dropdown lists
            model.RoleList = GetRoleList();
            model.DistrictList = GetDistrictList();
            model.BlockList = GetBlockList();
            model.PanchayatList = GetPanchayatList();
            model.CLFList = GetCLFList();

            if (!string.IsNullOrEmpty(id))
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    model.Id = user.Id;
                    model.UserName = user.UserName ?? string.Empty;
                    model.Email = user.Email ?? string.Empty;
                    model.Name = user.Name;
                    model.DistrictId = user.DistrictId;
                    model.BlockId = user.BlockId;
                    model.PanchayatId = user.PanchayatId;
                    model.CLFId = user.CLFId;
                    model.PhoneNumber = user.PhoneNumber;
                    model.IsActive = user.IsActive ?? true;
                    model.EmailConfirmed = user.EmailConfirmed;
                    model.CreatedBy = user.CreatedBy;
                    model.CreatedOn = user.CreatedOn;

                    // Get user role
                    var roles = _userManager.GetRolesAsync(user).Result;
                    if (roles.Any())
                    {
                        model.Role = roles.First();
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Result<ApplicationUser>> Create(UserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Result<ApplicationUser>.ValidationFailure(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList());
                }

                ApplicationUser user;
                bool isNewUser = string.IsNullOrEmpty(model.Id);

                if (isNewUser)
                {
                    // Check for duplicate username
                    var existingUserByUsername = await _userManager.FindByNameAsync(model.UserName);
                    if (existingUserByUsername != null)
                    {
                        return Result<ApplicationUser>.Failure("A user with this username already exists.");
                    }

                    // Check for duplicate phone number (if provided)
                    if (!string.IsNullOrEmpty(model.PhoneNumber))
                    {
                        var existingUserByPhone = await _context.Users
                            .FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber && (u.IsActive == true || u.IsActive == null));
                        if (existingUserByPhone != null)
                        {
                            return Result<ApplicationUser>.Failure("A user with this mobile number already exists.");
                        }
                    }

                    // Validate password for new user
                    if (string.IsNullOrEmpty(model.Password))
                    {
                        return Result<ApplicationUser>.Failure("Password is required for new user.");
                    }

                    user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Name = model.Name,
                        DistrictId = model.DistrictId,
                        BlockId = model.BlockId,
                        PanchayatId = model.PanchayatId,
                        CLFId = model.CLFId,
                        PhoneNumber = model.PhoneNumber,
                        EmailConfirmed = model.EmailConfirmed,
                        IsActive = model.IsActive,
                        CreatedBy = GetSubmittedBy(),
                        CreatedOn = DateTime.Now
                    };

                    var createResult = await _userManager.CreateAsync(user, model.Password!);
                    if (!createResult.Succeeded)
                    {
                        var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                        return Result<ApplicationUser>.Failure($"Failed to create user: {errors}");
                    }

                    // Assign role
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        var roleExists = await _roleManager.RoleExistsAsync(model.Role);
                        if (roleExists)
                        {
                            await _userManager.AddToRoleAsync(user, model.Role);
                        }
                    }
                }
                else
                {
                    // Update existing user
                    user = await _userManager.FindByIdAsync(model.Id);
                    if (user == null)
                    {
                        return Result<ApplicationUser>.Failure("User not found.");
                    }

                    // Check for duplicate username (excluding current user)
                    var existingUserByUsername = await _userManager.FindByNameAsync(model.UserName);
                    if (existingUserByUsername != null && existingUserByUsername.Id != model.Id)
                    {
                        return Result<ApplicationUser>.Failure("A user with this username already exists.");
                    }

                    // Check for duplicate phone number (excluding current user, if provided)
                    if (!string.IsNullOrEmpty(model.PhoneNumber))
                    {
                        var existingUserByPhone = await _context.Users
                            .FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber && u.Id != model.Id && (u.IsActive == true || u.IsActive == null));
                        if (existingUserByPhone != null)
                        {
                            return Result<ApplicationUser>.Failure("A user with this mobile number already exists.");
                        }
                    }

                    // Update user properties
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.Name = model.Name;
                    user.DistrictId = model.DistrictId;
                    user.BlockId = model.BlockId;
                    user.PanchayatId = model.PanchayatId;
                    user.CLFId = model.CLFId;
                    user.PhoneNumber = model.PhoneNumber;
                    user.EmailConfirmed = model.EmailConfirmed;
                    user.IsActive = model.IsActive;

                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                        return Result<ApplicationUser>.Failure($"Failed to update user: {errors}");
                    }

                    // Update password if provided
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                        if (!passwordResult.Succeeded)
                        {
                            var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                            return Result<ApplicationUser>.Failure($"Failed to update password: {errors}");
                        }
                    }

                    // Update role
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        var currentRoles = await _userManager.GetRolesAsync(user);
                        if (currentRoles.Any())
                        {
                            await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        }
                        
                        var roleExists = await _roleManager.RoleExistsAsync(model.Role);
                        if (roleExists)
                        {
                            await _userManager.AddToRoleAsync(user, model.Role);
                        }
                    }
                }

                var message = isNewUser 
                    ? "User created successfully!" 
                    : "User updated successfully!";
                
                return Result<ApplicationUser>.Success(user, message);
            }
            catch (Exception ex)
            {
                return Result<ApplicationUser>.Failure(ex.Message, ex);
            }
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "No Role";

            var model = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Name = user.Name,
                DistrictId = user.DistrictId,
                BlockId = user.BlockId,
                PanchayatId = user.PanchayatId,
                CLFId = user.CLFId,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive ?? true,
                EmailConfirmed = user.EmailConfirmed,
                Role = userRole,
                CreatedBy = user.CreatedBy,
                CreatedOn = user.CreatedOn
            };

            // Get related names for display
            if (user.DistrictId.HasValue)
            {
                ViewBag.DistrictName = await _context.MstDistrict
                    .Where(d => d.DistrictId_pk == user.DistrictId)
                    .Select(d => d.DistrictName)
                    .FirstOrDefaultAsync() ?? "N/A";
            }

            if (user.BlockId.HasValue)
            {
                ViewBag.BlockName = await _context.MstBlock
                    .Where(b => b.BlockId_pk == user.BlockId)
                    .Select(b => b.BlockName)
                    .FirstOrDefaultAsync() ?? "N/A";
            }

            if (user.PanchayatId.HasValue)
            {
                ViewBag.PanchayatName = await _context.MstPanchayat
                    .Where(p => p.PanchayatId_pk == user.PanchayatId)
                    .Select(p => p.PanchayatName)
                    .FirstOrDefaultAsync() ?? "N/A";
            }

            if (user.CLFId.HasValue)
            {
                ViewBag.CLFName = await _context.MstCLF
                    .Where(c => c.pk_CLFId == user.CLFId)
                    .Select(c => c.CLFName)
                    .FirstOrDefaultAsync() ?? "N/A";
            }

            return View(model);
        }

        
        public async Task<Result<string>> GetUserList()
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.IsActive == true || u.IsActive == null)
                    .OrderByDescending(u => u.CreatedOn)
                    .ToListAsync();

                var userList = new List<object>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var role = roles.FirstOrDefault() ?? "No Role";

                    userList.Add(new
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Name = user.Name ?? "N/A",
                        Role = role,
                        PhoneNumber = user.PhoneNumber ?? "N/A",
                        EmailConfirmed = user.EmailConfirmed,
                        CreatedBy = user.CreatedBy ?? "N/A",
                        CreatedOn = user.CreatedOn?.ToString("dd-MMM-yyyy") ?? "N/A"
                    });
                }

                string html = RenderPartialViewToString("_UserData", userList);
                return Result<string>.Success(html, "Record Found!!");
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

        // Helper methods for dropdowns
        private IEnumerable<SelectListItem> GetRoleList()
        {
            var roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name ?? string.Empty,
                Text = r.Name ?? string.Empty
            }).ToList();
            roles.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            return roles;
        }

        private IEnumerable<SelectListItem> GetDistrictList()
        {
            var districts = _context.MstDistrict
                .Where(d => d.IsActive == true)
                .Select(d => new SelectListItem
                {
                    Value = d.DistrictId_pk.ToString(),
                    Text = d.DistrictName ?? string.Empty
                }).ToList();
            districts.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            return districts;
        }

        private IEnumerable<SelectListItem> GetBlockList()
        {
            var blocks = _context.MstBlock
                .Where(b => b.IsActive == true)
                .Select(b => new SelectListItem
                {
                    Value = b.BlockId_pk.ToString(),
                    Text = b.BlockName ?? string.Empty
                }).ToList();
            blocks.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            return blocks;
        }

        private IEnumerable<SelectListItem> GetPanchayatList()
        {
            var panchayats = _context.MstPanchayat
                .Where(p => p.IsActive == true)
                .Select(p => new SelectListItem
                {
                    Value = p.PanchayatId_pk.ToString(),
                    Text = p.PanchayatName ?? string.Empty
                }).ToList();
            panchayats.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            return panchayats;
        }

        private IEnumerable<SelectListItem> GetCLFList()
        {
            var clfs = _context.MstCLF
                .Where(c => c.IsActive == true)
                .Select(c => new SelectListItem
                {
                    Value = c.pk_CLFId.ToString(),
                    Text = c.CLFName ?? string.Empty
                }).ToList();
            clfs.Insert(0, new SelectListItem { Value = "", Text = "Select" });
            return clfs;
        }
    }
}

