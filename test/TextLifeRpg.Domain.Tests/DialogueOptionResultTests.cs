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
    const string learnFact = "Job";
    const string specialAction = "AddTargetPhoneNumber";
    const bool endDialogue = true;

    // Act
    var result = DialogueOptionResult.Create(
      dialogueOptionId, targetRelationshipValueChange, learnFact, specialAction, endDialogue
    );

    // Assert
    Assert.NotNull(result);
    Assert.NotEqual(Guid.Empty, result.Id);
    Assert.Equal(dialogueOptionId, result.DialogueOptionId);
    Assert.Equal(targetRelationshipValueChange, result.TargetRelationshipValueChange);
    Assert.Equal(learnFact, result.ActorLearnFact);
    Assert.Equal(specialAction, result.ActorTargetSpecialAction);
    Assert.Equal(endDialogue, result.EndDialogue);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    var dialogueOptionId = Guid.NewGuid();
    const int targetRelationshipValueChange = 5;
    const string learnFact = "Job";
    const string specialAction = "AddTargetPhoneNumber";
    const bool endDialogue = false;

    // Act
    var result = DialogueOptionResult.Load(
      id, dialogueOptionId, targetRelationshipValueChange, learnFact, specialAction, endDialogue
    );

    // Assert
    Assert.Equal(id, result.Id);
    Assert.Equal(dialogueOptionId, result.DialogueOptionId);
    Assert.Equal(targetRelationshipValueChange, result.TargetRelationshipValueChange);
    Assert.Equal(learnFact, result.ActorLearnFact);
    Assert.Equal(specialAction, result.ActorTargetSpecialAction);
    Assert.Equal(endDialogue, result.EndDialogue);
  }

  #endregion
}
