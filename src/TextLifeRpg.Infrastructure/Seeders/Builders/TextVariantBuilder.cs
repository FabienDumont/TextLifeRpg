using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;

namespace TextLifeRpg.Infrastructure.Seeders.Builders;

public class TextVariantBuilder(ContextType contextType, Guid contextId, string text)
{
  private readonly List<ConditionDataModel> _conditions = [];

  public TextVariantBuilder WithEnergyCondition(string op, string value)
  {
    _conditions.Add(ConditionBuilder.BuildEnergyConditions(contextType, contextId, [(op, value)]).Single());
    return this;
  }

  public TextVariantBuilder WithTraitCondition(Guid traitId, bool negate = false)
  {
    _conditions.Add(ConditionBuilder.BuildActorTraitConditions(contextType, contextId, [traitId], negate).Single());
    return this;
  }

  public string Text => text;
  public IEnumerable<ConditionDataModel> Conditions => _conditions;
}
