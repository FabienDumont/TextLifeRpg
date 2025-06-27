using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
/// Narration data seeder.
/// </summary>
public class NarrationSeeder : IDataSeeder
{
  #region Implementation of IDataSeeder

  /// <inheritdoc />
  public async Task SeedAsync(ApplicationContext context)
  {
    await context.Narrations.AddAsync(
      new NarrationDataModel
      {
        Id = Guid.NewGuid(),
        Key = "GameIntro",
        Text =
          "You wake up in your bedroom. The faint hum of the city leaks through the closed windows, mixing with the ticking of a wall clock. Nothing feels out of place—yet, something nags at the edge of your mind. Today’s the day things change."
      }
    );
    await context.SaveChangesAsync();
  }

  #endregion
}
