using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Factories;

/// <summary>
/// Factory class to handle the creation and management of relationships between characters in the application.
/// </summary>
public class RelationshipFactory(IRandomProvider randomProvider) : IRelationshipFactory
{
  #region Methods

  /// <summary>
  /// Generates a random date within the specified range of two dates.
  /// </summary>
  /// <param name="from">The starting date of the range.</param>
  /// <param name="to">The ending date of the range.</param>
  /// <param name="rnd">An instance of IRandomProvider to generate random values.</param>
  /// <returns>A random date between the specified 'from' and 'to' dates, inclusive.</returns>
  private static DateOnly RandomDateBetween(DateOnly from, DateOnly to, IRandomProvider rnd)
  {
    var range = (to.ToDateTime(TimeOnly.MinValue) - from.ToDateTime(TimeOnly.MinValue)).Days;
    return range <= 0 ? from : from.AddDays(rnd.Next(0, range + 1));
  }

  /// <summary>
  /// Determines and returns the maximum of two specified DateOnly values.
  /// </summary>
  /// <param name="a">The first DateOnly value to compare.</param>
  /// <param name="b">The second DateOnly value to compare.</param>
  /// <returns>The greater of the two DateOnly values.</returns>
  private static DateOnly Max(DateOnly a, DateOnly b)
  {
    return a > b ? a : b;
  }

  /// <summary>
  /// Determines the earlier of two specified dates.
  /// </summary>
  /// <param name="a">The first date to compare.</param>
  /// <param name="b">The second date to compare.</param>
  /// <returns>The earlier of the two specified dates.</returns>
  private static DateOnly Min(DateOnly a, DateOnly b)
  {
    return a < b ? a : b;
  }

  /// <summary>
  /// Determines the reciprocal relationship type for a given relationship type.
  /// </summary>
  /// <param name="type">The relationship type for which the reciprocal type is to be determined.</param>
  /// <returns>The reciprocal relationship type if applicable; otherwise, the input type.</returns>
  public static RelationshipType GetReciprocal(RelationshipType type)
  {
    return type switch
    {
      RelationshipType.Parent => RelationshipType.Child,
      RelationshipType.Child => RelationshipType.Parent,
      RelationshipType.Grandparent => RelationshipType.Grandchild,
      RelationshipType.Grandchild => RelationshipType.Grandparent,
      _ => type
    };
  }

  #endregion

  #region Implementation of IRelationshipFactory

  public List<Relationship> Create(
    List<Relationship> existingRelationships, Character sourceCharacter, Character targetCharacter,
    RelationshipType type, DateOnly currentDate
  )
  {
    if (existingRelationships.Any(r =>
          r.SourceCharacterId == sourceCharacter.Id && r.TargetCharacterId == targetCharacter.Id
        ))
    {
      return [];
    }

    List<Relationship> newRelationships = [];

    var sourceBirth = sourceCharacter.BirthDate;
    var targetBirth = targetCharacter.BirthDate;

    var firstInteraction =
      type is RelationshipType.Parent or RelationshipType.Child or RelationshipType.Grandparent
        or RelationshipType.Grandchild or RelationshipType.Sibling
        ? Max(sourceBirth, targetBirth)
        : RandomDateBetween(
          Max(sourceBirth, targetBirth), Min(Max(sourceBirth, targetBirth).AddYears(5), currentDate), randomProvider
        );

    var lastInteraction = RandomDateBetween(firstInteraction, currentDate, randomProvider);

    var value = type switch
    {
      RelationshipType.Sibling or RelationshipType.Parent or RelationshipType.Child or RelationshipType.Grandparent
        or RelationshipType.Grandchild => randomProvider.Next(-100, 100),
      RelationshipType.Friend => randomProvider.Next(40, 100),
      RelationshipType.Enemy => randomProvider.Next(-100, -40),
      RelationshipType.CasualRomanticPartner or RelationshipType.RomanticPartner or RelationshipType.Spouse =>
        randomProvider.Next(-100, 100),
      _ => randomProvider.Next(-39, 40)
    };

    newRelationships.Add(
      Relationship.Create(sourceCharacter.Id, targetCharacter.Id, type, firstInteraction, lastInteraction, value)
    );
    newRelationships.Add(
      Relationship.Create(
        targetCharacter.Id, sourceCharacter.Id, GetReciprocal(type), firstInteraction, lastInteraction, value
      )
    );

    return newRelationships;
  }

  #endregion
}
