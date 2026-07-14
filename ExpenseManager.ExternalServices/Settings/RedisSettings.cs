namespace ExpenseManager.ExternalServices.Settings
{
    public class RedisSettings
    {
        public bool Enabled { get; set; }
        
        public string ConnectionString { get; set; } = "localhost:6379";
    }
}
