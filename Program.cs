using isgasoir;
using isgasoir.Services.ServiceApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//IConfiguration cf= new ConfigurationBuilder();
string? sconn = builder.Configuration.GetConnectionString("mycon");



builder.Services.AddDbContext<ApplicationContext>(op => op.UseSqlServer(sconn));

builder.Services.AddTransient(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));

builder.Services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

// Register a typed HttpClient for the simple LLM implementation
builder.Services.AddHttpClient<isgasoir.Services.ServiceApi.LLMApiImpl>();

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

// Ensure database created for local development (LocalDB)
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    ctx.Database.EnsureCreated();
}

// Serve frontend static files from wwwroot
// Serve default files and ensure HTML static files are sent with charset=utf-8
// Request logging middleware for diagnostics
app.Use(async (context, next) =>
{
    var loggerFactory = context.RequestServices.GetService(typeof(Microsoft.Extensions.Logging.ILoggerFactory)) as Microsoft.Extensions.Logging.ILoggerFactory;
    var logger = loggerFactory?.CreateLogger("RequestLogger");
    try
    {
        context.Request.EnableBuffering();
        using var reader = new System.IO.StreamReader(context.Request.Body, System.Text.Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        logger?.LogInformation("Incoming request {Method} {Path} Body: {Body}", context.Request.Method, context.Request.Path, body);
    }
    catch (System.Exception ex)
    {
        logger?.LogError(ex, "Failed to read request body");
    }
    await next();
});
app.UseDefaultFiles();
app.UseStaticFiles(new Microsoft.AspNetCore.Builder.StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var ct = ctx.Context.Response.ContentType;
        if (!string.IsNullOrEmpty(ct) && ct.StartsWith("text/html", System.StringComparison.OrdinalIgnoreCase))
        {
            if (!ct.Contains("charset=", System.StringComparison.OrdinalIgnoreCase))
            {
                ctx.Context.Response.ContentType = "text/html; charset=utf-8";
            }
        }
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
