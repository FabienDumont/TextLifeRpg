using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.RelationshipStrategies;

public class RomanticPartnerRule(
  IRandomProvider randomProvider, ICharacterPairSelector pairSelector, ICharacterService characterService,
  IRelationshipFactory relationshipFactory
) : IRelationshipRule
{
  #region Implementation of IRelationshipRule

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
