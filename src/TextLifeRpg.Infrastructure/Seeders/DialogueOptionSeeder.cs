using TextLifeRpg.Infrastructure.EfDataModels;

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
    var dialogueOptions = new List<DialogueOptionDataModel>();

    var goodbyeDialogueOption = new DialogueOptionDataModel
    {
      Id = Guid.NewGuid(), Label = "Say goodbye"
    };

    dialogueOptions.Add(goodbyeDialogueOption);

    await context.DialogueOptions.AddRangeAsync(dialogueOptions).ConfigureAwait(false);
    await context.SaveChangesAsync().ConfigureAwait(false);

    var goodbyeSpokenText = new DialogueOptionSpokenTextDataModel
    {
      Id = Guid.NewGuid(),
      DialogueOptionId = goodbyeDialogueOption.Id,
      Text = "Goodbye."
    };

    await context.DialogueOptionSpokenTexts.AddRangeAsync(goodbyeSpokenText);

    var goodbyeResultId = Guid.NewGuid();

    await context.DialogueOptionResults.AddAsync(
      new DialogueOptionResultDataModel
      {
        Id = goodbyeResultId,
        DialogueOptionId = goodbyeDialogueOption.Id,
        EndDialogue = true
      }
    );

    await context.SaveChangesAsync().ConfigureAwait(false);

    var goodbyeResultSpokenText = new DialogueOptionResultSpokenTextDataModel
    {
      Id = Guid.NewGuid(),
      DialogueOptionResultId = goodbyeResultId,
      Text = "Alright, goodbye."
    };

    await context.DialogueOptionResultSpokenTexts.AddRangeAsync(goodbyeResultSpokenText);

    var goodbyeResultNarration = new DialogueOptionResultNarrationDataModel
    {
      Id = Guid.NewGuid(),
      DialogueOptionResultId = goodbyeResultId,
      Text = "You walk away from [TARGETNAME]."
    };

    await context.DialogueOptionResultNarrations.AddRangeAsync(goodbyeResultNarration);

    await context.SaveChangesAsync().ConfigureAwait(false);
  }

  #endregion
}
