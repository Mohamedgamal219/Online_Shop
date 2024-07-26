using Microsoft.EntityFrameworkCore;
using myShop.Entity.Model;
using myShop.Entity.Repository;
using myShop.DataAccess.Implntation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using myShop.Uititlity;
using myShop.DataAccess.DbIntializer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


builder.Services.AddDbContext<AppDbContext>(sql=>sql.UseSqlServer
(builder.Configuration.GetConnectionString("Constr")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4)
)
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IDbIntialize, Dbintialize>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// session 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapRazorPages();
app.UseStaticFiles();
seedDb();
app.UseRouting();

app.UseAuthorization();

app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area=Admin}/{controller=Category}/{action=Index}/{id?}");
app.Run();


void seedDb()
{
    using(var scope = app.Services.CreateScope())
    {
        var Dbinti = scope.ServiceProvider.GetRequiredService<IDbIntialize>();
        Dbinti.Intiaize();
    }
}