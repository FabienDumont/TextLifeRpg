namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
///   Interface for data seeders.
/// </summary>
public interface IDataSeeder
{
  #region Methods

  /// <summary>
  ///   Seeds data into the context.
  /// </summary>
  Task SeedAsync(ApplicationContext context);

  #endregion
}
