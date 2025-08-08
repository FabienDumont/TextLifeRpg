using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Seeders.Builders;

public class DialogueOptionBuilder
{
  private readonly ApplicationContext _context;
  private readonly DialogueOptionDataModel _option;
  private readonly DialogueOptionResultDataModel _result;
  private readonly List<TextVariantBuilder> _spokenTextVariants = [];
  private readonly List<TextVariantBuilder> _resultSpokenTextVariants = [];
  private readonly List<TextVariantBuilder> _resultNarrationBuilders = [];

  public DialogueOptionBuilder(ApplicationContext context, string label)
  {
    _context = context;
    _option = new DialogueOptionDataModel
    {
      Id = Guid.NewGuid(),
      Label = label
    };

    _result = new DialogueOptionResultDataModel
    {
      Id = Guid.NewGuid(),
      DialogueOptionId = _option.Id
    };
  }

  public DialogueOptionBuilder EndDialogue()
  {
    _result.EndDialogue = true;
    return this;
  }

  public DialogueOptionBuilder AddSpokenText(string text, Action<TextVariantBuilder> buildConditions)
  {
    var builder = new TextVariantBuilder(ContextType.DialogueOptionSpokenText, _option.Id, text);
    buildConditions(builder);
    _spokenTextVariants.Add(builder);
    return this;
  }

  public DialogueOptionBuilder AddResultSpokenText(string text, Action<TextVariantBuilder> buildConditions)
  {
    var builder = new TextVariantBuilder(ContextType.DialogueOptionResultSpokenText, _result.Id, text);
    buildConditions(builder);
    _resultSpokenTextVariants.Add(builder);
    return this;
  }

  public DialogueOptionBuilder AddResultNarration(string text, Action<TextVariantBuilder> buildConditions)
  {
    var builder = new TextVariantBuilder(ContextType.DialogueOptionResultNarration, _result.Id, text);
    buildConditions(builder);
    _resultNarrationBuilders.Add(builder);
    return this;
  }

  public async Task BuildAsync()
  {
    await _context.DialogueOptions.AddAsync(_option);
    await _context.DialogueOptionResults.AddAsync(_result);

    foreach (var builder in _spokenTextVariants)
    {
      var textId = Guid.NewGuid();

      await _context.DialogueOptionSpokenTexts.AddAsync(
        new DialogueOptionSpokenTextDataModel
        {
          Id = textId,
          DialogueOptionId = _option.Id,
          Text = builder.Text
        }
      );

      if (builder.Conditions.Any())
      {
        foreach (var c in builder.Conditions) c.ContextId = textId;

        await _context.Conditions.AddRangeAsync(builder.Conditions);
      }
    }

    foreach (var builder in _resultSpokenTextVariants)
    {
      var textId = Guid.NewGuid();

      await _context.DialogueOptionResultSpokenTexts.AddAsync(
        new DialogueOptionResultSpokenTextDataModel
        {
          Id = textId,
          DialogueOptionResultId = _result.Id,
          Text = builder.Text
        }
      );

      if (builder.Conditions.Any())
      {
        foreach (var c in builder.Conditions) c.ContextId = textId;

        await _context.Conditions.AddRangeAsync(builder.Conditions);
      }
    }

    foreach (var builder in _resultNarrationBuilders)
    {
      var narrationId = Guid.NewGuid();

      await _context.DialogueOptionResultNarrations.AddAsync(
        new DialogueOptionResultNarrationDataModel
        {
          Id = narrationId,
          DialogueOptionResultId = _result.Id,
          Text = builder.Text
        }
      );

      if (builder.Conditions.Any())
      {
        foreach (var c in builder.Conditions) c.ContextId = narrationId;

        await _context.Conditions.AddRangeAsync(builder.Conditions);
      }
    }

    await _context.SaveChangesAsync();
  }
}
