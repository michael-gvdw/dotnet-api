namespace dotnet_api.Settings
{
    public class MongoDBSettings
    {
        public string host { get; set;}
        public int port { get; set; }
        public string connectionString 
        {
            get
            {
                return $"mongodb://{host}:{port}";
            }
        }
    }
}