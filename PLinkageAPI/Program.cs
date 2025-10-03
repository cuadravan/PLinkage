using MongoDB.Driver;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Models;
using PLinkageAPI.Repository;
using PLinkageAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registers IMongoClient with a factory method for getting our MongoClient
// Program.cs
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("MongoDb");
    return new MongoClient(connectionString);
});
builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("PLinkageDB")); // Registers IMongoDatabase to instance of the PLinkageDB
builder.Services.AddScoped<ProjectOwnerRepository>();
//builder.Services.AddScoped<SkillProviderRepository>();

// -------------------------------------------------------------------
// 🎯 Dependency Injection Declarations
// -------------------------------------------------------------------

//// 1. Register MongoDB Client and Database (Singleton/Scoped)
//// The MongoClient is usually registered as Singleton as it is safe to share across the application.
//builder.Services.AddSingleton<IMongoClient>(s =>
//    new MongoClient(builder.Configuration.GetConnectionString("MongoDbConnection"))
//);

//// The IMongoDatabase is registered as Scoped to ensure it uses the client within the request scope, 
//// and its name can be pulled from configuration.
//builder.Services.AddScoped<IMongoDatabase>(s =>
//{
//    var client = s.GetRequiredService<IMongoClient>();
//    var dbName = builder.Configuration["DatabaseName"] ?? "PLinkageDB"; // Use a fallback name
//    return client.GetDatabase(dbName);
//});


// 2. Register Repositories (Scoped)
// Repositories are typically scoped as they handle data access for a single request.
builder.Services.AddScoped<ISkillProviderRepository, SkillProviderRepository>();
// Add other repositories here (e.g., builder.Services.AddScoped<IUserRepository, UserRepository>();)


// 3. Register Services (Scoped)
// Application services containing business logic are also typically scoped.
builder.Services.AddScoped<ISkillProviderService, SkillProviderService>();
// Add other services here (e.g., builder.Services.AddScoped<IUserService, UserService>();)

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

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

app.Run();
