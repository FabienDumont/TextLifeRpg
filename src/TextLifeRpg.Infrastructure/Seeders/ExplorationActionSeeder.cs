using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.JsonDefinitions;
using TextLifeRpg.Infrastructure.Seeders.Builders;

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
    var baseDir = AppContext.BaseDirectory;
    var fullPath = Path.Combine(baseDir, "Data/ExplorationActions.json");

    await using var stream = File.OpenRead(fullPath);
    var definitions = await JsonSerializer.DeserializeAsync<List<ExplorationActionDefinition>>(stream) ??
                      throw new InvalidOperationException("Failed to parse exploration actions definition file.");

    foreach (var def in definitions)
    {
      var location = await context.Locations.FirstAsync(l => l.Name == def.LocationName);
      var room = await context.Rooms.FirstAsync(r => r.Name == def.RoomName);

      var builder = new ExplorationActionBuilder(context, def.Label, def.NeededMinutes, location.Id, room.Id);

      foreach (var result in def.Results)
      {
        if (result.AddMinutes)
        {
          builder.WithAddMinutes();
        }

        if (result.EnergyChange is not null)
        {
          builder.WithEnergyChange(result.EnergyChange.Value);
        }

        foreach (var narration in result.ResultNarrations)
        {
          builder.AddResultNarration(narration.Text, b => { ApplyConditions(b, narration.Conditions); });
        }
      }

      await builder.BuildAsync();
    }
  }

  #endregion

  #region Methods

  private static void ApplyConditions(TextVariantBuilder b, List<ExplorationActionConditionDefinition> conditions)
  {
    foreach (var condition in conditions)
    {
      if (condition.ActorEnergy is not null)
      {
        b.WithActorEnergyCondition(condition.ActorEnergy.Operator, condition.ActorEnergy.Value.ToString());
      }
    }
  }

  #endregion
}
