using TextLifeRpg.Domain;

namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
/// Data model for a single part of text with its associated color.
/// </summary>
public class TextPartDataModel
{
  #region Properties

  /// <summary>
  /// The color of the text. (null means no color specified)
  /// </summary>
  public CharacterColor? Color { get; set; }

  /// <summary>
  /// The actual text content.
  /// </summary>
  public string Text { get; set; } = string.Empty;

  #endregion
}
