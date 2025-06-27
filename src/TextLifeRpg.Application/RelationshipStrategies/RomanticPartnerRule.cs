using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.RelationshipStrategies;

/// <summary>
/// Represents a rule for generating romantic partner relationships between characters.
/// Ensures that relationships are created following specific criteria, such as attraction levels and availability.
/// </summary>
public class RomanticPartnerRule(
  IRandomProvider randomProvider, ICharacterPairSelector pairSelector, ICharacterService characterService,
  IRelationshipFactory relationshipFactory
) : IRelationshipRule
{
  #region Implementation of IRelationshipRule

  /// <summary>
  /// Generates a list of new romantic partner relationships based on the provided characters, existing relationships, and the current date.
  /// </summary>
  /// <param name="characters">The list of all characters available for pairing.</param>
  /// <param name="existingRelationships">The collection of pre-existing relationships among the characters.</param>
  /// <param name="currentDate">The current date used for evaluating relationship creation logic.</param>
  /// <returns>A list of newly created romantic partner relationships.</returns>
  public List<Relationship> Generate(
    List<Character> characters, List<Relationship> existingRelationships, DateOnly currentDate
  )
  {
    List<Relationship> newRelationships = [];
    var maxPairs = characters.Count / 8;
    var pairsCreated = 0;

    var pairs = pairSelector.SelectPairs(characters).OrderBy(_ => randomProvider.Next(0, int.MaxValue)).ToList();

    var committedCharacters = new HashSet<Guid>(
      existingRelationships.Where(r => r.Type is RelationshipType.RomanticPartner or RelationshipType.Spouse)
        .SelectMany(r => new[] {r.SourceCharacterId, r.TargetCharacterId})
    );

    foreach (var (a, b) in pairs)
    {
      if (pairsCreated >= maxPairs)
      {
        break;
      }

      if (committedCharacters.Contains(a.Id) || committedCharacters.Contains(b.Id))
      {
        continue;
      }

      var attraction = characterService.GetAttractionValue(a, b, currentDate);
      if (attraction < 40)
      {
        continue;
      }

      var results = relationshipFactory.Create(
        existingRelationships, a, b, RelationshipType.RomanticPartner, currentDate
      );

      if (results.Count <= 0)
      {
        continue;
      }

      newRelationships.AddRange(results);
      committedCharacters.Add(a.Id);
      committedCharacters.Add(b.Id);
      pairsCreated += results.Count;
    }

    return newRelationships;
  }

  #endregion
}
