using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
/// Trait data seeder.
/// </summary>
public class TraitSeeder : IDataSeeder
{
  #region Implementation of IDataSeeder

  /// <inheritdoc />
  public async Task SeedAsync(ApplicationContext context)
  {
    var traits = new Dictionary<string, TraitDataModel>();

    foreach (var name in new[]
             {
               "Blunt", "Kind", "Generous", "Mean", "Outgoing",
               "Polite", "Rude", "Selfish", "Shy"
             })
    {
      var trait = new TraitDataModel
      {
        Id = Guid.NewGuid(),
        Name = name
      };

      await context.Traits.AddAsync(trait).ConfigureAwait(false);
      traits[name] = trait;

      await context.SaveChangesAsync().ConfigureAwait(false);
    }

    var incompatiblePairs = new (string Trait, string Incompatible)[]
    {
      ("Blunt", "Polite"),
      ("Blunt", "Shy"),
      ("Kind", "Mean"),
      ("Outgoing", "Shy"),
      ("Generous", "Selfish"),
      ("Polite", "Rude")
    };

    var incompatibilities = incompatiblePairs.Select(pair => new TraitIncompatibilityDataModel
      {
        Trait1Id = traits[pair.Trait].Id,
        Trait2Id = traits[pair.Incompatible].Id
      }
    );

    await context.TraitIncompatibilities.AddRangeAsync(incompatibilities).ConfigureAwait(false);
    await context.SaveChangesAsync().ConfigureAwait(false);
  }

  #endregion
}
