using DevJobs.API.Persistence;
using DevJobs.API.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conectionString = builder.Configuration.GetConnectionString("DevJobsCs");
builder.Services.AddDbContext<IJobvacancyRepository>(options =>
    //options.UseInMemoryDatabase("Devjobs"));
    options.UseSqlServer(conectionString));

builder.Services.AddScoped<IJobVacancyRepository, JobVacancyRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevJobs.API",
        Contact = new OpenApiContact
        {
            Name = "Moisés Silva",
            Email = "moises50hp@hotmail.com"
        }
    });

    string xmlFile = "DevJobs.API.xml";

    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath);
});

//Configuração de Log.

builder.Host.ConfigureAppConfiguration((hosttingContext, config) => {
    Serilog.Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.MSSqlServer(conectionString,
        sinkOptions: new MSSqlServerSinkOptions()
        {
            AutoCreateSqlTable = true,
            TableName = "Logs"
        })
        .WriteTo.Console()
        .CreateLogger();
    }).UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
