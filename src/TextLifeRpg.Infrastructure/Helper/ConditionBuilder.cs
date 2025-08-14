using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Helper;

/// <summary>
/// Utility class for constructing condition data models used in contextual checks,
/// such as energy thresholds or trait requirements, for dialogue, events, or gameplay logic.
/// </summary>
public static class ConditionBuilder
{
  #region Methods

  /// <summary>
  /// Builds a condition that checks the actor's energy using a specified operator and value.
  /// </summary>
  /// <param name="contextType">The type of the entity this condition is attached to (e.g., Dialogue, Action).</param>
  /// <param name="contextId">The unique ID of the context entity.</param>
  /// <param name="op">The comparison operator to use.</param>
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
  /// Builds a single condition checking whether the actor has a specific trait.
  /// </summary>
  /// <param name="contextType">The type of the context entity the condition applies to.</param>
  /// <param name="contextId">The identifier of the context entity.</param>
  /// <param name="conditionType">The condition type.</param>
  /// <param name="traitId">The identifier of the trait to check for.</param>
  /// <param name="negate">Whether the condition should check for absence instead of presence.</param>
  /// <returns>A trait-based <see cref="ConditionDataModel"/>.</returns>
  private static ConditionDataModel TraitCondition(
    ContextType contextType, Guid contextId, ConditionType conditionType, Guid traitId, bool negate = false
  )
  {
    return new ConditionDataModel
    {
      Id = Guid.NewGuid(),
      ContextType = contextType,
      ContextId = contextId,
      ConditionType = conditionType,
      OperandLeft = traitId.ToString(),
      Operator = "==",
      OperandRight = "true",
      Negate = negate
    };
  }

  /// <summary>
  /// Constructs a condition that evaluates an actor's relationship value based on the specified operator and value.
  /// </summary>
  /// <param name="contextType">
  /// The type of the entity this condition is associated with (e.g., Dialogue, Action).
  /// </param>
  /// <param name="contextId">The unique identifier of the contextual entity.</param>
  /// <param name="op">The comparison operator to apply.</param>
  /// <param name="value">The value to compare the relationship against.</param>
  /// <returns>A configured <see cref="ConditionDataModel"/> for checking the actor's relationship value.</returns>
  private static ConditionDataModel ActorRelationshipValueCondition(
    ContextType contextType, Guid contextId, string op, string value
  )
  {
    return new ConditionDataModel
    {
      Id = Guid.NewGuid(),
      ContextType = contextType,
      ContextId = contextId,
      ConditionType = ConditionType.ActorRelationship,
      Operator = op,
      OperandRight = value,
      Negate = false
    };
  }

  /// <summary>
  /// Builds a condition that verifies whether an actor has learned a specific fact.
  /// </summary>
  /// <param name="contextType">The type of entity (e.g., Dialogue, Action) the condition is based on.</param>
  /// <param name="contextId">The unique identifier of the context entity.</param>
  /// <param name="factKey">The key representing the specific fact to check if the actor has learned.</param>
  /// <param name="negate">Determines whether to negate the condition (true if the condition should be inverted).</param>
  /// <returns>A configured <see cref="ConditionDataModel"/> checking if an actor has learned the specified fact.</returns>
  private static ConditionDataModel ActorLearnedFactCondition(
    ContextType contextType, Guid contextId, string factKey, bool negate
  )
  {
    return new ConditionDataModel
    {
      Id = Guid.NewGuid(),
      ContextType = contextType,
      ContextId = contextId,
      ConditionType = ConditionType.ActorLearnedFact,
      OperandLeft = factKey,
      Operator = "==",
      OperandRight = "true",
      Negate = negate
    };
  }

  private static ConditionDataModel ActorTargetSpecialCondition(
    ContextType contextType, Guid contextId, string conditionLabel, bool negate
  )
  {
    return new ConditionDataModel
    {
      Id = Guid.NewGuid(),
      ContextType = contextType,
      ContextId = contextId,
      ConditionType = ConditionType.ActorTargetSpecialCondition,
      OperandLeft = conditionLabel,
      Operator = "==",
      OperandRight = "true",
      Negate = negate
    };
  }

  /// <summary>
  /// Builds multiple energy-based conditions for a given context.
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
  /// Builds a collection of trait based conditions for the given context.
  /// </summary>
  /// <param name="contextType">The type of the context the conditions apply to.</param>
  /// <param name="contextId">The identifier of the context entity.</param>
  /// <param name="conditionType">The condition type.</param>
  /// <param name="traitIds">The traits required to satisfy the conditions.</param>
  /// <param name="negate">Whether the trait check should be negated.</param>
  /// <returns>A list of trait conditions.</returns>
  public static IEnumerable<ConditionDataModel> BuildTraitConditions(
    ContextType contextType, Guid contextId, ConditionType conditionType, IEnumerable<Guid> traitIds,
    bool negate = false
  )
  {
    return traitIds.Select(id => TraitCondition(contextType, contextId, conditionType, id, negate));
  }

  /// <summary>
  /// Constructs a collection of condition data models that validate actor relationship values
  /// based on a sequence of operator-value rules.
  /// </summary>
  /// <param name="contextType">The type of the entity associated with the conditions (e.g., Dialogue, Action).</param>
  /// <param name="contextId">The unique identifier of the entity context.</param>
  /// <param name="rules">
  /// An array of tuples where each includes an operator and value to define the relationship value conditions.
  /// </param>
  /// <returns>
  /// An enumerable collection of <see cref="ConditionDataModel"/> representing the relationship value conditions.
  /// </returns>
  public static IEnumerable<ConditionDataModel> BuildActorRelationshipValueConditions(
    ContextType contextType, Guid contextId, (string op, string value)[] rules
  )
  {
    return rules.Select(rule => ActorRelationshipValueCondition(contextType, contextId, rule.op, rule.value));
  }

  /// <summary>
  /// Builds a collection of conditions that check whether the actor has learned specific facts, with an option to negate the condition.
  /// </summary>
  /// <param name="contextType">The type of the entity this condition is associated with (e.g., Dialogue, Action).</param>
  /// <param name="contextId">The unique identifier of the context entity.</param>
  /// <param name="facts">A collection of tuples where each tuple contains the fact key to check and a boolean indicating if the condition should be negated.</param>
  /// <returns>A collection of <see cref="ConditionDataModel"/> objects, each representing a condition for the specified facts.</returns>
  public static IEnumerable<ConditionDataModel> BuildActorLearnedFactConditions(
    ContextType contextType, Guid contextId, IEnumerable<(string factKey, bool negate)> facts
  )
  {
    return facts.Select(f => ActorLearnedFactCondition(contextType, contextId, f.factKey, f.negate));
  }

  public static IEnumerable<ConditionDataModel> BuildActorTargetSpecialConditions(
    ContextType contextType, Guid contextId, IEnumerable<(string conditionLabel, bool negate)> specialConditions
  )
  {
    return specialConditions.Select(f => ActorTargetSpecialCondition(contextType, contextId, f.conditionLabel, f.negate)
    );
  }

  #endregion
}
