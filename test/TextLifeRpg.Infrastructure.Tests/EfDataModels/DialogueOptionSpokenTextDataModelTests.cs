using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class DialogueOptionSpokenTextDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var spokenTextId = Guid.NewGuid();
    var optionId = Guid.NewGuid();
    const string text = "Goodbye.";

    var dialogueOption = new DialogueOptionDataModel
    {
      Id = optionId,
      Label = "Say goodbye", NeededMinutes = 0
    };

    // Act
    var spokenText = new DialogueOptionSpokenTextDataModel
    {
      Id = spokenTextId,
      DialogueOptionId = optionId,
      Text = text,
      DialogueOption = dialogueOption
    };

    // Assert
    Assert.Equal(spokenTextId, spokenText.Id);
    Assert.Equal(optionId, spokenText.DialogueOptionId);
    Assert.Equal(text, spokenText.Text);
    Assert.Equal(dialogueOption, spokenText.DialogueOption);
  }

  #endregion
}
