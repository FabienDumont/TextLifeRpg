using System.Text.RegularExpressions;

namespace TextLifeRpg.Domain;

/// <summary>
/// Builds dynamic text lines for narrative purposes, supporting token replacement (e.g. [CHARACTERNAME]).
/// </summary>
public static partial class TextLineBuilder
{
  #region Fields

  /// <summary>
  /// Compiled regex to match tokens like [CHARACTERNAME] in the template.
  /// </summary>
  private static readonly Regex TokenRegex = MyRegex();

  #endregion

  #region Methods

  /// <summary>
  /// Builds a <see cref="TextLine" /> from a template string by replacing tokens with character-specific data.
  /// </summary>
  /// <param name="template">The text containing tokens like [CHARACTERNAME].</param>
  /// <param name="character">The character to use for token replacement.</param>
  /// <param name="playerCharacterId">The identifier of the player character.</param>
  /// <returns>A constructed <see cref="TextLine" /> with formatted parts.</returns>
  public static TextLine BuildNarrationLine(string template, Character character, Guid playerCharacterId)
  {
    var parts = new List<TextPart>();
    var lastIndex = 0;

    foreach (Match match in TokenRegex.Matches(template))
    {
      if (match.Index > lastIndex)
      {
        parts.Add(new TextPart(null, template.Substring(lastIndex, match.Index - lastIndex)));
      }

      parts.Add(
        match.Value switch
        {
          "[CHARACTERNAME]" => new TextPart(CharacterColorHelper.GetCharacterColor(character, playerCharacterId), character.Name),
          _ => new TextPart(null, match.Value)
        }
      );

      lastIndex = match.Index + match.Length;
    }

    if (lastIndex < template.Length)
    {
      parts.Add(new TextPart(null, template[lastIndex..]));
    }

    return new TextLine(parts);
  }

  /// <summary>
  /// Regex factory method to match token patterns in the form [TOKEN].
  /// </summary>
  [GeneratedRegex(@"\[([A-Z]+)\]", RegexOptions.Compiled)]
  private static partial Regex MyRegex();

  #endregion
}
