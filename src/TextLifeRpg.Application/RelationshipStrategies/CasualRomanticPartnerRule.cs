using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.RelationshipStrategies;

public class CasualRomanticPartnerRule(
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
