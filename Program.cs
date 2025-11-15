using GlassLP.Data;
using GlassLP.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add DbContext
builder.Services.AddDbContext<GlassDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity with roles and the default UI
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // password & lockout settings (adjust as needed)
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<GlassDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // required if you use the default Identity UI

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<SPManager>();

var app = builder.Build();
UrlUtility.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // MUST before UseAuthorization
app.UseAuthorization();

// custom middleware after authentication
app.UseUserAuthenticationLogger();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages(); // map Identity pages

// Seed roles and default user (same as your code)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    var adminRole = "Admin";
    if (!await roleManager.RoleExistsAsync(adminRole))
        await roleManager.CreateAsync(new IdentityRole(adminRole));

    var defaultUserName = "admin";
    var defaultEmail = "admin@gmail.com";
    var defaultPassword = "Admin@1234";
    var adminUser = await userManager.FindByEmailAsync(defaultEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser { UserName = defaultUserName, Email = defaultEmail, EmailConfirmed = true };
        var createResult = await userManager.CreateAsync(adminUser, defaultPassword);
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}

app.Run();
