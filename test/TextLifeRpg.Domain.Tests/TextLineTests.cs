using TextLifeRpg.Domain;

namespace TextLifeRpg.Domain.Tests;

public class TextLineTests
{
  #region Methods

  [Fact]
  public void Constructor_ShouldInitializeWithTextParts()
  {
    // Arrange
    var initialParts = new List<TextPart>
    {
      new("red", "Hello"),
      new("green", " world")
    };

    // Act
    var textLine = new TextLine(initialParts);

    // Assert
    Assert.Equal(initialParts.Count, textLine.TextParts.Count);
    for (var i = 0; i < initialParts.Count; i++)
    {
      Assert.Equal(initialParts[i].Color, textLine.TextParts[i].Color);
      Assert.Equal(initialParts[i].Text, textLine.TextParts[i].Text);
    }
  }

  [Fact]
  public void AddTextPart_ShouldAddPartWithSpecifiedColor()
  {
    // Arrange
    var textLine = new TextLine(new List<TextPart>());

    // Act
    textLine.AddTextPart("Hello", "blue");

    // Assert
    Assert.Single(textLine.TextParts);
    Assert.Equal("Hello", textLine.TextParts[0].Text);
    Assert.Equal("blue", textLine.TextParts[0].Color);
  }

  [Fact]
  public void AddTextPart_ShouldAddPartWithNullColor()
  {
    // Arrange
    var textLine = new TextLine(new List<TextPart>());

    // Act
    textLine.AddTextPart("Narration");

    // Assert
    Assert.Single(textLine.TextParts);
    Assert.Equal("Narration", textLine.TextParts[0].Text);
    Assert.Null(textLine.TextParts[0].Color);
  }

  #endregion
}
