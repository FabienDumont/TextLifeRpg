using Microsoft.EntityFrameworkCore;
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
    var home = await context.Locations.FirstAsync(l => l.Name == "Home");
    var bedroom = await context.Rooms.FirstAsync(r => r.Name == "Bedroom");

    await new ExplorationActionBuilder(context, "Sleep (10 hours)", 480, home.Id, bedroom.Id).WithEnergyChange(100)
      .AddNarration(
        "You collapse into bed, too tired to even pull the sheets over yourself.",
        b => b.WithActorEnergyCondition("<=", "25")
      ).AddNarration(
        "You ease into bed, your body grateful for the rest.",
        b => b.WithActorEnergyCondition(">", "25").WithActorEnergyCondition("<=", "50")
      ).AddNarration(
        "You're not very tired, but you lay down anyway, hoping to fall asleep.", b => b.WithActorEnergyCondition(">", "50")
      ).BuildAsync();

    await new ExplorationActionBuilder(context, "Nap (1 hour)", 60, home.Id, bedroom.Id).WithEnergyChange(10)
      .AddNarration(
        "You sink into the mattress and quickly doze off, too drained to think.", b => b.WithActorEnergyCondition("<=", "25")
      ).AddNarration(
        "You rest your head and fall asleep faster than expected. It's brief but helpful.",
        b => b.WithActorEnergyCondition(">", "25").WithActorEnergyCondition("<=", "50")
      ).AddNarration(
        "You lie back and close your eyes, but your thoughts keep drifting. You barely nap at all.",
        b => b.WithActorEnergyCondition(">", "50")
      ).BuildAsync();
  }

  #endregion
}
