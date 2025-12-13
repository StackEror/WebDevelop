namespace WebDevelopment.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }
        public string? Nationality { get; set; }
    }
}
