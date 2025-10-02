using MongoDB.Driver;
using PLinkageAPI.Models;
using PLinkageAPI.Repository;

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
builder.Services.AddScoped<SkillProviderRepository>();

// ProjectOwner has a "UserId"
builder.Services.AddScoped<IRepository<ProjectOwner>>(sp =>
    new MongoRepository<ProjectOwner>(
        sp.GetRequiredService<IMongoDatabase>(),
        "ProjectOwner",
        "UserId"
    )
);

//// Project has a "ProjectId"
//builder.Services.AddScoped<IRepository<Project>>(sp =>
//    new MongoRepository<Project>(
//        sp.GetRequiredService<IMongoDatabase>(),
//        "Project",
//        "ProjectId"
//    )
//);

//// OfferApplication has an "OfferApplicationId"
//builder.Services.AddScoped<IRepository<OfferApplication>>(sp =>
//    new MongoRepository<OfferApplication>(
//        sp.GetRequiredService<IMongoDatabase>(),
//        "OfferApplication",
//        "OfferApplicationId"
//    )
//);


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
