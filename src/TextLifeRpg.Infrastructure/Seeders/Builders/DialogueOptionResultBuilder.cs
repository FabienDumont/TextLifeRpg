using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Seeders.Builders;

public class DialogueOptionResultBuilder(ApplicationContext context, Guid optionId)
{
  private readonly DialogueOptionResultDataModel _result = new() {Id = Guid.NewGuid(), DialogueOptionId = optionId};
  private readonly List<TextVariantBuilder> _resultSpoken = [];
  private readonly List<TextVariantBuilder> _resultNarrations = [];
  private readonly List<ConditionDataModel> _resultConditions = [];
  private readonly List<(Guid nextId, int order)> _nexts = [];

  public DialogueOptionResultBuilder EndDialogue()
  {
    _result.EndDialogue = true;
    return this;
  }

  public DialogueOptionResultBuilder WithAddMinutes()
  {
    _result.AddMinutes = true;
    return this;
  }

  public DialogueOptionResultBuilder WithTargetRelationshipValueChange(int v)
  {
    _result.TargetRelationshipValueChange = v;
    return this;
  }

  public DialogueOptionResultBuilder WithActorLearnFact(Fact fact)
  {
    _result.ActorLearnFact = fact;
    return this;
  }

  public DialogueOptionResultBuilder WithActorTargetSpecialAction(ActorTargetSpecialAction specialAction)
  {
    _result.ActorTargetSpecialAction = specialAction;
    return this;
  }

  public DialogueOptionResultBuilder WithNextDialogueOptions(IEnumerable<Guid> ids)
  {
    var i = 0;
    foreach (var id in ids) _nexts.Add((id, i++));
    return this;
  }

  public DialogueOptionResultBuilder WithTargetRelationshipValueCondition(string op, string value)
  {
    _resultConditions.Add(
      new ConditionDataModel
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionResult,
        ConditionType = ConditionType.TargetRelationship,
        Operator = op,
        OperandRight = value
      }
    );
    return this;
  }

  // RESULT-LEVEL CONDITIONS (ContextType = DialogueOptionResult)
  public DialogueOptionResultBuilder WithActorTraitCondition(Guid traitId)
  {
    _resultConditions.Add(
      new ConditionDataModel
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionResult,
        // ContextId set in BuildAsync
        ConditionType = ConditionType.ActorHasTrait,
        Operator = "==",
        OperandLeft = traitId.ToString(),
        OperandRight = "true",
        Negate = false
      }
    );
    return this;
  }

  public DialogueOptionResultBuilder WithActorRelationshipValueCondition(string op, string value)
  {
    _resultConditions.Add(
      new ConditionDataModel
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionResult,
        ConditionType = ConditionType.ActorRelationship,
        Operator = op,
        OperandRight = value
      }
    );
    return this;
  }

  public DialogueOptionResultBuilder WithActorEnergyCondition(string op, string value)
  {
    _resultConditions.Add(
      new ConditionDataModel
      {
        Id = Guid.NewGuid(),
        ContextType = ContextType.DialogueOptionResult,
        ConditionType = ConditionType.ActorEnergy,
        Operator = op,
        OperandRight = value
      }
    );
    return this;
  }

  public DialogueOptionResultBuilder AddResultSpokenText(string text, Action<TextVariantBuilder> build)
  {
    var b = new TextVariantBuilder(ContextType.DialogueOptionResultSpokenText, _result.Id, text);
    build(b);
    _resultSpoken.Add(b);
    return this;
  }

  public DialogueOptionResultBuilder AddResultNarration(string text, Action<TextVariantBuilder> build)
  {
    var b = new TextVariantBuilder(ContextType.DialogueOptionResultNarration, _result.Id, text);
    build(b);
    _resultNarrations.Add(b);
    return this;
  }

  public async Task BuildAsync()
  {
    await context.DialogueOptionResults.AddAsync(_result);

    foreach (var (nextId, order) in _nexts)
      await context.DialogueOptionResultNextDialogueOptions.AddAsync(
        new DialogueOptionResultNextDialogueOption
        {
          Id = Guid.NewGuid(),
          DialogueOptionResultId = _result.Id,
          NextDialogueOptionId = nextId,
          Order = order
        }
      );

    if (_resultConditions.Count > 0)
    {
      foreach (var c in _resultConditions) c.ContextId = _result.Id;
      await context.Conditions.AddRangeAsync(_resultConditions);
    }

    foreach (var b in _resultSpoken)
    {
      var textId = Guid.NewGuid();
      await context.DialogueOptionResultSpokenTexts.AddAsync(
        new DialogueOptionResultSpokenTextDataModel
        {
          Id = textId, DialogueOptionResultId = _result.Id, Text = b.Text
        }
      );
      if (!b.Conditions.Any())
      {
        continue;
      }

      foreach (var c in b.Conditions) c.ContextId = textId;
      await context.Conditions.AddRangeAsync(b.Conditions);
    }

    foreach (var b in _resultNarrations)
    {
      var id = Guid.NewGuid();
      await context.DialogueOptionResultNarrations.AddAsync(
        new DialogueOptionResultNarrationDataModel
        {
          Id = id, DialogueOptionResultId = _result.Id, Text = b.Text
        }
      );
      if (!b.Conditions.Any())
      {
        continue;
      }

      foreach (var c in b.Conditions) c.ContextId = id;
      await context.Conditions.AddRangeAsync(b.Conditions);
    }
  }
}
