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
  /// Builds a narration line for storytelling purposes, associates it with the acting character and optional target,
  /// and updates the game save with the resulting narration text.
  /// </summary>
  /// <param name="narration">The narration text to process and build into structured text parts.</param>
  /// <param name="actor">The main character associated with the narration.</param>
  /// <param name="target">The optional target character referenced in the narration.</param>
  /// <param name="gameSave">The game save instance where the built narration will be stored.</param>
  public static void BuildNarrationLine(string narration, Character actor, Character? target, GameSave gameSave)
  {
    var parts = new List<TextPart>();
    var lastIndex = 0;

    foreach (Match match in TokenRegex.Matches(narration))
    {
      if (match.Index > lastIndex)
      {
        parts.Add(new TextPart(null, narration.Substring(lastIndex, match.Index - lastIndex)));
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

    if (lastIndex < narration.Length)
    {
      parts.Add(new TextPart(null, narration[lastIndex..]));
    }

    gameSave.AddText(parts);
  }

  /// <summary>
  /// Builds a spoken text line for dialogue purposes, associates it with the acting character, and updates the game save with the resulting text.
  /// </summary>
  /// <param name="spokenText">The text that is being spoken by the actor.</param>
  /// <param name="actor">The character who performs the spoken text.</param>
  /// <param name="target">The character to whom the spoken text is directed.</param>
  /// <param name="gameSave">The current game save where the spoken text will be stored.</param>
  public static void BuildSpokenText(string spokenText, Character actor, Character target, GameSave gameSave)
  {
    var textPartsResultSpokenText = new List<TextPart>(
      [
        new TextPart(CharacterColorHelper.GetColorKey(actor, gameSave.PlayerCharacterId), $"{actor.Name}"),
        new TextPart(null, $" : {spokenText}")
      ]
    );

    gameSave.AddText(textPartsResultSpokenText);
  }

  /// <summary>
  /// Regex factory method to match token patterns in the form [TOKEN].
  /// </summary>
  [GeneratedRegex(@"\[([A-Z]+)\]", RegexOptions.Compiled)]
  private static partial Regex MyRegex();

  #endregion
}
