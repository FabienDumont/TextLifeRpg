using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Helper;

/// <summary>
///   Utility class for constructing condition data models used in contextual checks,
///   such as energy thresholds or trait requirements, for dialogue, events, or gameplay logic.
/// </summary>
public static class ConditionBuilder
{
  #region Methods

  /// <summary>
  ///   Builds a condition that checks the actor's energy using a specified operator and value.
  /// </summary>
  /// <param name="contextType">The type of the entity this condition is attached to (e.g., Dialogue, Action).</param>
  /// <param name="contextId">The unique ID of the context entity.</param>
  /// <param name="op">The comparison operator to use (e.g., "&gt;", "&lt;", "=").</param>
  /// <param name="value">The threshold energy value to compare against.</param>
  /// <returns>A configured <see cref="ConditionDataModel"/> checking actor energy.</returns>
  private static ConditionDataModel EnergyCondition(ContextType contextType, Guid contextId, string op, string value)
  {
    return new ConditionDataModel
    {
      Id = Guid.NewGuid(),
      ContextType = contextType,
      ContextId = contextId,
      ConditionType = ConditionType.ActorEnergy,
      Operator = op,
      OperandRight = value,
      Negate = false
    };
  }

  /// <summary>
  ///   Builds multiple energy-based conditions for a given context.
  /// </summary>
  /// <param name="contextType">The type of the context entity the conditions belong to.</param>
  /// <param name="contextId">The identifier of the context entity.</param>
  /// <param name="rules">An array of operator/value pairs to generate conditions from.</param>
  /// <returns>A collection of <see cref="ConditionDataModel"/> entries.</returns>
  public static IEnumerable<ConditionDataModel> BuildEnergyConditions(
    ContextType contextType, Guid contextId, (string op, string value)[] rules
  )
  {
    return rules.Select(rule => EnergyCondition(contextType, contextId, rule.op, rule.value));
  }

  /// <summary>
  ///   Builds a single condition checking whether the actor has a specific trait.
  /// </summary>
  /// <param name="contextType">The type of the context entity the condition applies to.</param>
  /// <param name="contextId">The identifier of the context entity.</param>
  /// <param name="traitId">The identifier of the trait to check for.</param>
  /// <param name="negate">Whether the condition should check for absence instead of presence.</param>
  /// <returns>A trait-based <see cref="ConditionDataModel"/>.</returns>
  private static ConditionDataModel TraitCondition(
    ContextType contextType, Guid contextId, Guid traitId, bool negate = false
  )
  {
    return new ConditionDataModel
    {
      Id = Guid.NewGuid(),
      ContextType = contextType,
      ContextId = contextId,
      ConditionType = ConditionType.ActorHasTrait,
      OperandLeft = traitId.ToString(),
      Operator = "=",
      OperandRight = "true",
      Negate = negate
    };
  }

  /// <summary>
  ///   Builds a collection of trait based conditions for the given context.
  /// </summary>
  /// <param name="contextType">The type of the context the conditions apply to.</param>
  /// <param name="contextId">The identifier of the context entity.</param>
  /// <param name="traitIds">The traits required to satisfy the conditions.</param>
  /// <param name="negate">Whether or not the trait check should be negated.</param>
  /// <returns>A list of trait conditions.</returns>
  public static IEnumerable<ConditionDataModel> BuildTraitConditions(
    ContextType contextType, Guid contextId, IEnumerable<Guid> traitIds, bool negate = false
  )
  {
    return traitIds.Select(id => TraitCondition(contextType, contextId, id, negate));
  }

  #endregion
}
