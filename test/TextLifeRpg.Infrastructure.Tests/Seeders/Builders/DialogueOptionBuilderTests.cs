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
    var builder = new DialogueOptionBuilder(context, "Say goodbye").EndDialogue()
      .AddSpokenText("Goodbye", c => c.WithActorEnergyCondition(">", "50"))
      .AddResultSpokenText("Alright, goodbye.", c => c.WithActorTraitCondition(Guid.NewGuid())).AddResultNarration(
        "You walk away from [TARGETNAME].", c => c.WithActorEnergyCondition("<", "40")
      );

    // Act
    await builder.BuildAsync();

    // Assert: DialogueOption
    var option = await context.DialogueOptions.SingleAsync();
    Assert.Equal("Say goodbye", option.Label);

    // Assert: Result
    var result = await context.DialogueOptionResults.SingleAsync();
    Assert.Equal(option.Id, result.DialogueOptionId);
    Assert.True(result.EndDialogue);

    // Assert: SpokenText
    var spokenText = await context.DialogueOptionSpokenTexts.SingleAsync();
    Assert.Equal(option.Id, spokenText.DialogueOptionId);
    Assert.Equal("Goodbye", spokenText.Text);

    // Assert: ResultSpokenText
    var resultSpokenText = await context.DialogueOptionResultSpokenTexts.SingleAsync();
    Assert.Equal(result.Id, resultSpokenText.DialogueOptionResultId);
    Assert.Equal("Alright, goodbye.", resultSpokenText.Text);

    // Assert: ResultNarration
    var narration = await context.DialogueOptionResultNarrations.SingleAsync();
    Assert.Equal(result.Id, narration.DialogueOptionResultId);
    Assert.Equal("You walk away from [TARGETNAME].", narration.Text);

    // Assert: Conditions
    var conditionCount = await context.Conditions.CountAsync();
    Assert.Equal(3, conditionCount);
  }

  #endregion
}
