using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

namespace TextLifeRpg.Infrastructure.Tests.JsonDataModels;

public class TextLineDataModelTests
{
  #region Tests

  [Fact]
  public void TextLineDataModel_Should_InitializeWithDefaultValues()
  {
    // Act
    var model = new TextLineDataModel();

    // Assert
    Assert.NotNull(model.TextParts);
    Assert.Empty(model.TextParts);
  }

  [Fact]
  public void TextLineDataModel_Should_Allow_Adding_TextParts()
  {
    // Arrange
    var textPart1 = new TextPartDataModel {Color = CharacterColor.Blue, Text = "Daniel:"};
    var textPart2 = new TextPartDataModel {Color = null, Text = "Hello, how are you?"};
    var model = new TextLineDataModel();

    // Act
    model.TextParts.Add(textPart1);
    model.TextParts.Add(textPart2);

    // Assert
    Assert.Equal(2, model.TextParts.Count);

    Assert.Equal(CharacterColor.Blue, model.TextParts[0].Color);
    Assert.Equal("Daniel:", model.TextParts[0].Text);

    Assert.Equal(null, model.TextParts[1].Color);
    Assert.Equal("Hello, how are you?", model.TextParts[1].Text);
  }

  [Fact]
  public void TextLineDataModel_Should_Initialize_TextParts_WithGivenValues()
  {
    // Arrange
    var textPart1 = new TextPartDataModel {Color = CharacterColor.Blue, Text = "NPC:"};
    var textPart2 = new TextPartDataModel {Color = CharacterColor.Yellow, Text = "Greetings!"};

    // Act
    var model = new TextLineDataModel
    {
      TextParts = [textPart1, textPart2]
    };

    // Assert
    Assert.Equal(2, model.TextParts.Count);

    Assert.Equal(CharacterColor.Blue, model.TextParts[0].Color);
    Assert.Equal("NPC:", model.TextParts[0].Text);

    Assert.Equal(CharacterColor.Yellow, model.TextParts[1].Color);
    Assert.Equal("Greetings!", model.TextParts[1].Text);
  }

  #endregion
}
