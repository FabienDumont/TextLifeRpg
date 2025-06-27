using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.RelationshipStrategies;

/// <summary>
/// Represents a rule that generates spouse relationships between characters
/// within the TextLifeRpg application. The rule determines compatibility
/// and commitment between characters based on factors such as attraction
/// values and pre-existing relationships. Implements the IRelationshipRule interface.
/// </summary>
public class SpouseRule(
  IRandomProvider randomProvider, ICharacterPairSelector pairSelector, ICharacterService characterService,
  IRelationshipFactory relationshipFactory
) : IRelationshipRule
{
  #region Implementation of IRelationshipRule

  /// <summary>
  /// Generates a list of new spouse relationships based on provided characters,
  /// existing relationships, and the current date.
  /// </summary>
  /// <param name="characters">The list of characters to evaluate for potential spouse relationships.</param>
  /// <param name="existingRelationships">The list of existing relationships to consider when generating new ones.</param>
  /// <param name="currentDate">The current date, used to evaluate relationship attributes such as attraction.</param>
  /// <returns>A list of newly created spouse relationships.</returns>
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

      var results = relationshipFactory.Create(existingRelationships, a, b, RelationshipType.Spouse, currentDate);

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
