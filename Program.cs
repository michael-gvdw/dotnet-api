using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using dotnet_api.Repositories;
using dotnet_api.Settings;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

// Add services to the container.
builder.Services.AddSingleton<IMongoClient>(serviceProvide => {
    var settings = builder.Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
    return new MongoClient(settings.connectionString);
});
builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();
builder.Services.AddControllers(options=> {
    options.SuppressAsyncSuffixInActionNames = false;
});
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

app.Run();
