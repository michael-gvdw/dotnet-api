using dotnet_api.Entities;
using MongoDB.Driver;
using MongoDB.Bson;

namespace dotnet_api.Repositories
{
    public interface IItemsRepository
    {
        Item? GetItem(Guid id);
        IEnumerable<Item> GetItems();
        void CreateItem(Item item);
        void UpdatedItem(Item item);
        void DeleteItem(Guid id);
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

        public void CreateItem(Item item)
        {
            this.itemsCollection.InsertOne(item);
        }

        public void DeleteItem(Guid id)
        {
            var filter = this.filterDefinitionBuilder.Eq(existingItem => existingItem.id, id);
            this.itemsCollection.DeleteOne(filter);
        }

        public Item? GetItem(Guid id)
        {
            var filter = this.filterDefinitionBuilder.Eq(item => item.id, id);
            return this.itemsCollection.Find(filter).SingleOrDefault();
        }

        public IEnumerable<Item> GetItems()
        {
            return itemsCollection.Find(new BsonDocument()).ToList();
        }

        public void UpdatedItem(Item item)
        {
            var filter = this.filterDefinitionBuilder.Eq(existingItem => existingItem.id, item.id);
            this.itemsCollection.ReplaceOne(filter, item);
        }
    }
}