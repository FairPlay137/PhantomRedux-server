using Fondue.DebugHelpers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var version = Assembly.GetEntryAssembly()?.GetName().Version;
string displayableVersion;
if (version is not null)
{
    var buildDate = new DateTime(2000, 1, 1)
        .AddDays(version.Build).AddSeconds(version.Revision * 2);
    var netVersion = Environment.Version;
    displayableVersion = $"{version} (built {buildDate}, running on .NET {netVersion})";
}
else
    displayableVersion = "[unknown]";
#if DEBUG
displayableVersion += " (debug)";
#endif

DebugHelper.ColorfulWrite(new ColorfulString(ConsoleColor.Green, Console.BackgroundColor, $"Fondue - process started at " + DateTime.Now + "\n"));
DebugHelper.ColorfulWrite(new ColorfulString(ConsoleColor.Red, Console.BackgroundColor, "This software is experimental. Do not use this software in a production environment until development has progressed to a point where it can be deemed safe to do so.\n\n"));

Fondue.Db.Initialize();

app.UseHttpLogging();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync($"OK - Phantom Redux v{displayableVersion}");
});

app.MapGet("/robots.txt", async context =>
{
    await context.Response.WriteAsync("User-agent: *\nDisallow: /");
});

app.MapGet("/humans.txt", async context =>
{
    await context.Response.WriteAsync("Fondue is a reverse-engineered server reimplementation for the overseas version of Rhythm Thief and the Paris Caper.\r\n\r\n"+
        "The server is still very early in development, and as such a lot of stuff may either be missing or not behave as expected. Any efforts to improve the server are much appreciated!");
});

app.MapGet("/generate204", async context =>
{
    // for uptime monitoring
    context.Response.StatusCode = 204;
    await context.Response.WriteAsync("");
});

app.MapControllers();

app.Run();
