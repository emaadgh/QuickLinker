namespace QuickLinker.API
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = null!;
        public QuickLinkerDomain QuickLinkerDomain { get; set; } = null!;
    }

    public class ConnectionStrings
    {
        public string QuickLinkerDbContextConnection { get; set; } = null!;
        public string QuickLinkerRedisConnection { get; set; } = null!;
    }
    public class QuickLinkerDomain
    {
        public string Domain { get; set; } = null!;
    }
}
