namespace TextLifeRpg.Domain;

/// <summary>
///   Class representing a single line of text with multiple parts, each part having its own color.
/// </summary>
public class TextLine
{
  #region Properties

  /// <summary>
  ///   List of text parts.
  /// </summary>
  public List<TextPart> TextParts { get; }

  #endregion

  #region Ctors

  public TextLine(List<TextPart> textParts)
  {
    TextParts = textParts;
  }

  #endregion

  #region Methods

  /// <summary>
  ///   Adds a new part of text with a color.
  /// </summary>
  /// <param name="text">The text to be added.</param>
  /// <param name="color">The color to be applied to the text.</param>
  public void AddTextPart(string text, string? color = null)
  {
    TextParts.Add(new TextPart(color, text));
  }

  #endregion
}
