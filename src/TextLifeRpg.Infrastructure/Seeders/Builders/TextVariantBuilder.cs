using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Helper;

namespace TextLifeRpg.Infrastructure.Seeders.Builders;

/// <summary>
/// A builder class for constructing text variants with associated conditions.
/// </summary>
/// <remarks>
/// This class is used to define multiple text variations, each with a set of conditions
/// that must be satisfied for it to be applicable. The text variants are typically part of dynamic
/// dialogue or narration systems.
/// </remarks>
public class TextVariantBuilder(ContextType contextType, Guid contextId, string text)
{
  /// <summary>
  /// Represents a private, internal list used to store condition data models
  /// for various contextual rule evaluations in the TextVariantBuilder.
  /// </summary>
  private readonly List<ConditionDataModel> _conditions = [];

  /// <summary>
  /// Adds an energy-based condition to the builder. The condition specifies a comparison
  /// operation and a value that determine how energy levels relate to the triggering of a condition.
  /// </summary>
  /// <param name="op">The comparison operator as a string.</param>
  /// <param name="value">The value to compare the energy level against, represented as a string.</param>
  /// <returns>
  /// Returns the current instance of <c>TextVariantBuilder</c>, enabling method chaining for further
  /// configuration of text variants.
  /// </returns>
  public TextVariantBuilder WithActorEnergyCondition(string op, string value)
  {
    _conditions.Add(ConditionBuilder.BuildEnergyConditions(contextType, contextId, [(op, value)]).Single());
    return this;
  }

  /// <summary>
  /// Adds a condition to the builder that checks whether the actor has a specific trait and optionally negates it.
  /// </summary>
  /// <param name="traitId">The unique identifier of the trait to be checked.</param>
  /// <param name="negate">
  /// Specifies whether the condition should be negated (i.e., check if the actor does not have the trait).
  /// Defaults to false.
  /// </param>
  /// <returns>
  /// The updated instance of <see cref="TextVariantBuilder"/> for chaining additional conditions or modifications.
  /// </returns>
  public TextVariantBuilder WithActorTraitCondition(Guid traitId, bool negate = false)
  {
    _conditions.Add(
      ConditionBuilder.BuildTraitConditions(contextType, contextId, ConditionType.ActorHasTrait, [traitId], negate)
        .Single()
    );
    return this;
  }

  /// <summary>
  /// Adds a relationship value-based condition to the builder. This condition specifies
  /// a comparative operation and a threshold value to determine the relationship's impact
  /// on the triggering of a condition.
  /// </summary>
  /// <param name="op">The comparison operator as a string, used to evaluate the relationship value.</param>
  /// <param name="value">The threshold value as a string, which is used for the comparison.</param>
  /// <returns>
  /// Returns the current instance of <c>TextVariantBuilder</c>, allowing method chaining for further
  /// configuration of text variants and their associated conditions.
  /// </returns>
  public TextVariantBuilder WithActorRelationshipValueCondition(string op, string value)
  {
    _conditions.Add(
      ConditionBuilder.BuildActorRelationshipValueConditions(contextType, contextId, [(op, value)]).Single()
    );
    return this;
  }

  /// <summary>
  /// Gets the text associated with the builder instance.
  /// </summary>
  public string Text => text;

  /// <summary>
  /// Gets the collection of conditions associated with the current builder instance.
  /// Each condition is represented as a <c>ConditionDataModel</c> and holds metadata
  /// about specific requirements or constraints evaluated in various contexts, such as
  /// dialogue options, narrations, or exploration scenarios.
  /// </summary>
  public IEnumerable<ConditionDataModel> Conditions => _conditions;
}
