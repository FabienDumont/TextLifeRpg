using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Domain.Tests;

public class TextLineBuilderTests
{
  #region Methods

  [Fact]
  public void BuildNarrationLine_ShouldReplacePlayerNameToken_WithCharacterName()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    const string template = "Hello [CHARACTERNAME], welcome back.";

    // Act
    var line = TextLineBuilder.BuildNarrationLine(template, character, character.Id);

    // Assert
    Assert.Equal(3, line.TextParts.Count);

    Assert.Equal("Hello ", line.TextParts[0].Text);
    Assert.Null(line.TextParts[0].Color);

    Assert.Equal(character.Name, line.TextParts[1].Text);
    Assert.Equal(CharacterColor.Yellow, line.TextParts[1].Color);

    Assert.Equal(", welcome back.", line.TextParts[2].Text);
    Assert.Null(line.TextParts[2].Color);
  }

  [Theory]
  [InlineData(BiologicalSex.Male, CharacterColor.Blue)]
  [InlineData(BiologicalSex.Female, CharacterColor.Pink)]
  [InlineData((BiologicalSex) 99, CharacterColor.Purple)]
  public void BuildNarrationLine_ShouldAssignCorrectColor_BasedOnSex(BiologicalSex sex, CharacterColor expectedColor)
  {
    // Arrange
    var character = new CharacterBuilder().WithBirthDate(new DateOnly(1990, 1, 1)).WithSex(sex).Build();
    const string template = "[CHARACTERNAME] enters the room.";

    // Act
    var line = TextLineBuilder.BuildNarrationLine(template, character, Guid.Empty);

    // Assert
    var part = line.TextParts.First(p => p.Text == character.Name);
    Assert.Equal(expectedColor, part.Color);
  }

  [Fact]
  public void BuildNarrationLine_ShouldHandleTemplateWithoutTokens()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    const string template = "No tokens here.";

    // Act
    var line = TextLineBuilder.BuildNarrationLine(template, character, Guid.Empty);

    // Assert
    Assert.Single(line.TextParts);
    Assert.Equal("No tokens here.", line.TextParts[0].Text);
    Assert.Null(line.TextParts[0].Color);
  }

  [Fact]
  public void BuildNarrationLine_ShouldPreserveUnknownTokensAsText()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    const string template = "This is [UNKNOWN].";

    // Act
    var line = TextLineBuilder.BuildNarrationLine(template, character, Guid.Empty);

    // Assert
    Assert.Equal(3, line.TextParts.Count);
    Assert.Equal("This is ", line.TextParts[0].Text);
    Assert.Equal("[UNKNOWN]", line.TextParts[1].Text);
    Assert.Equal(".", line.TextParts[2].Text);
  }

  #endregion
}
