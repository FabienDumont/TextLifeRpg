using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Factories;

public class RelationshipFactory(IRandomProvider randomProvider) : IRelationshipFactory
{
  #region Methods

  private static DateOnly RandomDateBetween(DateOnly from, DateOnly to, IRandomProvider rnd)
  {
    var range = (to.ToDateTime(TimeOnly.MinValue) - from.ToDateTime(TimeOnly.MinValue)).Days;
    return range <= 0 ? from : from.AddDays(rnd.Next(0, range + 1));
  }

  private static DateOnly Max(DateOnly a, DateOnly b)
  {
    return a > b ? a : b;
  }

  private static DateOnly Min(DateOnly a, DateOnly b)
  {
    return a < b ? a : b;
  }

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
