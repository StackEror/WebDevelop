using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WebDevelopment.Client.Extensions;

public static class EnumExtensions
{
    /*public static string GetDisplayName<T>(T? enumValue) where T : Enum?*/
    public static string GetDisplayName(this Enum? enumValue)
    {
        if (enumValue == null)
            return "Enum value is null";

        var member = enumValue.GetType().GetMember(enumValue.ToString()).First();
        var display = member.GetCustomAttribute<DisplayAttribute>();

        return display?.GetName() ?? enumValue.ToString();
    }
}
