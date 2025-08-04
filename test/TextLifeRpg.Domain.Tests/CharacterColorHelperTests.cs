using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Domain.Tests;

public class CharacterColorHelperTests
{
  #region Methods

  [Fact]
  public void GetColorKey_ReturnsYellow_WhenCharacterIsPlayer()
  {
    // Arrange
    var character = new CharacterBuilder().WithSex(BiologicalSex.Male).Build();

    // Act
    var result = CharacterColorHelper.GetColorKey(character, character.Id);

    // Assert
    Assert.Equal(CharacterColor.Yellow, result);
  }

  [Fact]
  public void GetColorKey_ReturnsBlue_WhenCharacterIsMale()
  {
    // Arrange
    var character = new CharacterBuilder().WithSex(BiologicalSex.Male).Build();

    // Act
    var result = CharacterColorHelper.GetColorKey(character, Guid.NewGuid());

    // Assert
    Assert.Equal(CharacterColor.Blue, result);
  }

  [Fact]
  public void GetColorKey_ReturnsPink_WhenCharacterIsFemale()
  {
    // Arrange
    var character = new CharacterBuilder().WithSex(BiologicalSex.Female).Build();

    // Act
    var result = CharacterColorHelper.GetColorKey(character, Guid.NewGuid());

    // Assert
    Assert.Equal(CharacterColor.Pink, result);
  }

  [Fact]
  public void GetColorKey_ReturnsPurple_WhenCharacterHasOtherSex()
  {
    // Arrange
    var character = new CharacterBuilder().WithSex((BiologicalSex)999).Build();

    // Act
    var result = CharacterColorHelper.GetColorKey(character, Guid.NewGuid());

    // Assert
    Assert.Equal(CharacterColor.Purple, result);
  }

  #endregion
}
