using MongoDB.Driver;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Repository;
using PLinkageAPI.Services;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using PLinkageAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("MongoDb");
    return new MongoClient(connectionString);
});
builder.Services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("PLinkageDB"));

builder.Services.AddSingleton(typeof(IRepository<>), typeof(MongoRepository<>));

builder.Services.AddScoped<ISkillProviderService, SkillProviderService>();
builder.Services.AddScoped<IProjectOwnerService, ProjectOwnerService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IOfferApplicationService, OfferApplicationService>();
builder.Services.AddScoped<IChatService, ChatService>();

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();