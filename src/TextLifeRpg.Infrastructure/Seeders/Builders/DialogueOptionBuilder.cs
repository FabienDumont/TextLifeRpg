using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Seeders.Builders;

public class DialogueOptionBuilder(ApplicationContext context, Guid id, string label)
{
  private readonly DialogueOptionDataModel _option = new() {Id = id, Label = label};
  private readonly List<TextVariantBuilder> _spokenTextVariants = [];
  private readonly List<DialogueOptionResultBuilder> _results = [];

  public DialogueOptionResultBuilder AddResult()
  {
    var rb = new DialogueOptionResultBuilder(context, _option.Id);
    _results.Add(rb);
    return rb;
  }

  public DialogueOptionBuilder AddSpokenText(string text, Action<TextVariantBuilder> build)
  {
    var b = new TextVariantBuilder(ContextType.DialogueOptionSpokenText, _option.Id, text);
    build(b);
    _spokenTextVariants.Add(b);
    return this;
  }

  public async Task BuildAsync()
  {
    await context.DialogueOptions.AddAsync(_option);

    foreach (var b in _spokenTextVariants)
    {
      var textId = Guid.NewGuid();
      await context.DialogueOptionSpokenTexts.AddAsync(
        new DialogueOptionSpokenTextDataModel
        {
          Id = textId, DialogueOptionId = _option.Id, Text = b.Text
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
