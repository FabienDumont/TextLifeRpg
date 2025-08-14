using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;

namespace TextLifeRpg.Infrastructure.Seeders.Builders;

public class DialogueOptionBuilder(ApplicationContext context, Guid id, string label)
{
  private readonly DialogueOptionDataModel _dialogueOption = new() {Id = id, Label = label};
  private readonly List<ConditionDataModel> _conditions = [];
  private readonly List<TextVariantBuilder> _spokenTextVariants = [];
  private readonly List<DialogueOptionResultBuilder> _results = [];

  public DialogueOptionBuilder WithActorLearnedFactCondition(string actorLearnedFact, bool actorLearnedFactNegate)
  {
    _conditions.Add(
      ConditionBuilder.BuildActorLearnedFactConditions(
        ContextType.DialogueOption, _dialogueOption.Id, [(actorLearnedFact, actorLearnedFactNegate)]
      ).Single()
    );
    return this;
  }

  public DialogueOptionBuilder WithActorTargetSpecialCondition(string specialConditionLabel, bool negate)
  {
    _conditions.Add(
      ConditionBuilder.BuildActorTargetSpecialConditions(
        ContextType.DialogueOption, _dialogueOption.Id, [(specialConditionLabel, negate)]
      ).Single()
    );
    return this;
  }

  public DialogueOptionResultBuilder AddResult()
  {
    var rb = new DialogueOptionResultBuilder(context, _dialogueOption.Id);
    _results.Add(rb);
    return rb;
  }

  public DialogueOptionBuilder AddSpokenText(string text, Action<TextVariantBuilder> build)
  {
    var b = new TextVariantBuilder(ContextType.DialogueOptionSpokenText, _dialogueOption.Id, text);
    build(b);
    _spokenTextVariants.Add(b);
    return this;
  }

  public async Task BuildAsync()
  {
    await context.DialogueOptions.AddAsync(_dialogueOption);

    if (_conditions.Count > 0)
    {
      foreach (var c in _conditions) c.ContextId = _dialogueOption.Id;
      await context.Conditions.AddRangeAsync(_conditions);
    }

    foreach (var b in _spokenTextVariants)
    {
      var textId = Guid.NewGuid();
      await context.DialogueOptionSpokenTexts.AddAsync(
        new DialogueOptionSpokenTextDataModel
        {
          Id = textId, DialogueOptionId = _dialogueOption.Id, Text = b.Text
        }
      );
      if (!b.Conditions.Any())
      {
        continue;
      }

      foreach (var c in b.Conditions) c.ContextId = textId;
      await context.Conditions.AddRangeAsync(b.Conditions);
    }

    foreach (var r in _results) await r.BuildAsync();

    await context.SaveChangesAsync();
  }
}
