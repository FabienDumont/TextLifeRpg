using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.RelationshipStrategies;

/// <summary>
/// Represents a relationship rule that generates casual romantic partner relationships
/// between characters based on attraction values and pre-existing relationships.
/// </summary>
public class CasualRomanticPartnerRule(
  IRandomProvider randomProvider, ICharacterPairSelector pairSelector, ICharacterService characterService,
  IRelationshipFactory relationshipFactory
) : IRelationshipRule
{
  #region Implementation of IRelationshipRule

  /// <summary>
  /// Generates a list of new casual romantic partner relationships based on the provided characters,
  /// existing relationships, and the current date.
  /// </summary>
  /// <param name="characters">
  /// A list of available characters to be considered for relationship pairing.
  /// </param>
  /// <param name="existingRelationships">
  /// A list of existing relationships that should be considered to avoid duplication or conflicts.
  /// </param>
  /// <param name="currentDate">
  /// The current date used for evaluating conditions such as attraction values for relationship generation.
  /// </param>
  /// <returns>
  /// A list of newly created casual romantic partner relationships.
  /// </returns>
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

      var attraction = characterService.GetAttractionValue(a, b, currentDate);
      if (attraction < 40)
      {
        continue;
      }

      var results = relationshipFactory.Create(
        existingRelationships, a, b, RelationshipType.CasualRomanticPartner, currentDate
      );

      newRelationships.AddRange(results);
      pairsCreated += results.Count;
    }

    return newRelationships;
  }

  #endregion
}
