using System.Diagnostics;
using HMS.Api;
using HMS.Api.Data;
using HMS.Api.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddXmlSerializerFormatters();
builder.Services.Configure<HospitalSettingsOptions>(
    builder.Configuration.GetSection(HospitalSettingsOptions.SectionName));

builder.Services.AddDbContext<HmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    var stopwatch = Stopwatch.StartNew();

    try
    {
        await next();

        stopwatch.Stop();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[Profiler] {context.Request.Method} {context.Request.Path} took {stopwatch.ElapsedMilliseconds} ms");
        Console.ResetColor();
    }
    catch (OperationCanceledException)
    {
        stopwatch.Stop();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[Profiler] CANCELLED: {context.Request.Method} {context.Request.Path} was ABORTED after {stopwatch.ElapsedMilliseconds} ms! Server freed immediately.");
        Console.ResetColor();
        throw;
    }
});

// 2. Branch: emergency requests
app.Map("/emergency", emergencyApp =>
{
    emergencyApp.Run(async context =>
    {
        await context.Response.WriteAsync("Emergency Department: Fast-track pipeline active!");
    });
});

app.MapControllers();

app.Run();