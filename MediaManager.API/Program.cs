using FluentValidation;
using MediaManager.API;
using MediaManager.Application.Interfaces;
using MediaManager.Application.UseCases.Commands.Media.CreateMedia;
using MediaManager.Infrastructure.Persistence;
using MediaManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var dbPath = Path.Combine(builder.Environment.ContentRootPath, "mediamanager.db");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateMediaHandler).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(CreateMediaHandler).Assembly);

builder.Services.AddScoped<IMediaItemRepository, MediaItemRepository>();
builder.Services.AddScoped<IConsumptionRecordRepository, ConsumptionRecordRepository>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
