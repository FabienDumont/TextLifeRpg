using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Services;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Services;

public class RelationshipServiceTests
{
  #region Fields

  private readonly ICharacterService _characterService;

  private readonly IRelationshipRule _generator1;
  private readonly IRelationshipRule _generator2;
  private readonly IRandomProvider _randomProvider = A.Fake<IRandomProvider>();
  private readonly RelationshipService _relationshipService;

  #endregion

  #region Ctors

  public RelationshipServiceTests()
  {
    _generator1 = A.Fake<IRelationshipRule>();
    _generator2 = A.Fake<IRelationshipRule>();
    var generators = new List<IRelationshipRule> {_generator1, _generator2};
    _characterService = A.Fake<ICharacterService>();
    _relationshipService = new RelationshipService(generators, _randomProvider, _characterService);
  }

  #endregion

  #region Methods

  [Fact]
  public void GenerateRelationships_ShouldCallAllGeneratorsAndReturnCollectedRelationships()
  {
    // Arrange

    var character = new CharacterBuilder().WithBirthDate(new DateOnly(1990, 1, 1)).Build();
    var characters = new List<Character> {character};
    var now = new DateOnly(2025, 1, 1);

    A.CallTo(() => _generator1.Generate(A<List<Character>>._, A<List<Relationship>>._, now))
      .ReturnsLazily((List<Character> _, List<Relationship> _, DateOnly _) =>
        [
          Relationship.Create(character.Id, character.Id, RelationshipType.Friend, now, now, 50),
          Relationship.Create(character.Id, character.Id, RelationshipType.Child, now, now, 50)
        ]
      );

    A.CallTo(() => _generator2.Generate(A<List<Character>>._, A<List<Relationship>>._, now))
      .ReturnsLazily((List<Character> _, List<Relationship> _, DateOnly _) =>
        [
          Relationship.Create(character.Id, character.Id, RelationshipType.Enemy, now, now, -50),
          Relationship.Create(character.Id, character.Id, RelationshipType.Parent, now, now, -50)
        ]
      );

    // Act
    var result = _relationshipService.GenerateRelationships(characters, now);

    // Assert
    A.CallTo(() => _generator1.Generate(A<List<Character>>._, A<List<Relationship>>._, now)).MustHaveHappened();

    A.CallTo(() => _generator2.Generate(A<List<Character>>._, A<List<Relationship>>._, now)).MustHaveHappened();

    Assert.Contains(result, r => r.Type == RelationshipType.Friend);
    Assert.Contains(result, r => r.Type == RelationshipType.Enemy);
    Assert.Equal(4, result.Count);
  }

  [Fact]
  public async Task GenerateChildrenFromCouplesAsync_ShouldGenerateChildrenAndRelationships()
  {
    // Arrange
    var parentA = new CharacterBuilder().WithBirthDate(new DateOnly(1980, 1, 1)).WithSex(BiologicalSex.Male).Build();
    var parentB = new CharacterBuilder().WithBirthDate(new DateOnly(1982, 1, 1)).WithSex(BiologicalSex.Female).Build();

    var child = new CharacterBuilder().WithBirthDate(new DateOnly(2010, 1, 1)).Build();

    var currentDate = new DateOnly(2025, 6, 1);

    A.CallTo(() => _randomProvider.Next(0, 3)).Returns(1); // one child
    A.CallTo(() => _characterService.CreateChildAsync(A<Character>._, A<Character>._, currentDate)).Returns(child);

    // Act
    var (children, relationships) =
      await _relationshipService.GenerateChildrenFromCouplesAsync([(parentA, parentB)], currentDate);

    // Assert
    Assert.Single(children);
    Assert.Equal(child.Id, children[0].Id);

    var parentToChild = relationships.Count(r => r.Type == RelationshipType.Parent && r.TargetCharacterId == child.Id);
    var childToParent = relationships.Count(r => r.Type == RelationshipType.Child && r.SourceCharacterId == child.Id);

    Assert.Equal(2, parentToChild);
    Assert.Equal(2, childToParent);
    Assert.DoesNotContain(relationships, r => r.Type == RelationshipType.Sibling);
  }

  [Fact]
  public async Task GenerateChildrenFromCouplesAsync_ShouldCreateSiblingRelationships_WhenMultipleChildren()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 1);
    var mother = new CharacterBuilder().WithBirthDate(new DateOnly(1980, 1, 1)).WithSex(BiologicalSex.Female).Build();
    var father = new CharacterBuilder().WithBirthDate(new DateOnly(1982, 1, 1)).WithSex(BiologicalSex.Male).Build();
    var couples = new List<(Character, Character)> {(mother, father)};

    var child1 = new CharacterBuilder().WithBirthDate(new DateOnly(2015, 1, 1)).Build();
    var child2 = new CharacterBuilder().WithBirthDate(new DateOnly(2017, 1, 1)).Build();

    A.CallTo(() => _randomProvider.Next(0, 3)).Returns(2);
    A.CallTo(() => _characterService.CreateChildAsync(mother, father, now)).ReturnsNextFromSequence(child1, child2);

    // Act
    var (children, relationships) =
      await _relationshipService.GenerateChildrenFromCouplesAsync(couples, now);

    // Assert
    Assert.Equal(2, children.Count);
    Assert.Contains(children, c => c.Id == child1.Id);
    Assert.Contains(children, c => c.Id == child2.Id);

    var siblingRelations = relationships.Where(r => r.Type == RelationshipType.Sibling).ToList();

    Assert.Equal(2, siblingRelations.Count); // reciprocal
    Assert.Contains(siblingRelations, r => r.SourceCharacterId == child1.Id && r.TargetCharacterId == child2.Id);
    Assert.Contains(siblingRelations, r => r.SourceCharacterId == child2.Id && r.TargetCharacterId == child1.Id);
  }

  #endregion
}
