namespace TextLifeRpg.Domain.Tests;

public class DialogueOptionResultTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitializeWithNewGuid()
  {
    // Arrange
    var dialogueOptionId = Guid.NewGuid();
    const int targetRelationshipValueChange = 5;
    const bool endDialogue = true;

    // Act
    var result = DialogueOptionResult.Create(dialogueOptionId, targetRelationshipValueChange, endDialogue);

    // Assert
    Assert.NotNull(result);
    Assert.NotEqual(Guid.Empty, result.Id);
    Assert.Equal(dialogueOptionId, result.DialogueOptionId);
    Assert.Equal(targetRelationshipValueChange, result.TargetRelationshipValueChange);
    Assert.Equal(endDialogue, result.EndDialogue);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var dialogueOptionId = Guid.NewGuid();
    const int targetRelationshipValueChange = 5;
    const bool endDialogue = false;

    // Act
    var result = DialogueOptionResult.Load(id, dialogueOptionId, targetRelationshipValueChange, endDialogue);

    // Assert
    Assert.Equal(id, result.Id);
    Assert.Equal(dialogueOptionId, result.DialogueOptionId);
    Assert.Equal(targetRelationshipValueChange, result.TargetRelationshipValueChange);
    Assert.Equal(endDialogue, result.EndDialogue);
  }

  #endregion
}
