namespace RootNamespace.Entities.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string ApplicationName { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }
        public string Admin { get; set; }
        public string Password { get; set; }
    }
}