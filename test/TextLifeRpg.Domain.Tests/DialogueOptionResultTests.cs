namespace TextLifeRpg.Domain.Tests;

public class DialogueOptionResultTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithNewGuid()
  {
    // Arrange
    var dialogueOptionId = Guid.NewGuid();
    const bool endDialogue = true;

    // Act
    var result = DialogueOptionResult.Create(dialogueOptionId, endDialogue);

    // Assert
    Assert.NotNull(result);
    Assert.NotEqual(Guid.Empty, result.Id);
    Assert.Equal(dialogueOptionId, result.DialogueOptionId);
    Assert.Equal(endDialogue, result.EndDialogue);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var dialogueOptionId = Guid.NewGuid();
    const bool endDialogue = false;

    // Act
    var result = DialogueOptionResult.Load(id, dialogueOptionId, endDialogue);

    // Assert
    Assert.Equal(id, result.Id);
    Assert.Equal(dialogueOptionId, result.DialogueOptionId);
    Assert.Equal(endDialogue, result.EndDialogue);
  }

  #endregion
}
