using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WaterQualityApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ---------- Налаштування БД ----------
var dbSection = builder.Configuration.GetSection("Database");
var provider = dbSection.GetValue<string>("Provider") ?? "Sqlite";

builder.Services.AddDbContext<WaterQualityContext>(options =>
{
    switch (provider)
    {
        case "SqlServer":
            options.UseSqlServer(dbSection.GetValue<string>("SqlServer"));
            break;
        case "Postgres":
            options.UseNpgsql(dbSection.GetValue<string>("Postgres"));
            break;
        case "MySql":
            options.UseMySql(
                dbSection.GetValue<string>("MySql"),
                ServerVersion.AutoDetect(dbSection.GetValue<string>("MySql")!)
            );
            break;
        case "Sqlite":
        default:
            options.UseSqlite(dbSection.GetValue<string>("Sqlite"));
            break;
    }
});

// ---------- MVC + версіонування ----------
builder.Services.AddControllers();

builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WaterQuality API v1", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "WaterQuality API v2", Version = "v2" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WaterQuality API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "WaterQuality API v2");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
