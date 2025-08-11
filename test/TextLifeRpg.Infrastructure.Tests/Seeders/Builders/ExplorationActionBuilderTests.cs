using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Seeders.Builders;

namespace TextLifeRpg.Infrastructure.Tests.Seeders.Builders;

public class ExplorationActionBuilderTests
{
  #region Methods

  [Fact]
  public async Task BuildAsync_ShouldSaveExplorationAction_WithAllComponents()
  {
    // Arrange
    var options = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;

    var context = new ApplicationContext(options);

    var locationId = Guid.NewGuid();
    var roomId = Guid.NewGuid();

    // Seed location and room (required FK)
    context.Locations.Add(new LocationDataModel {Id = locationId, Name = "Test Location"});
    context.Rooms.Add(new RoomDataModel {Id = roomId, Name = "Test Room", LocationId = locationId});
    await context.SaveChangesAsync();

    var builder = new ExplorationActionBuilder(context, "Nap", 60, locationId, roomId).WithAddMinutes()
      .WithEnergyChange(10).WithMoneyChange(10).AddResultNarration(
        "You nap lightly.", c => c.WithActorEnergyCondition("<", "60")
      );

    // Act
    await builder.BuildAsync();

    // Assert: Action
    var action = await context.ExplorationActions.SingleOrDefaultAsync();
    Assert.NotNull(action);
    Assert.Equal("Nap", action.Label);
    Assert.Equal(60, action.NeededMinutes);

    // Assert: Result
    var result = await context.ExplorationActionResults.SingleOrDefaultAsync();
    Assert.NotNull(result);
    Assert.Equal(action.Id, result.ExplorationActionId);
    Assert.Equal(10, result.EnergyChange);
    Assert.Equal(10, result.MoneyChange);
    Assert.True(result.AddMinutes);

    // Assert: Narration
    var narration = await context.ExplorationActionResultNarrations.SingleOrDefaultAsync();
    Assert.NotNull(narration);
    Assert.Equal(result.Id, narration.ExplorationActionResultId);
    Assert.Equal("You nap lightly.", narration.Text);

    // Assert: Condition
    var condition = await context.Conditions.SingleOrDefaultAsync();
    Assert.NotNull(condition);
    Assert.Equal(narration.Id, condition.ContextId);
    Assert.Equal(ContextType.ExplorationActionResultNarration, condition.ContextType);
  }

  #endregion
}
