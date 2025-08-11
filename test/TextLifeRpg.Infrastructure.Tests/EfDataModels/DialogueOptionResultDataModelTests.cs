using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class DialogueOptionResultDataModelTests
{
  #region Methods

  [Fact]
  public void ShouldSetPropertiesCorrectly()
  {
    // Arrange
    var id = Guid.NewGuid();
    var dialogueOptionId = Guid.NewGuid();
    const bool endDialogue = true;

    var relatedDialogueOption = new DialogueOptionDataModel
    {
      Id = dialogueOptionId,
      Label = "Say goodbye"
    };

    // Act
    var model = new DialogueOptionResultDataModel
    {
      Id = id,
      DialogueOptionId = dialogueOptionId,
      EndDialogue = endDialogue,
      DialogueOption = relatedDialogueOption
    };

    // Assert
    Assert.Equal(id, model.Id);
    Assert.Equal(dialogueOptionId, model.DialogueOptionId);
    Assert.Equal(endDialogue, model.EndDialogue);
    Assert.Equal(relatedDialogueOption, model.DialogueOption);
  }

  #endregion
}
