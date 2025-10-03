using MongoDB.Driver;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Repository;
using PLinkageAPI.ApplicationServices;
using PLinkageAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("MongoDb");
    return new MongoClient(connectionString);
});
builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("PLinkageDB")); // Registers IMongoDatabase to instance of the PLinkageDB

builder.Services.AddSingleton(typeof(IRepository<>), typeof(MongoRepository<>));

//builder.Services.AddScoped<ISkillProviderRepository, SkillProviderRepository>();
builder.Services.AddScoped<ISkillProviderService, SkillProviderService>();

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
