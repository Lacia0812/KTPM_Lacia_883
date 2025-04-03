using ASC.DataAccess;
using ASC.DataAccess.Interface;
using Lab1_THKTPM.Configuration;
using Lab1_THKTPM.Data;
using Lab1_THKTPM.Services;
using Lab1_THKTPM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Kết nối cơ sở dữ liệu
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🔹 Đăng ký DbContext & UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 🔹 Cấu hình Identity (CHỈ ĐĂNG KÝ 1 LẦN)
//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
//{
// roviders();   options.SignIn.RequireConfirmedAccount = true;
//    options.User.RequireUniqueEmail = true;
//})
//.AddEntityFrameworkStores<ApplicationDbContext>()
//.AddDefaultTokenP

// 🔹 Đăng ký các dịch vụ cần thiết
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 🔹 Cấu hình AppSettings
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("AppSettings"));

// 🔹 Đăng ký dịch vụ email & SMS
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, AuthMessageSender>();



builder.Services.AddTransient<ISmsSender, AuthMessageSender>();

// 🔹 Đăng ký HttpContextAccessor (Chỉ cần 1 lần)
builder.Services.AddHttpContextAccessor();

// 🔹 Đăng ký cấu hình mở rộng (XÓA Identity trùng lặp ở đây)
builder.Services
    .AddConfig(builder.Configuration)
    .AddMyDependencyGroup();

// 🔹 Cấu hình bộ nhớ cache & session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// 🔹 Cấu hình Middleware
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseAuthentication(); // ✅ Đảm bảo chỉ gọi 1 lần
app.UseAuthorization();

// 🔹 Cấu hình Routes
app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.UseSession();

// 🔹 Khởi tạo dữ liệu Seed cho Identity (CHỈ ĐỌC, KHÔNG ĐĂNG KÝ Identity LẠI)
using (var scope = app.Services.CreateScope())
{
    var storageSeed = scope.ServiceProvider.GetRequiredService<IIdentitySeed>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var appSettings = scope.ServiceProvider.GetRequiredService<IOptions<ApplicationSettings>>();

    // Chạy Seed đồng bộ để tránh lỗi await trong Main()
    storageSeed.Seed(userManager, roleManager, appSettings).Wait();
}
// CreateNavigationCache
using (var scope = app.Services.CreateScope())
{
    var navigationCacheOperations = scope.ServiceProvider.GetRequiredService<INavigationCacheOperations>();
    await navigationCacheOperations.CreateNavigationCacheAsync();
}
app.MapRazorPages();

// 🔹 Chạy ứng dụng
app.Run();
