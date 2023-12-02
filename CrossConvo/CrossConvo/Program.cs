using Microsoft.EntityFrameworkCore;
using CrossConvo.Models;
//using CrossConvo.Hub.CrossConvo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();


var configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json")
.Build();
var connectionString = configuration.GetConnectionString("LocalSqlServerConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// pour base des données mysql
/*
var connectionString = configuration.GetConnectionString("LocalDbConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
*/

builder.Services.AddDistributedMemoryCache(); // Utilisation de la mémoire pour stocker les sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Définir la durée de la session
    options.Cookie.Name = ".AspNetCore.Session"; // Nom du cookie de session
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();


// Configurez la base de données et ajoutez des données de test
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Appliquer les migrations si nécessaire
    dbContext.SeedData(); // Ajouter des données de test
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
/*
app.MapHub<ChatHub>("/chatHub");
*/
app.Run();
