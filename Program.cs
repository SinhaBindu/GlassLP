using GlassLP.Data;
using GlassLP.Middleware;
using GlassLP.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static GlassLP.Data.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
// Add DbContext
builder.Services.AddDbContext<GlassDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CommonData>();
builder.Services.AddScoped<GlobalDataService>();

// Add Identity with roles and the default UI
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // password & lockout settings (adjust as needed)
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

    options.User.RequireUniqueEmail = false; // Allow duplicate emails
})
.AddEntityFrameworkStores<GlassDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); // <--- important
builder.Services.AddRazorPages(); // required if you use the default Identity UI

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<SPManager>();
builder.Services.AddScoped<JWTHelper>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
	// ? MVC will use Cookie by default
	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})

/* ? COOKIE AUTH ? FOR MVC */
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
	options.LoginPath = "/Account/Login";
	options.LogoutPath = "/Account/Logout";
	options.AccessDeniedPath = "/Account/Login";
	options.ExpireTimeSpan = TimeSpan.FromDays(365);
	options.SlidingExpiration = true;
})

/* ? JWT AUTH ? FOR API */
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,

		ValidIssuer = jwtSettings["Issuer"],
		ValidAudience = jwtSettings["Audience"],

		IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(secretKey!)
		),

		ClockSkew = TimeSpan.Zero
	};
});
// Authorization
builder.Services.AddAuthorization();


var app = builder.Build();
UrlUtility.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // MUST before UseAuthorization
app.UseAuthorization();

// custom middleware after authentication
app.UseUserAuthenticationLogger();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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
    var defaultPassword = "User@123";//"Admin@1234";
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
