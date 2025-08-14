using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.Seeders.Builders;

namespace TextLifeRpg.Infrastructure.Tests.Seeders.Builders;

public class DialogueOptionBuilderTests
{
  #region Methods

  [Fact]
  public async Task BuildAsync_ShouldSaveDialogueOption_WithAllComponents()
  {
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    var context = new ApplicationContext(options);
    var builder = new DialogueOptionBuilder(context, Guid.NewGuid(), "Say goodbye", 0).AddSpokenText(
      "Goodbye", c => c.WithActorEnergyCondition(">", "50")
    );

    builder.AddResult();

    // Act
    await builder.BuildAsync();

    // Assert: DialogueOption
    var option = await context.DialogueOptions.SingleAsync();
    Assert.Equal("Say goodbye", option.Label);

    // Assert: Result
    var result = await context.DialogueOptionResults.SingleAsync();
    Assert.Equal(option.Id, result.DialogueOptionId);

    // Assert: SpokenText
    var spokenText = await context.DialogueOptionSpokenTexts.SingleAsync();
    Assert.Equal(option.Id, spokenText.DialogueOptionId);
    Assert.Equal("Goodbye", spokenText.Text);

    // Assert: Conditions
    var conditionCount = await context.Conditions.CountAsync();
    Assert.Equal(1, conditionCount);
  }

  #endregion
}
