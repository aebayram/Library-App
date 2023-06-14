/*
1-)Data katmanýna ApplicationDbContext.cs diye bir class açalým
2-)Ve onu DbContext 'ten kalýtarak, olmasý gereken constructor' yazalým
3-) Models katmanýmýzdaki modelleri birer DbSet olarak tanýmlayalým.
bunlarý yaparken, data katmanýna efcore kurmanýz gerekebilir.
4-)Web katmanýnda, bui1der.Services .AddDbCOntext komutuyla dependency injection kullanarak
context'imizi Katmanýmýza tanýtalým.
5-) appsettings.json dosyasýna connection string tanýmlamasý yapalým, database adýna "Library" diyelim.
*/



using Kutuphane.Data;
using Kutuphane.Repository.Abstract;
using Kutuphane.Repository.Concrete;
using Kutuphane.Repository.Shared.Abstract;
using Kutuphane.Repository.Shared.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Programımızın login işlemi gerektirdiğine dair bir authentication tanımlıyoruz.

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
	options.LoginPath = "/User/Login";
	options.AccessDeniedPath = "/User/UnAuthorized";
});


builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IBookRepository, BookRepository>();
//builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
//builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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