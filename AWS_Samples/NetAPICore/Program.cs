using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetAPICore.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("BooksConnection");
var sqlConnectionBuilder = new SqlConnectionStringBuilder(connectionString);
sqlConnectionBuilder.Password = builder.Configuration["DbPassword"];
sqlConnectionBuilder.UserID = builder.Configuration["DbUser"];


builder.Services.AddDbContext<BookContext>(options => options.UseSqlServer(sqlConnectionBuilder.ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
