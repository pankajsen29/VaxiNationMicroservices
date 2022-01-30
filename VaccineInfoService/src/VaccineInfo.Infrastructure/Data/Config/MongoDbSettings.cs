namespace VaccineInfo.Infrastructure.Data.Config
{
    public class MongoDbSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"mongodb://{User}:{Password}@{Host}:{Port}"; //here the password is read from the secrets.json file (.net secret manager)
            }
        }
    }
}
