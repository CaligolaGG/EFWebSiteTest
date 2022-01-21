namespace EFWebSiteTest
{
    public class User:EntityBase
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public Account Account { get; set; }
    }
}
