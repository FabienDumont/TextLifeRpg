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
    var folderPath = Path.Combine(AppContext.BaseDirectory, "Data", "DialogueOptions");
    var files = Directory.GetFiles(folderPath, "*.json", SearchOption.AllDirectories);

    foreach (var file in files)
    {
      var json = await File.ReadAllTextAsync(file);

      var def = JsonSerializer.Deserialize<DialogueOptionDefinition>(json) ??
                throw new InvalidOperationException($"Failed to parse dialogue definition file: {file}");

      var builder = new DialogueOptionBuilder(context, def.Label);

      foreach (var spokenText in def.SpokenTexts)
      {
        builder.AddSpokenText(spokenText.Text, b => { ApplyConditions(b, spokenText.Conditions, traitMap); });
      }

      foreach (var result in def.Results)
      {
        if (result.TargetRelationshipValueChange is not null)
        {
          builder.WithTargetRelationshipValueChange(result.TargetRelationshipValueChange.Value);
        }

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

  /// <summary>
  /// Applies a series of conditions to a <see cref="TextVariantBuilder"/> object.
  /// </summary>
  /// <param name="b">The <see cref="TextVariantBuilder"/> instance to which the conditions will be applied.</param>
  /// <param name="conditions">A list of <see cref="DialogueOptionConditionDefinition"/> representing the conditions to be applied.</param>
  /// <param name="traitMap">A dictionary mapping trait names to their corresponding GUIDs.</param>
  private static void ApplyConditions(
    TextVariantBuilder b, List<DialogueOptionConditionDefinition> conditions, Dictionary<string, Guid> traitMap
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
