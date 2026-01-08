namespace WebDevelopment.Shared.Roles;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Client = "Client";

    public static string Merge(params string[] roles) => string.Join(", ", roles);
}
