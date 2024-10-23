
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WEBAPP_NATURPIURA.Interfaces;
using WEBAPP_NATURPIURA.Models1;
using WEBAPP_NATURPIURA.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IServicioRol, RoleService>();
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<NatupiuraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NaturPiuraDbConn")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x => x.LoginPath = new PathString("/Account/Login"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
