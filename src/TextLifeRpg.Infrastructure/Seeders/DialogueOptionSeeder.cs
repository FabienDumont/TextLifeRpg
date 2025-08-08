using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.JsonDefinitions;
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
    var traitMap = await context.Traits.ToDictionaryAsync(t => t.Name, t => t.Id).ConfigureAwait(false);
    var baseDir = AppContext.BaseDirectory;
    var fullPath = Path.Combine(baseDir, "Data/DialogueOptions.json");

    await using var stream = File.OpenRead(fullPath);
    var definitions = await JsonSerializer.DeserializeAsync<List<DialogueOptionDefinition>>(stream) ??
                      throw new InvalidOperationException("Failed to parse dialogue definition file.");

    foreach (var def in definitions)
    {
      var builder = new DialogueOptionBuilder(context, def.Label);

      foreach (var spokenText in def.SpokenTexts)
      {
        builder.AddSpokenText(spokenText.Text, b => { ApplyConditions(b, spokenText.Conditions, traitMap); });
      }

      foreach (var result in def.Results)
      {
        if (result.EndsDialogue)
        {
          builder.EndDialogue();
        }

        foreach (var spoken in result.ResultSpokenTexts)
        {
          builder.AddResultSpokenText(spoken.Text, b => { ApplyConditions(b, spoken.Conditions, traitMap); });
        }

        foreach (var narration in result.ResultNarrations)
        {
          builder.AddResultNarration(narration.Text, b => { ApplyConditions(b, narration.Conditions, traitMap); });
        }
      }

      await builder.BuildAsync();
    }
  }

  #endregion

  #region Methods

  private static void ApplyConditions(
    TextVariantBuilder b, List<DialogueConditionDefinition> conditions, Dictionary<string, Guid> traitMap
  )
  {
    foreach (var condition in conditions)
    {
      if (condition.Trait != null && traitMap.TryGetValue(condition.Trait, out var traitId))
      {
        b.WithActorTraitCondition(traitId);
      }

      if (condition.ActorRelationshipValue is not null)
      {
        b.WithActorRelationshipValueCondition(
          condition.ActorRelationshipValue.Operator, condition.ActorRelationshipValue.Value.ToString()
        );
      }

      if (condition.ActorEnergy is not null)
      {
        b.WithActorEnergyCondition(condition.ActorEnergy.Operator, condition.ActorEnergy.Value.ToString());
      }
    }
  }

  #endregion
}
