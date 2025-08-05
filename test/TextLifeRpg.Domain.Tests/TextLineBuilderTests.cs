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
    var npc = new CharacterBuilder().WithSex(BiologicalSex.Male).Build();
    var world = World.Create(DateTime.Now, [character, npc]);
    var save = GameSave.Create(character, world);
    const string template = "You walk away from [TARGETNAME].";

    // Act
    TextLineBuilder.BuildNarrationLine(template, character, npc, save);

    // Assert
    Assert.Equal(3, save.TextLines[0].TextParts.Count);

    Assert.Equal("You walk away from ", save.TextLines[0].TextParts[0].Text);
    Assert.Null(save.TextLines[0].TextParts[0].Color);

    Assert.Equal(npc.Name, save.TextLines[0].TextParts[1].Text);
    Assert.Equal(CharacterColor.Blue, save.TextLines[0].TextParts[1].Color);

    Assert.Equal(".", save.TextLines[0].TextParts[2].Text);
    Assert.Null(save.TextLines[0].TextParts[2].Color);
  }

  [Fact]
  public void BuildNarrationLine_ShouldThrow_WhenTargetNull()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);
    var save = GameSave.Create(character, world);
    const string template = "You walk away from [TARGETNAME].";

    // Act & Assert
    var ex = Assert.Throws<ArgumentNullException>(() => TextLineBuilder.BuildNarrationLine(template, character, null, save));

    Assert.Equal("target", ex.ParamName);
  }

  [Theory]
  [InlineData(BiologicalSex.Male, CharacterColor.Blue)]
  [InlineData(BiologicalSex.Female, CharacterColor.Pink)]
  [InlineData((BiologicalSex) 99, CharacterColor.Purple)]
  public void BuildNarrationLine_ShouldAssignCorrectColor_BasedOnSex(BiologicalSex sex, CharacterColor expectedColor)
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var npc = new CharacterBuilder().WithBirthDate(new DateOnly(1990, 1, 1)).WithSex(sex).Build();
    var world = World.Create(DateTime.Now, [character, npc]);
    var save = GameSave.Create(character, world);
    const string template = "[TARGETNAME] runs away.";

    // Act
    TextLineBuilder.BuildNarrationLine(template, character, npc, save);

    // Assert
    var part = save.TextLines[0].TextParts.First(p => p.Text == npc.Name);
    Assert.Equal(expectedColor, part.Color);
  }

  [Fact]
  public void BuildNarrationLine_ShouldHandleTemplateWithoutTokens()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);
    var save = GameSave.Create(character, world);
    const string template = "No tokens here.";

    // Act
    TextLineBuilder.BuildNarrationLine(template, character, null, save);

    // Assert
    Assert.Single(save.TextLines[0].TextParts);
    Assert.Equal("No tokens here.", save.TextLines[0].TextParts[0].Text);
    Assert.Null(save.TextLines[0].TextParts[0].Color);
  }

  [Fact]
  public void BuildNarrationLine_ShouldPreserveUnknownTokensAsText()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var world = World.Create(DateTime.Now, [character]);
    var save = GameSave.Create(character, world);
    const string template = "This is [UNKNOWN].";

    // Act
    TextLineBuilder.BuildNarrationLine(template, character, null, save);

    // Assert
    Assert.Equal(3, save.TextLines[0].TextParts.Count);
    Assert.Equal("This is ", save.TextLines[0].TextParts[0].Text);
    Assert.Equal("[UNKNOWN]", save.TextLines[0].TextParts[1].Text);
    Assert.Equal(".", save.TextLines[0].TextParts[2].Text);
  }

  #endregion
}
