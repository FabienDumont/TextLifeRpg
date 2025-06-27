using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

/// <summary>
/// Service for managing relationships.
/// </summary>
public class RelationshipService(
  IEnumerable<IRelationshipRule> generators, IRandomProvider randomProvider, ICharacterService characterService
) : IRelationshipService
{
  #region Implementation of IRelationshipService

  /// <inheritdoc />
  public List<Relationship> GenerateRelationships(List<Character> characters, DateOnly currentDate)
  {
    var allRelationships = new List<Relationship>();

    foreach (var generator in generators)
    {
      var generatorOutput = generator.Generate(characters, allRelationships.ToList(), currentDate);
      allRelationships.AddRange(generatorOutput);
    }

    return allRelationships;
  }

  /// <inheritdoc />
  public async Task<(List<Character> children, List<Relationship> relationships)> GenerateChildrenFromCouplesAsync(
    List<(Character parentA, Character parentB)> couples, DateOnly currentDate
  )
  {
    var allChildren = new List<Character>();
    var allRelationships = new List<Relationship>();

    var possibleParents = couples.Where(couple =>
      (couple.parentA.BiologicalSex == BiologicalSex.Male && couple.parentB.BiologicalSex == BiologicalSex.Female) ||
      (couple.parentA.BiologicalSex == BiologicalSex.Female && couple.parentB.BiologicalSex == BiologicalSex.Male)
    ).ToList();

    foreach (var (parentA, parentB) in possibleParents)
    {
      var numChildren = randomProvider.Next(0, 3);
      var siblings = new List<Character>();

      for (var i = 0; i < numChildren; i++)
      {
        var mother = parentA.BiologicalSex == BiologicalSex.Female ? parentA : parentB;
        var father = mother == parentA ? parentB : parentA;

        var child = await characterService.CreateChildAsync(mother, father, currentDate);
        allChildren.Add(child);
        siblings.Add(child);

        // Parent → Child and Child → Parent
        allRelationships.AddRange(
          [
            Relationship.Create(parentA.Id, child.Id, RelationshipType.Parent, child.BirthDate, currentDate, 50),
            Relationship.Create(parentB.Id, child.Id, RelationshipType.Parent, child.BirthDate, currentDate, 50),
            Relationship.Create(child.Id, parentA.Id, RelationshipType.Child, child.BirthDate, currentDate, 50),
            Relationship.Create(child.Id, parentB.Id, RelationshipType.Child, child.BirthDate, currentDate, 50)
          ]
        );
      }

      // Add sibling links
      for (var i = 0; i < siblings.Count; i++)
      {
        for (var j = i + 1; j < siblings.Count; j++)
        {
          var sibA = siblings[i];
          var sibB = siblings[j];

          allRelationships.AddRange(
            [
              Relationship.Create(sibA.Id, sibB.Id, RelationshipType.Sibling, sibA.BirthDate, currentDate, 50),
              Relationship.Create(sibB.Id, sibA.Id, RelationshipType.Sibling, sibB.BirthDate, currentDate, 50)
            ]
          );
        }
      }
    }

    return (allChildren, allRelationships);
  }

  #endregion
}
