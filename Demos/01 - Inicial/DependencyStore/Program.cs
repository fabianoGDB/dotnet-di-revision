using DependencyStore;
using DependencyStore.Repositories;
using DependencyStore.Repositories.Contracts;
using DependencyStore.Services;
using DependencyStore.Services.Contracts;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

builder.Services.AddSingleton<Configuration>();
builder.Services.AddSqlConnection(builder.Configuration.GetConnectionString("Default"));
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();

app.MapControllers();

app.Run();