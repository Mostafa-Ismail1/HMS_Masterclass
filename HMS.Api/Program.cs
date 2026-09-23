using System.Diagnostics;
using HMS.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<HospitalSettingsOptions>(
    builder.Configuration.GetSection(HospitalSettingsOptions.SectionName));

var app = builder.Build();

app.Use(async (context, next) =>
{
    var stopwatch = Stopwatch.StartNew();

    await next();

    stopwatch.Stop();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"[Profiler] {context.Request.Method} {context.Request.Path} took {stopwatch.ElapsedMilliseconds} ms");
    Console.ResetColor();
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