using dotnet_api.Entities;
using MongoDB.Driver;
using MongoDB.Bson;

namespace dotnet_api.Repositories
{
    public interface IItemsRepository
    {
        Task<Item?> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task CreateItemAsync(Item item);
        Task UpdatedItemAsync(Item item);
        Task DeleteItemAsync(Guid id);
    }

    public class ItemsRepository : IItemsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;
        private readonly FilterDefinitionBuilder<Item> filterDefinitionBuilder = Builders<Item>.Filter;

        public ItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }

        public async Task CreateItemAsync(Item item)
        {
            await this.itemsCollection.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var filter = this.filterDefinitionBuilder.Eq(existingItem => existingItem.id, id);
            await this.itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item?> GetItemAsync(Guid id)
        {
            var filter = this.filterDefinitionBuilder.Eq(item => item.id, id);
            return await this.itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await this.itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdatedItemAsync(Item item)
        {
            var filter = this.filterDefinitionBuilder.Eq(existingItem => existingItem.id, item.id);
            await this.itemsCollection.ReplaceOneAsync(filter, item);
        }
    }
}