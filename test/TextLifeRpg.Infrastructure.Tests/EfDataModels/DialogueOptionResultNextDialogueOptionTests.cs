using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class DialogueOptionResultNextDialogueOptionTests
{
  #region Methods

  [Fact]
  public void ShouldSetPropertiesCorrectly()
  {
    // Arrange
    var relatedDialogueOption = new DialogueOptionDataModel
    {
      Id = Guid.NewGuid(),
      Label = "Ask something", NeededMinutes = 0
    };

    var relatedDialogueOptionResult = new DialogueOptionResultDataModel
    {
      Id = Guid.NewGuid(),
      DialogueOptionId = relatedDialogueOption.Id,
      EndDialogue = false,
      DialogueOption = relatedDialogueOption
    };

    var nextDialogueOption = new DialogueOptionDataModel
    {
      Id = Guid.NewGuid(),
      Label = "Nevermind", NeededMinutes = 0
    };

    var id = Guid.NewGuid();
    var order = 1;

    // Act
    var dataModel = new DialogueOptionResultNextDialogueOption()
    {
      Id = id,
      DialogueOptionResultId = relatedDialogueOptionResult.Id,
      NextDialogueOptionId = nextDialogueOption.Id,
      Order = order,
      DialogueOptionResult = relatedDialogueOptionResult,
      NextDialogueOption = nextDialogueOption
    };

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(relatedDialogueOptionResult.Id, dataModel.DialogueOptionResultId);
    Assert.Equal(nextDialogueOption.Id, dataModel.NextDialogueOptionId);
    Assert.Equal(order, dataModel.Order);
    Assert.Equal(relatedDialogueOptionResult, dataModel.DialogueOptionResult);
    Assert.Equal(nextDialogueOption, dataModel.NextDialogueOption);
  }

  #endregion
}
