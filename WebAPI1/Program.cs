using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication2;
using WebApplication2.Controllers;
using WebApplication2.Models;

var builder = WebApplication.CreateBuilder(args);

SQLitePCL.Batteries.Init();

builder.Services.AddDbContext<ApplicationContext>(
    options =>
    {
        options.UseSqlite("DataSource=sqlite20250409.db");
    });

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
    dbContext.SeedData();
}

app.Run();
