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
    var targetRelationship = context.World.Relationships.FirstOrDefault(r =>
      r.SourceCharacterId == target?.Id && r.TargetCharacterId == actor.Id
    );

    return condition.ConditionType switch
    {
      ConditionType.ActorEnergy when operandRight is null => throw new InvalidOperationException(
        $"{nameof(ConditionType.ActorEnergy)} requires OperandRight."
      ),
      ConditionType.ActorEnergy => CompareInt(
        actor.Energy, condition.Operator, int.Parse(operandRight), condition.Negate
      ),

      ConditionType.ActorMoney when operandRight is null => throw new InvalidOperationException(
        $"{nameof(ConditionType.ActorMoney)} requires OperandRight."
      ),
      ConditionType.ActorMoney => CompareInt(
        actor.Money, condition.Operator, int.Parse(operandRight), condition.Negate
      ),

      ConditionType.ActorHasTrait => EvaluateHasTrait(condition, actor.TraitsId),

      ConditionType.ActorRelationship when operandRight is null => throw new InvalidOperationException(
        $"{nameof(ConditionType.ActorRelationship)} requires OperandRight."
      ),
      ConditionType.ActorRelationship => CompareInt(
        actorRelationship?.Value ?? 0, condition.Operator, int.Parse(operandRight), condition.Negate
      ),

      ConditionType.ActorLearnedFact => actorRelationship is null ||
                                        EvaluateActorLearnedFact(condition, actorRelationship),
      ConditionType.ActorTargetSpecialCondition => EvaluateSpecialCondition(
        condition, actor,
        target ?? throw new InvalidOperationException(
          $"{nameof(ConditionType.ActorTargetSpecialCondition)} requires target."
        )
      ),
      ConditionType.TargetRelationship when operandRight is null => throw new InvalidOperationException(
        $"{nameof(ConditionType.TargetRelationship)} requires OperandRight."
      ),
      ConditionType.TargetRelationship => CompareInt(
        targetRelationship?.Value ?? 0, condition.Operator, int.Parse(operandRight), condition.Negate
      ),
      _ => throw new InvalidOperationException($"Invalid {nameof(ConditionType)}.")
    };
  }

  /// <summary>
  /// Evaluates whether an actor has learned a specific fact within a relationship context.
  /// </summary>
  /// <param name="condition">The condition to evaluate, which includes the fact identifier and evaluation logic.</param>
  /// <param name="actorRelationship">The relationship containing the actor's history, which is used to determine if the fact is learned.</param>
  /// <returns>True if the actor has learned the specified fact and the condition evaluates to true; otherwise, false.</returns>
  private static bool EvaluateActorLearnedFact(ConditionDataModel condition, Relationship actorRelationship)
  {
    var raw = condition.OperandLeft ??
              throw new InvalidOperationException($"{nameof(condition.OperandLeft)} shouldn't be null.");

    if (!Enum.TryParse<Fact>(raw, ignoreCase: true, out var fact))
    {
      throw new ArgumentOutOfRangeException(nameof(condition), raw, "Unknown fact.");
    }

    var learned = actorRelationship.History.HasLearnedFact(fact);
    return condition.Negate ? !learned : learned;
  }

  /// <summary>
  /// Evaluates a special condition based on the given condition data, actor, and target.
  /// </summary>
  /// <param name="condition">The condition to be evaluated.</param>
  /// <param name="actor">The actor character in the current context.</param>
  /// <param name="target">The target character in the current context, if any.</param>
  /// <returns>True if the special condition is met; otherwise, false.</returns>
  private static bool EvaluateSpecialCondition(ConditionDataModel condition, Character actor, Character target)
  {
    return condition.OperandLeft switch
    {
      "HaveTargetPhoneNumber" => !condition.Negate == actor.Phone.Contacts.Any(c => c.Character.Id == target.Id),
      _ => throw new ArgumentOutOfRangeException(nameof(condition), condition.OperandLeft, null)
    };
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
