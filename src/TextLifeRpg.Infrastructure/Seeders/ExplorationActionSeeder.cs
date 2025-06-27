using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;

namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
/// Exploration action data seeder.
/// </summary>
public class ExplorationActionSeeder : IDataSeeder
{
  #region Implementation of IDataSeeder

  /// <inheritdoc />
  public async Task SeedAsync(ApplicationContext context)
  {
    var home = await context.Locations.FirstAsync(l => l.Name == "Home");
    var bedroom = await context.Rooms.FirstAsync(r => r.Name == "Bedroom");

    var sleepAction = new ExplorationActionDataModel
    {
      Id = Guid.NewGuid(),
      LocationId = home.Id,
      RoomId = bedroom.Id,
      Label = "Sleep (10 hours)",
      NeededMinutes = 480
    };

    var napAction = new ExplorationActionDataModel
    {
      Id = Guid.NewGuid(),
      LocationId = home.Id,
      RoomId = bedroom.Id,
      Label = "Nap (1 hour)",
      NeededMinutes = 60
    };

    await context.ExplorationActions.AddRangeAsync(sleepAction, napAction);
    await context.SaveChangesAsync().ConfigureAwait(false);

    var sleepResultId = Guid.NewGuid();

    await context.ExplorationActionResults.AddAsync(
      new ExplorationActionResultDataModel
      {
        Id = sleepResultId,
        ExplorationActionId = sleepAction.Id,
        AddMinutes = true,
        EnergyChange = 100
      }
    );

    var exhausted = Guid.NewGuid();
    var tired = Guid.NewGuid();
    var fine = Guid.NewGuid();

    await context.ExplorationActionResultNarrations.AddRangeAsync(
      new ExplorationActionResultNarrationDataModel
      {
        Id = exhausted,
        ExplorationActionResultId = sleepResultId,
        Text = "You collapse into bed, too tired to even pull the sheets over yourself."
      }, new ExplorationActionResultNarrationDataModel
      {
        Id = tired,
        ExplorationActionResultId = sleepResultId,
        Text = "You ease into bed, your body grateful for the rest."
      }, new ExplorationActionResultNarrationDataModel
      {
        Id = fine,
        ExplorationActionResultId = sleepResultId,
        Text = "You're not very tired, but you lay down anyway, hoping to fall asleep."
      }
    );

    await context.Conditions.AddRangeAsync(
      new[]
      {
        ConditionBuilder.BuildEnergyConditions(ContextType.ExplorationActionResult, exhausted, [("<=", "25")]),
        ConditionBuilder.BuildEnergyConditions(ContextType.ExplorationActionResult, tired, [(">", "25"), ("<=", "50")]),
        ConditionBuilder.BuildEnergyConditions(ContextType.ExplorationActionResult, fine, [(">", "50")])
      }.SelectMany(x => x)
    );

    var napResultId = Guid.NewGuid();

    await context.ExplorationActionResults.AddAsync(
      new ExplorationActionResultDataModel
      {
        Id = napResultId,
        ExplorationActionId = napAction.Id,
        AddMinutes = true,
        EnergyChange = 10
      }
    );

    var napExhausted = Guid.NewGuid();
    var napTired = Guid.NewGuid();
    var napFine = Guid.NewGuid();

    await context.ExplorationActionResultNarrations.AddRangeAsync(
      new ExplorationActionResultNarrationDataModel
      {
        Id = napExhausted,
        ExplorationActionResultId = napResultId,
        Text = "You sink into the mattress and quickly doze off, too drained to think."
      }, new ExplorationActionResultNarrationDataModel
      {
        Id = napTired,
        ExplorationActionResultId = napResultId,
        Text = "You rest your head and fall asleep faster than expected. It's brief but helpful."
      }, new ExplorationActionResultNarrationDataModel
      {
        Id = napFine,
        ExplorationActionResultId = napResultId,
        Text = "You lie back and close your eyes, but your thoughts keep drifting. You barely nap at all."
      }
    );

    await context.Conditions.AddRangeAsync(
      new[]
      {
        ConditionBuilder.BuildEnergyConditions(ContextType.ExplorationActionResult, napExhausted, [("<=", "25")]),
        ConditionBuilder.BuildEnergyConditions(
          ContextType.ExplorationActionResult, napTired, [(">", "25"), ("<=", "50")]
        ),
        ConditionBuilder.BuildEnergyConditions(ContextType.ExplorationActionResult, napFine, [(">", "50")])
      }.SelectMany(x => x)
    );

    await context.SaveChangesAsync().ConfigureAwait(false);
  }

  #endregion
}
