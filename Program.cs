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
// Normalize request encoding (convert common UTF-16 payloads to UTF-8) to avoid garbled accents
app.Use(async (context, next) =>
{
    // Only operate for requests with a body
    if (context.Request.ContentLength.GetValueOrDefault() > 0)
    {
        context.Request.EnableBuffering();
        // read raw bytes
        using var ms = new System.IO.MemoryStream();
        await context.Request.Body.CopyToAsync(ms);
        var bytes = ms.ToArray();
        // detect BOM for UTF-16 LE/BE
        if (bytes.Length >= 2)
        {
            if (bytes[0] == 0xFF && bytes[1] == 0xFE)
            {
                // UTF-16 LE
                var str = System.Text.Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
                var utf8 = System.Text.Encoding.UTF8.GetBytes(str);
                context.Request.Body = new System.IO.MemoryStream(utf8);
                context.Request.ContentLength = utf8.Length;
                context.Request.ContentType = (context.Request.ContentType ?? "application/json") + "; charset=utf-8";
            }
            else if (bytes[0] == 0xFE && bytes[1] == 0xFF)
            {
                // UTF-16 BE
                var str = System.Text.Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2);
                var utf8 = System.Text.Encoding.UTF8.GetBytes(str);
                context.Request.Body = new System.IO.MemoryStream(utf8);
                context.Request.ContentLength = utf8.Length;
                context.Request.ContentType = (context.Request.ContentType ?? "application/json") + "; charset=utf-8";
            }
            else
            {
                // try to detect UTF-8 validity; if invalid, attempt best-effort conversion from UTF-16
                try
                {
                    _ = System.Text.Encoding.UTF8.GetString(bytes);
                    // valid UTF-8 - put stream back
                    context.Request.Body = new System.IO.MemoryStream(bytes);
                    context.Request.ContentLength = bytes.Length;
                }
                catch
                {
                    // fallback: interpret as UTF-16LE and convert
                    try
                    {
                        var str = System.Text.Encoding.Unicode.GetString(bytes);
                        var utf8 = System.Text.Encoding.UTF8.GetBytes(str);
                        context.Request.Body = new System.IO.MemoryStream(utf8);
                        context.Request.ContentLength = utf8.Length;
                        context.Request.ContentType = (context.Request.ContentType ?? "application/json") + "; charset=utf-8";
                    }
                    catch { context.Request.Body = new System.IO.MemoryStream(bytes); }
                }
            }
        }
        else
        {
            // small body - restore
            context.Request.Body = new System.IO.MemoryStream(bytes);
            context.Request.ContentLength = bytes.Length;
        }
        context.Request.Body.Position = 0;
    }
    await next();
});

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
