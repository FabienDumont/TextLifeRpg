using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class DialogueOptionResultSpokenTextDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var spokenTextId = Guid.NewGuid();
    var resultId = Guid.NewGuid();
    const string text = "Hey!";

    var result = new DialogueOptionResultDataModel
    {
      Id = resultId,
      DialogueOptionId = Guid.NewGuid(),
      EndDialogue = false
    };

    // Act
    var spokenText = new DialogueOptionResultSpokenTextDataModel
    {
      Id = spokenTextId,
      DialogueOptionResultId = resultId,
      Text = text,
      DialogueOptionResult = result
    };

    // Assert
    Assert.Equal(spokenTextId, spokenText.Id);
    Assert.Equal(resultId, spokenText.DialogueOptionResultId);
    Assert.Equal(text, spokenText.Text);
    Assert.Equal(result, spokenText.DialogueOptionResult);
  }

  [Fact]
  public void Instantiation_ShouldAllowNullBoundsAndResult()
  {
    // Arrange
    var spokenText = new DialogueOptionResultSpokenTextDataModel
    {
      Id = Guid.NewGuid(),
      DialogueOptionResultId = Guid.NewGuid(),
      Text = "Hey!",
      DialogueOptionResult = null
    };

    // Act & Assert
    Assert.Null(spokenText.DialogueOptionResult);
  }

  #endregion
}
