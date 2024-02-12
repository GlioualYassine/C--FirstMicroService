namespace Common.Settings
{
    public class MongoDbSettings
    {
        public string host { get; init; }
        public int port { get; init; }
        public string ConnectionString => $"mongodb://{host}:{port}";

    }
}
