using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Seeders.Builders;

public class ExplorationActionBuilder
{
  private readonly ApplicationContext _context;
  private readonly ExplorationActionDataModel _action;
  private readonly ExplorationActionResultDataModel _result;
  private readonly List<TextVariantBuilder> _narrationBuilders = [];

  public ExplorationActionBuilder(
    ApplicationContext context, string label, int neededMinutes, Guid locationId, Guid roomId
  )
  {
    _context = context;
    _action = new ExplorationActionDataModel
    {
      Id = Guid.NewGuid(),
      Label = label,
      NeededMinutes = neededMinutes,
      LocationId = locationId,
      RoomId = roomId
    };

    _result = new ExplorationActionResultDataModel
    {
      Id = Guid.NewGuid(),
      ExplorationActionId = _action.Id,
      AddMinutes = true
    };
  }

  public ExplorationActionBuilder WithEnergyChange(int energy)
  {
    _result.EnergyChange = energy;
    return this;
  }

  public ExplorationActionBuilder AddNarration(string text, Action<TextVariantBuilder> buildConditions)
  {
    var builder = new TextVariantBuilder(ContextType.ExplorationActionResult, _result.Id, text);
    buildConditions(builder);
    _narrationBuilders.Add(builder);
    return this;
  }

  public async Task BuildAsync()
  {
    await _context.ExplorationActions.AddAsync(_action);
    await _context.ExplorationActionResults.AddAsync(_result);

    foreach (var builder in _narrationBuilders)
    {
      var narrationId = Guid.NewGuid();
      await _context.ExplorationActionResultNarrations.AddAsync(
        new ExplorationActionResultNarrationDataModel
        {
          Id = narrationId,
          ExplorationActionResultId = _result.Id,
          Text = builder.Text
        }
      );

      var conditionModels = builder.Conditions.Select(c =>
        {
          c.ContextId = narrationId;
          return c;
        }
      );

      await _context.Conditions.AddRangeAsync(conditionModels);

      await _context.SaveChangesAsync();
    }
  }
}
