using library_api.DAL;
using library_api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    $"Server={builder.Configuration["MYSQL_DB_HOST"]};" +
    $"Database=library_db;" +
    $"User={builder.Configuration["MYSQL_DB_USER"]};" +
    $"Password={builder.Configuration["MYSQL_DB_PASS"]};";

builder.Services.AddDbContext<LibraryDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
//builder.Services.AddSingleton<ILibraryRepository, InMemoryLibraryRepository>(); // Session memory
builder.Services.AddScoped<ILibraryRepository, EfLibraryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/test", () => {
    return "Greetings";
});

app.Run();
