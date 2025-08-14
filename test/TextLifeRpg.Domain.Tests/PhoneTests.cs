using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Domain.Tests;

public class PhoneTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Arrange & Act
    var phone = Phone.Create();

    // Assert
    Assert.NotNull(phone);
    Assert.Empty(phone.Contacts);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var character = new CharacterBuilder().Build();

    // Act
    var phone = Phone.Load([character]);

    // Assert
    Assert.NotNull(phone);
    Assert.Single(phone.Contacts);
  }

  [Fact]
  public void AddToContacts_ShouldWorkAsExpected()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var phone = Phone.Create();

    // Act
    phone.AddToContacts(character);

    // Assert
    Assert.Single(phone.Contacts);
  }

  [Fact]
  public void RemoveFromContacts_ShouldWorkAsExpected()
  {
    // Arrange
    var character = new CharacterBuilder().Build();
    var phone = Phone.Load([character]);

    // Act
    phone.RemoveFromContacts(character.Id);

    // Assert
    Assert.Empty(phone.Contacts);
  }

  #endregion
}
