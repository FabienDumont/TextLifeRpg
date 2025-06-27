using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class NarrationDataModelTests
{
  #region Methods

  [Fact]
  public void Instantiation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string key = "intro_scene";
    const string text = "You wake up in a strange place.";

    // Act
    var narration = new NarrationDataModel
    {
      Id = id,
      Key = key,
      Text = text
    };

    // Assert
    Assert.Equal(id, narration.Id);
    Assert.Equal(key, narration.Key);
    Assert.Equal(text, narration.Text);
  }

  #endregion
}
