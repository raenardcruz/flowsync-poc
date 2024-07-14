public class ConfigModel
{
    public string secret { get; set; }
    public MongoDBConfigModel mongoDB { get; set; }
}
public class MongoDBConfigModel
{
    public string connectionString { get; set; }
    public string databaseName { get; set; }
    public string processCollectionName { get; set; }
}