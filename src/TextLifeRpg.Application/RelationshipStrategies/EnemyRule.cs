using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.RelationshipStrategies;

/// <summary>
/// Represents a rule that generates enemy relationships between characters in a role-playing game setting.
/// </summary>
public class EnemyRule(
  IRandomProvider randomProvider, ICharacterPairSelector pairSelector, IRelationshipFactory relationshipFactory
) : IRelationshipRule
{
  #region Implementation of IRelationshipRule

  /// <summary>
  /// Generates a list of enemy relationships from a given set of characters and existing relationships.
  /// </summary>
  /// <param name="characters">The list of characters to evaluate for potential enemy relationships.</param>
  /// <param name="existingRelationships">The list of current relationships used to prevent duplicate or conflicting relationships.</param>
  /// <param name="currentDate">The current date, used for creating and timestamping relationships.</param>
  /// <returns>A list of newly created enemy relationships.</returns>
  public List<Relationship> Generate(
    List<Character> characters, List<Relationship> existingRelationships, DateOnly currentDate
  )
  {
    List<Relationship> newRelationships = [];
    var maxPairs = characters.Count / 8;
    var pairsCreated = 0;

    var pairs = pairSelector.SelectPairs(characters).OrderBy(_ => randomProvider.Next(0, int.MaxValue)).ToList();

    foreach (var (a, b) in pairs)
    {
      if (pairsCreated >= maxPairs)
      {
        break;
      }

      var results = relationshipFactory.Create(existingRelationships, a, b, RelationshipType.Enemy, currentDate);

      newRelationships.AddRange(results);
      pairsCreated += results.Count;
    }

    return newRelationships;
  }

  #endregion
}
