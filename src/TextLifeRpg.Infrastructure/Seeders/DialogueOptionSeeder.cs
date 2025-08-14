using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.JsonDefinitions;
using TextLifeRpg.Infrastructure.Seeders.Builders;

namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
/// Dialogue option data seeder.
/// </summary>
public class DialogueOptionSeeder : IDataSeeder
{
  #region Fields

  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNameCaseInsensitive = true,
    Converters =
    {
      new JsonStringEnumConverter(allowIntegerValues: false)
    }
  };

  #endregion

  #region Implementation of IDataSeeder

  /// <inheritdoc />
  public async Task SeedAsync(ApplicationContext context)
  {
    var traitMap = await context.Traits.ToDictionaryAsync(t => t.Name, t => t.Id).ConfigureAwait(false);
    var folderPath = Path.Combine(AppContext.BaseDirectory, "Data", "DialogueOptions");
    var files = Directory.GetFiles(folderPath, "*.json", SearchOption.AllDirectories);

    var defs = new List<(DialogueOptionDefinition, Guid)>();
    foreach (var file in files)
    {
      var json = await File.ReadAllTextAsync(file);
      var def = JsonSerializer.Deserialize<DialogueOptionDefinition>(json, JsonOptions) ??
                throw new Exception($"Failed to parse {file}");
      defs.Add((def, Guid.NewGuid()));
    }

    foreach (var (def, optionId) in defs)
    {
      var builder = new DialogueOptionBuilder(context, optionId, def.Label);

      foreach (var c in def.Conditions)
      {
        if (c.ActorHasntLearnedFact is not null)
        {
          builder.WithActorLearnedFactCondition(c.ActorHasntLearnedFact.Value, true);
        }

        if (c.ActorTargetSpecialCondition is not null)
        {
          builder.WithActorTargetSpecialCondition(
            c.ActorTargetSpecialCondition.Label, c.ActorTargetSpecialCondition.Negate
          );
        }
      }

      foreach (var s in def.SpokenTexts)
      {
        builder.AddSpokenText(s.Text, b => ApplyTextConditions(b, s.Conditions, traitMap));
      }

      foreach (var res in def.Results)
      {
        var rb = builder.AddResult();

        if (res.TargetRelationshipValueChange is not null)
        {
          rb.WithTargetRelationshipValueChange(res.TargetRelationshipValueChange.Value);
        }

        if (res.ActorLearnFact is not null)
        {
          rb.WithActorLearnFact(res.ActorLearnFact.Value);
        }

        if (res.ActorTargetSpecialAction is not null)
        {
          rb.WithActorTargetSpecialAction(res.ActorTargetSpecialAction.Value);
        }

        if (res.EndsDialogue)
        {
          rb.EndDialogue();
        }

        if (res.NextDialogueOptionNames is not null)
        {
          var nextIds = res.NextDialogueOptionNames.Select(name => defs.First(d => d.Item1.Name == name).Item2)
            .ToList();
          rb.WithNextDialogueOptions(nextIds);
        }

        ApplyResultConditions(rb, res.Conditions, traitMap);

        foreach (var s in res.ResultSpokenTexts)
          rb.AddResultSpokenText(s.Text, b => ApplyTextConditions(b, s.Conditions, traitMap));

        foreach (var n in res.ResultNarrations)
          rb.AddResultNarration(n.Text, b => ApplyTextConditions(b, n.Conditions, traitMap));
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
  private static void ApplyTextConditions(
    TextVariantBuilder b, List<DialogueOptionConditionDefinition> conditions, Dictionary<string, Guid> traitMap
  )
  {
    foreach (var condition in conditions)
    {
      if (condition.ActorHasTrait != null && traitMap.TryGetValue(condition.ActorHasTrait, out var traitId))
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

  private static void ApplyResultConditions(
    DialogueOptionResultBuilder rb, List<DialogueOptionConditionDefinition> conditions,
    Dictionary<string, Guid> traitMap
  )
  {
    foreach (var c in conditions)
    {
      if (c.ActorHasTrait != null && traitMap.TryGetValue(c.ActorHasTrait, out var traitId))
      {
        rb.WithActorTraitCondition(traitId);
      }

      if (c.ActorRelationshipValue is not null)
      {
        rb.WithActorRelationshipValueCondition(
          c.ActorRelationshipValue.Operator, c.ActorRelationshipValue.Value.ToString()
        );
      }

      if (c.TargetRelationshipValue is not null)
      {
        rb.WithTargetRelationshipValueCondition(
          c.TargetRelationshipValue.Operator, c.TargetRelationshipValue.Value.ToString()
        );
      }

      if (c.ActorEnergy is not null)
      {
        rb.WithActorEnergyCondition(c.ActorEnergy.Operator, c.ActorEnergy.Value.ToString());
      }
    }
  }

  #endregion
}
