namespace WebDevelopment.Domain.Entities;
public class BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ?Description { get; set; }
}
