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

  public static void BuildNarrationLine(string template, Character actor, Character? target, GameSave gameSave)
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
          "[TARGETNAME]" => new TextPart(
            CharacterColorHelper.GetColorKey(
              target ?? throw new ArgumentNullException(nameof(target)), gameSave.PlayerCharacterId
            ), target.Name
          ),
          _ => new TextPart(null, match.Value)
        }
      );

      lastIndex = match.Index + match.Length;
    }

    if (lastIndex < template.Length)
    {
      parts.Add(new TextPart(null, template[lastIndex..]));
    }

    gameSave.AddText(parts);
  }

  /// <summary>
  /// Regex factory method to match token patterns in the form [TOKEN].
  /// </summary>
  [GeneratedRegex(@"\[([A-Z]+)\]", RegexOptions.Compiled)]
  private static partial Regex MyRegex();

  #endregion
}
