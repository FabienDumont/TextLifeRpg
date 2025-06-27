namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
///   Data model representing a line of text with optional color formatting.
/// </summary>
public class TextLineDataModel
{
  #region Properties

  /// <summary>
  ///   List of text parts, each part can have a specific color.
  /// </summary>
  public List<TextPartDataModel> TextParts { get; init; } = new();

  #endregion
}
