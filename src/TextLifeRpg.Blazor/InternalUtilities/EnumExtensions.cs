using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TextLifeRpg.Blazor.InternalUtilities;

/// <summary>
/// Provides extension methods for operations related to enumeration types.
/// </summary>
public static class EnumExtensions
{
  /// <summary>
  /// Retrieves the display name of an enumeration value. If a <see cref="DisplayAttribute"/> with a Name property is present,
  /// its value is returned. Otherwise, the default string representation of the enumeration value is returned.
  /// </summary>
  /// <param name="value">The enumeration value for which to retrieve the display name.</param>
  /// <returns>The display name of the enumeration value or its default string representation if no display attribute is found.</returns>
  public static string GetDisplayName(this Enum value)
  {
    return value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.Name ??
           value.ToString();
  }
}
