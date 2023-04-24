using System;
using Catalog.Entities;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemRepository : IItemRepository
    {
        private const string DatabaseName = "Catalog";
        private const string CollectionName = "Items";

        private readonly FilterDefinitionBuilder<Item> _filterDefinitionBuilder = new();

        private readonly IMongoCollection<Item> _itemsCollection;

        public MongoDbItemRepository(IMongoClient oClient)
        {
            _itemsCollection = oClient.GetDatabase(DatabaseName).GetCollection<Item>(CollectionName);
        }

        public async Task CreateItemAsync(Item item)
        {
            await _itemsCollection.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(x => x.Id, id);
            await _itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(x => x.Id, id);
            return await _itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await _itemsCollection.Find(x => true).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            var filter = _filterDefinitionBuilder.Eq(x => x.Id, item.Id);
            await _itemsCollection.ReplaceOneAsync(filter, item);
        }
    }
}