namespace TextLifeRpg.Domain;

/// <summary>
/// Class representing a part of text with an optional color.
/// </summary>
public class TextPart
{
  #region Properties

  public string? Color { get; set; }
  public string Text { get; set; }

  #endregion

  #region Ctors

  public TextPart(string? color, string text)
  {
    Color = color;
    Text = text;
  }

  #endregion
}
