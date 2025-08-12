using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Helper;

/// <summary>
/// Evaluates game conditions.
/// </summary>
public static class ConditionEvaluator
{
  #region Methods

  /// <summary>
  /// Evaluates a single condition using the provided game state.
  /// </summary>
  /// <param name="condition">The condition to evaluate.</param>
  /// <param name="context">The game context.</param>
  /// <returns>True if the condition is met; otherwise, false.</returns>
  public static bool EvaluateCondition(ConditionDataModel condition, GameContext context)
  {
    var actor = context.Actor;
    var target = context.Target;
    var operandRight = condition.OperandRight;
    var actorRelationship = context.World.Relationships.FirstOrDefault(r =>
      r.SourceCharacterId == actor.Id && r.TargetCharacterId == target?.Id
    );

    return condition.ConditionType switch
    {
      ConditionType.ActorEnergy when operandRight is null => throw new InvalidOperationException(
        "ActorEnergy condition requires OperandRight."
      ),
      ConditionType.ActorEnergy => CompareInt(
        actor.Energy, condition.Operator, int.Parse(operandRight), condition.Negate
      ),

      ConditionType.ActorMoney when operandRight is null => throw new InvalidOperationException(
        "ActorMoney condition requires OperandRight."
      ),
      ConditionType.ActorMoney => CompareInt(
        actor.Money, condition.Operator, int.Parse(operandRight), condition.Negate
      ),

      ConditionType.ActorHasTrait => EvaluateHasTrait(condition, actor.TraitsId),

      ConditionType.ActorRelationship when operandRight is null => throw new InvalidOperationException(
        "ActorRelationship condition requires OperandRight."
      ),
      ConditionType.ActorRelationship => CompareInt(
        actorRelationship?.Value ?? 0, condition.Operator, int.Parse(operandRight), condition.Negate
      ),
      ConditionType.ActorLearnedFact => actorRelationship is null ||
                                        EvaluateActorLearnedTrait(condition, actorRelationship),
      _ => throw new InvalidOperationException("Invalid ConditionType.")
    };
  }

  private static bool EvaluateActorLearnedTrait(ConditionDataModel condition, Relationship actorRelationship)
  {
    var learnedFact = actorRelationship.History.HasLearnedFact(
      condition.OperandLeft ??
      throw new InvalidOperationException($"{nameof(condition.OperandLeft)} shouldn't be null.")
    );
    return learnedFact && !condition.Negate || !learnedFact && condition.Negate;
  }

  /// <summary>
  /// Evaluates a "HasTrait" condition by checking if the provided traits include the required trait.
  /// </summary>
  /// <param name="condition">The trait-based condition to evaluate.</param>
  /// <param name="traitIds">The list of traits identifiers to search within.</param>
  /// <returns>True if the trait condition is met; otherwise, false.</returns>
  private static bool EvaluateHasTrait(ConditionDataModel condition, IEnumerable<Guid> traitIds)
  {
    var hasTrait = Guid.TryParse(condition.OperandLeft, out var traitId) && traitIds.Contains(traitId);
    return condition.Negate ? !hasTrait : hasTrait;
  }

  /// <summary>
  /// Evaluates a numerical comparison for relationship values using the given operator.
  /// </summary>
  /// <param name="left">The current relationship value.</param>
  /// <param name="op">The comparison operator (e.g., '=', '>=', etc.).</param>
  /// <param name="right">The target value to compare against.</param>
  /// <param name="negate">Whether to negate the result.</param>
  /// <returns>True if the comparison is satisfied; otherwise, false.</returns>
  private static bool CompareInt(int left, string op, int right, bool negate)
  {
    var result = op switch
    {
      "==" => left == right,
      "!=" => left != right,
      ">" => left > right,
      "<" => left < right,
      ">=" => left >= right,
      "<=" => left <= right,
      _ => throw new InvalidOperationException("Unknown operator.")
    };

    return negate ? !result : result;
  }

  #endregion
}
