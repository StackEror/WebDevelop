namespace WebDevelopment.Domain.Entities;

public class File : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[]? Content { get; set; }
}
