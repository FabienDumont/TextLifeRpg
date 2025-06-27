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
    var textPart1 = new TextPartDataModel {Color = "blue", Text = "Daniel:"};
    var textPart2 = new TextPartDataModel {Color = "white", Text = "Hello, how are you?"};
    var model = new TextLineDataModel();

    // Act
    model.TextParts.Add(textPart1);
    model.TextParts.Add(textPart2);

    // Assert
    Assert.Equal(2, model.TextParts.Count);

    Assert.Equal("blue", model.TextParts[0].Color);
    Assert.Equal("Daniel:", model.TextParts[0].Text);

    Assert.Equal("white", model.TextParts[1].Color);
    Assert.Equal("Hello, how are you?", model.TextParts[1].Text);
  }

  [Fact]
  public void TextLineDataModel_Should_Initialize_TextParts_WithGivenValues()
  {
    // Arrange
    var textPart1 = new TextPartDataModel {Color = "green", Text = "NPC:"};
    var textPart2 = new TextPartDataModel {Color = "yellow", Text = "Greetings!"};

    // Act
    var model = new TextLineDataModel
    {
      TextParts = [textPart1, textPart2]
    };

    // Assert
    Assert.Equal(2, model.TextParts.Count);

    Assert.Equal("green", model.TextParts[0].Color);
    Assert.Equal("NPC:", model.TextParts[0].Text);

    Assert.Equal("yellow", model.TextParts[1].Color);
    Assert.Equal("Greetings!", model.TextParts[1].Text);
  }

  #endregion
}
