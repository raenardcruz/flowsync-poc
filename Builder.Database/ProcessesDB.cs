
using MongoDB.Bson;
using MongoDB.Driver;

namespace Builder.Database
{
    public class ProcessesDB : IDatabase<ProcessesDBModel>
    {
        private IMongoCollection<ProcessesDBModel> collection;
        const string collectionName = "processes";
        public ProcessesDB(string connectionstring, string databaseName)
        {
            string connectionString = connectionstring;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            collection = database.GetCollection<ProcessesDBModel>(collectionName);
        }
        public async Task Add(ProcessesDBModel entity)
        {
            await collection.InsertOneAsync(entity);
        }

        public async Task Delete(object id)
        {
            var filter = Builders<ProcessesDBModel>.Filter.Eq(p => p.id, id);
            var result = await collection.DeleteOneAsync(filter);
        }

        public async Task<ProcessesDBModel> ReadWebhook(object id, string name)
        {
            var filter = Builders<ProcessesDBModel>.Filter.And(
                    Builders<ProcessesDBModel>.Filter.Eq(p => p.id, id),
                    Builders<ProcessesDBModel>.Filter.Eq(p => p.webhookName, name),
                    Builders<ProcessesDBModel>.Filter.Eq(p => p.type, "webhook")
                );
            var process = await collection.Find(filter).FirstOrDefaultAsync();
            return process;
        }

        public async Task<ProcessesDBModel> Read(object id)
        {
            var filter = Builders<ProcessesDBModel>.Filter.Eq(p => p.id, id);
            var process = await collection.Find(filter).FirstOrDefaultAsync();
            return process;
        }

        public async Task<List<ProcessesDBModel>> ReadAll()
        {
            var processes = await collection.Find(new BsonDocument()).ToListAsync();
            return processes;
        }

        public async Task Update(ProcessesDBModel entity)
        {
            var filter = Builders<ProcessesDBModel>.Filter.Eq(p => p.id, entity.id);
                
            var result = await collection.ReplaceOneAsync(filter, entity);
        }
    }
}
