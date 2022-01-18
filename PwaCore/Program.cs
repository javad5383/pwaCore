
using Microsoft.EntityFrameworkCore;
using PwaCore.Context;
using PwaCore.Services.Class;
using PwaCore.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PwaContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PWAConnection"));
});
builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
