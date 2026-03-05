using Microsoft.EntityFrameworkCore;
using GestionF1.Data.Entidades;
using GestionF1.Servicio;

var builder = WebApplication.CreateBuilder(args);

// 1. Al principio del archivo, agregá este método para parsear la URL de Railway
static string GetConnectionString(IConfiguration config)
{
    var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (string.IsNullOrEmpty(connectionUrl))
        return config.GetConnectionString("DefaultConnection");

    var databaseUri = new Uri(connectionUrl);
    var userInfo = databaseUri.UserInfo.Split(':');

    return $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.LocalPath.Substring(1)};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

// 2. Configurá el puerto dinámico (Vital para que Railway no te dé error de timeout)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");


// 3. Registrá el Contexto usando Npgsql (Postgres)
var connectionString = GetConnectionString(builder.Configuration);
builder.Services.AddDbContext<GestionF1Context>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddScoped<IPilotoServicio, PilotoServicio>();
builder.Services.AddScoped<IEscuderiasServicio, EscuderiasServicio>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pilotos}/{action=ListadoPilotos}/{id?}");

app.Run();


static string GetHerokuConnectionString()
{
    // Railway usa el mismo formato de URL que Heroku
    string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (string.IsNullOrEmpty(connectionUrl))
        return null;

    var databaseUri = new Uri(connectionUrl);
    var userInfo = databaseUri.UserInfo.Split(':');

    return $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.LocalPath.Substring(1)};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}