namespace WebDevelopment.Shared.Interfaces.Authentication;

public interface IUserContext
{
    Guid? GetId();
    IEnumerable<string> GetRoles();
}
