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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
