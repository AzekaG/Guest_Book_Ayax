
using Microsoft.EntityFrameworkCore;
using Guest_Book_Ayax.Models;
using Guest_Book_Ayax.Repository;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(connection));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepository, UserRepository>();

var app = builder.Build();

app.UseSession();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
