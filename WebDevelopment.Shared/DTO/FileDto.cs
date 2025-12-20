namespace WebDevelopment.Shared.DTO;

public class FileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[]? Content { get; set; }
}
