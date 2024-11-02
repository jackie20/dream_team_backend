using Dream_Team_Assessment.Services;
using Dream_Team_Assessment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StockApi.Services;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();


// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IStockService, StockService>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
// Use CORS policy
app.UseCors("AllowAllOrigins");


app.UseAuthorization();

app.MapControllers();

app.Run();

