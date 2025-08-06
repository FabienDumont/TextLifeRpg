using TextLifeRpg.Infrastructure.Seeders.Builders;

namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
/// Dialogue option data seeder.
/// </summary>
public class DialogueOptionSeeder : IDataSeeder
{
  #region Implementation of IDataSeeder

  /// <inheritdoc />
  public async Task SeedAsync(ApplicationContext context)
  {
    await new DialogueOptionBuilder(context, "Say goodbye").EndDialogue().AddSpokenText("Goodbye.", _ => { })
      .AddResultSpokenText("Alright, goodbye.", _ => { })
      .AddResultNarration("You walk away from [TARGETNAME].", _ => { }).BuildAsync();
  }

  #endregion
}
