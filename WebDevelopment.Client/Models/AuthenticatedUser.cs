namespace WebDevelopment.Client.Models;

public class AuthenticatedUser
{
    public Guid Id { get; }
    public string Email { get; } = string.Empty;
    public string GivenName { get; } = string.Empty;
    public string FamilyName { get; } = string.Empty;
    public bool IsFirstLogin { get; }
    public string[] Roles { get; } = [];

    public AuthenticatedUser(Guid id, string email, string givenName, string familyName, string isFirstLogin, string[] roles)
    {
        Id = id;
        Email = email;
        GivenName = givenName;
        FamilyName = familyName;
        IsFirstLogin = isFirstLogin == "True" ? true : false;
        Roles = roles;
    }
}