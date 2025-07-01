using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
/// Item data seeder.
/// </summary>
public class ItemSeeder : IDataSeeder
{
  #region Implementation of IDataSeeder

  /// <inheritdoc />
  public async Task SeedAsync(ApplicationContext context)
  {
    foreach (var name in new[]
             {
               "Student card"
             })
    {
      var item = new ItemDataModel
      {
        Id = Guid.NewGuid(),
        Name = name
      };

      await context.Items.AddAsync(item).ConfigureAwait(false);

      await context.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  #endregion
}
