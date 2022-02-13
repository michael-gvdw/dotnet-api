namespace dotnet_api.Settings
{
    public class MongoDBSettings
    {
        public string host { get; set;}
        public int port { get; set; }
        public string user { get; init; }
        public string password { get; init; }
        public string connectionString 
        {
            get
            {
                return $"mongodb://{user}:{password}@{host}:{port}";
            }
        }
    }
}