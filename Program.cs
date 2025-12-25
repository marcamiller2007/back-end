using API.Data;
using API.Controllers;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

//! ALLOWING CALLS FROM VUE
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Vue dev server
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//? Allows access to the user Table
builder.Services.AddControllers();
builder.Services.AddScoped<UserRepo>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//? Maps the http actions to our controller methods
app.MapControllers();

// For production scenarios, consider keeping Swagger configurations behind the environment check
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//! More VUE stuff
app.UseCors("AllowVueFrontend");
app.UseHttpsRedirection();

app.Run();
// REMEMBER TO RUN "az login" IN TERMINAL BEFORE RUNNING THE SERVER