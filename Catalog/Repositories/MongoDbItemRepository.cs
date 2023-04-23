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

        public void CreateItem(Item item)
        {
            _itemsCollection.InsertOne(item);
        }

        public void DeleteItem(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(x => x.Id, id);
            _itemsCollection.DeleteOne(filter);
        }

        public Item GetItem(Guid id)
        {
            var filter = _filterDefinitionBuilder.Eq(x => x.Id, id);
            return _itemsCollection.Find(filter).FirstOrDefault();
        }

        public IEnumerable<Item> GetItems()
        {
            return _itemsCollection.Find(x => true).ToList();
        }

        public void UpdateItem(Item item)
        {
            var filter = _filterDefinitionBuilder.Eq(x => x.Id, item.Id);
            _itemsCollection.ReplaceOne(filter, item);
        }
    }
}