using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("StaffDb"));

builder.Services.AddScoped<StaffService>();
builder.Services.AddScoped<ExcelService>();

var app = builder.Build();
app.UseCors(option => option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Staff Management v1");
    // Makes Swagger UI available at root URL, otherwise, https://localhost:port/swagger
    options.RoutePrefix = string.Empty;
});

app.MapControllers();
app.Run();

