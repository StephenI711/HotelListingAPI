using HotelListingAPI.Configurations;
using HotelListingAPI.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
builder.Services.AddDbContext<HotelListingDbContext>(options => {
    options.UseSqlServer(connectionString);
    });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{                       //create CORS policy name "AllowAll" methods to specigy what is allowed
    options.AddPolicy("AllowAll", 
     b => b.AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod());
});
//buildercontext, loggingConfiguration| logging configuratiion write to console | read configurations from configuration file
//appsettings.json
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll"); // tell pipeline to use CORS

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
