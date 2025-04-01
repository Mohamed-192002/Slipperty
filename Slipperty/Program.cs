using Business;
using Business.Helpers;
using Business.Managers;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Infrastructure.Contracts;
using Infrastructure.Contracts.Seeds;
using Infrastructure.Data;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");

//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.CommandTimeout(480)  // Set the command timeout to 180 seconds (3 minutes)
    )
);

//Read Whatsapp API Settings from appsettings
var WhatsappApiSetting = builder.Services.Configure<WhatsappAPISetting>(builder.Configuration.GetSection("Whatsapp"));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SlippertyContext>();



if (!builder.Environment.IsDevelopment())
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.File(
            "Logs/Errors", 
            rollingInterval: RollingInterval.Day 
        ).MinimumLevel.Error()
        .CreateLogger();
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();   
        loggingBuilder.AddSerilog();       
    });
}



builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._ ";
    options.User.RequireUniqueEmail = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();



//Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));
// Add services to the container.
builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options =>
{
    // Register the custom model binder at the top of the list
    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/User/Home/Index";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});


//Register Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Inject Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//Inject Businesses
builder.Services.AddScoped<ICategoriesBusiness, CategoriesBusiness>();
builder.Services.AddScoped<ISubCategoriesBusiness, SubCategoriesBusiness>();
builder.Services.AddScoped<IProductsBusiness, ProductsBusiness>();
builder.Services.AddScoped<IBrandsBusiness, BrandsBusiness>();
builder.Services.AddScoped<IMaterialsBusiness, MaterialsBusiness>();
builder.Services.AddScoped<IManufacturingBusiness, ManufacturingBusiness>();
builder.Services.AddScoped<IFAQsBusiness, FAQsBusiness>();
builder.Services.AddScoped<IProductTypesBusiness, ProductTypesBusiness>();
builder.Services.AddScoped<IBlockedNumbersBusiness, BlockedNumbersBusiness>();
builder.Services.AddScoped<IBannersBusiness, BannersBusiness>();
builder.Services.AddScoped<ILinkTypesBusiness, LinkTypesBusiness>();
builder.Services.AddScoped<ILinksBusiness, LinksBusiness>();
builder.Services.AddScoped<IReviewsBusiness, ReviewsBusiness>();
builder.Services.AddScoped<IReportsBusiness, ReportsBusiness>();
builder.Services.AddScoped<IGovernmentsBusiness, GovernmentsBusiness>();
builder.Services.AddScoped<IRegionsBusiness, RegionsBusiness>();
builder.Services.AddHttpClient<IWhatsAppAPIBusiness, WhatsAppAPIBusiness>();
builder.Services.AddScoped<IPaymentMethodsBusiness, PaymentMethodsBusiness>();
builder.Services.AddScoped<IAddressTypesBusiness, AddressTypesBusiness>();
builder.Services.AddScoped<IUserAddressesBusiness, UserAddressesBusiness>();
builder.Services.AddScoped<IUserPaymentMethodsBusiness, UserPaymentMethodsBusiness>();
builder.Services.AddScoped<IShoppingCartsBusiness, ShoppingCartsBusiness>();
builder.Services.AddScoped<IOrdersBusiness, OrdersBusiness>();
builder.Services.AddScoped<IPixelSettingsBusiness, PixelSettingsBusiness>();
builder.Services.AddScoped<IGoogleSheetBusiness, GoogleSheetBusiness>();
builder.Services.AddScoped<OrderContextService>();
builder.Services.AddScoped<OrderTransactionService>();
builder.Services.AddScoped<INotifyOrderTransactionService, NotifyOrderTransactionService>();
builder.Services.AddSingleton<OrderLockService, OrderLockService>(); 
builder.Services.AddSignalR();
builder.Services.AddHostedService<StoredProcedureBackgroundService>();

var app = builder.Build();

//Seed Data 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var context = services.GetRequiredService<ApplicationDbContext>();

        // // Seed roles and users
        await DefaultRoles.SeedRolesAsync(roleManager);
        await DefaultUsers.SeedAdminsUsersAsync(userManager, roleManager);
        await DefaultData.SeedPaymentMethodsAsync(context);
        await DefaultData.SeedAddressTypesAsync(context);

        logger.LogInformation("Data seeded");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding data");
    }
}
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); 
}




if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotifyIOrderTransaction>("/notifyIOrderTransaction");

    

app.Run();
