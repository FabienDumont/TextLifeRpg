using TextLifeRpg.Domain;

namespace TextLifeRpg.Blazor.InternalUtilities;

public static class CharacterColorExtensions
{
  public static string ToTailwindClass(this CharacterColor color) =>
    color switch
    {
      CharacterColor.Yellow => "text-yellow-400",
      CharacterColor.Blue => "text-blue-400",
      CharacterColor.Pink => "text-pink-400",
      CharacterColor.Purple => "text-purple-400",
      _ => ""
    };
}
