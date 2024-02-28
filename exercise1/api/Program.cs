using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.Data.Helpers;
using StargateAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
   options.Filters.Add<ExceptionHandlerAttribute>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "StargateAPI.xml");
    c.IncludeXmlComments(filePath);
});

builder.Services.AddDbContext<StargateContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("StarbaseApiDatabase")));

builder.Services.AddScoped<IExceptionLoggingHelper, ExceptionLoggingHelper>();

builder.Services.AddMediatR(cfg =>
{
    cfg.AddRequestPreProcessor<CreateAstronautDutyPreProcessor>();
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});
builder.Services.AddLogging();
var app = builder.Build();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader());
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


