using Microsoft.EntityFrameworkCore;
using GestionF1.Data.Entidades;
using GestionF1.Servicio;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración del Puerto para Railway (Evita el error 502)
// Buscamos la variable de entorno PORT que asigna Railway, si no existe usamos el 5000 para local.
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// 2. Registro de Servicios Básicos de MVC
// Sin esto, MapControllerRoute no sabe cómo manejar tus controladores.
builder.Services.AddControllersWithViews();
// Este es el servicio que te estaba pidiendo el último error en los logs.
builder.Services.AddAuthorization();

// 3. Configuración de la Base de Datos (Postgres)
var connectionString = GetConnectionString(builder.Configuration);
builder.Services.AddDbContext<GestionF1Context>(options =>
    options.UseNpgsql(connectionString));

// 4. Inyección de Dependencias de tus Servicios de Negocio
builder.Services.AddScoped<IPilotoServicio, PilotoServicio>();
builder.Services.AddScoped<IEscuderiasServicio, EscuderiasServicio>();

var app = builder.Build();

// 5. Configuración del Pipeline de HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// El orden aquí es vital: Routing -> Authorization -> Endpoints
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pilotos}/{action=ListadoPilotos}/{id?}");

app.Run();

// Método para parsear la DATABASE_URL de Railway a un formato que Entity Framework entienda
static string GetConnectionString(IConfiguration config)
{
    var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (string.IsNullOrEmpty(connectionUrl))
        return config.GetConnectionString("DefaultConnection");

    var databaseUri = new Uri(connectionUrl);
    var userInfo = databaseUri.UserInfo.Split(':');

    // Construcción del string de conexión para PostgreSQL
    return $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.LocalPath.Substring(1)};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}