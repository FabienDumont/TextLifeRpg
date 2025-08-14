using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Domain.Tests;

public class PhoneContactTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Arrange
    var character = new CharacterBuilder().Build();

    // Act
    var phoneContact = PhoneContact.Create(character);

    // Assert
    Assert.NotNull(phoneContact);
    Assert.Equal(character, phoneContact.Character);
  }

  #endregion
}
