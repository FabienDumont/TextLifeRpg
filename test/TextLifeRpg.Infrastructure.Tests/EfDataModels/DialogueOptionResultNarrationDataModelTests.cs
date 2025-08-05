using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class DialogueOptionResultNarrationDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var narrationId = Guid.NewGuid();
    var resultId = Guid.NewGuid();
    const string text = "You are too tired to think straight.";

    var result = new DialogueOptionResultDataModel
    {
      Id = resultId,
      DialogueOptionId = Guid.NewGuid(),
      EndDialogue = false
    };

    // Act
    var narration = new DialogueOptionResultNarrationDataModel
    {
      Id = narrationId,
      DialogueOptionResultId = resultId,
      Text = text,
      DialogueOptionResult = result
    };

    // Assert
    Assert.Equal(narrationId, narration.Id);
    Assert.Equal(resultId, narration.DialogueOptionResultId);
    Assert.Equal(text, narration.Text);
    Assert.Equal(result, narration.DialogueOptionResult);
  }

  #endregion
}
