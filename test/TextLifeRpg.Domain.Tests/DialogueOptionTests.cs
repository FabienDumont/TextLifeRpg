namespace TextLifeRpg.Domain.Tests;

public class DialogueOptionTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Arrange
    const string label = "Say goodbye";

    // Act
    var dialogueOption = DialogueOption.Create(label);

    // Assert
    Assert.NotNull(dialogueOption);
    Assert.NotEqual(Guid.Empty, dialogueOption.Id);
    Assert.Equal(label, dialogueOption.Label);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string label = "Say goodbye";

    // Act
    var dialogueOption = DialogueOption.Load(id, label);

    // Assert
    Assert.Equal(id, dialogueOption.Id);
    Assert.Equal(label, dialogueOption.Label);
  }

  #endregion
}
