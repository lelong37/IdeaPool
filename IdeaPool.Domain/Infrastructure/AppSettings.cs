namespace IdeaPool.Domain.Infrastructure
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public bool? UseInMemoryDatabase { get; set; }
    }
}