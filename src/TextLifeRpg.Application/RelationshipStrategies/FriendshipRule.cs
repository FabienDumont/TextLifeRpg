using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.RelationshipStrategies;

/// <summary>
/// Represents a rule for generating and managing friendship relationships between characters.
/// </summary>
public class FriendshipRule(
  IRandomProvider randomProvider, ICharacterPairSelector pairSelector, IRelationshipFactory relationshipFactory
) : IRelationshipRule
{
  #region Implementation of IRelationshipRule

  /// <summary>
  /// Generates a set of new friendship relationships based on the provided characters and existing relationships.
  /// </summary>
  /// <param name="characters">The list of characters available to form new relationships.</param>
  /// <param name="existingRelationships">The existing relationships between characters.</param>
  /// <param name="currentDate">The current date used to timestamp the relationships.</param>
  /// <returns>A list of newly generated friendship relationships.</returns>
  public List<Relationship> Generate(
    List<Character> characters, List<Relationship> existingRelationships, DateOnly currentDate
  )
  {
    List<Relationship> newRelationships = [];
    var maxPairs = characters.Count / 5;
    var pairsCreated = 0;

    var pairs = pairSelector.SelectPairs(characters).OrderBy(_ => randomProvider.Next(0, int.MaxValue)).ToList();

    foreach (var (a, b) in pairs)
    {
      if (pairsCreated >= maxPairs)
      {
        break;
      }

      var results = relationshipFactory.Create(existingRelationships, a, b, RelationshipType.Friend, currentDate);

      newRelationships.AddRange(results);
      pairsCreated += results.Count;
    }

    return newRelationships;
  }

  #endregion
}
