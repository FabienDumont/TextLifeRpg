using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Factories;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Factories;

public class RelationshipFactoryTests
{
  #region Fields

  private readonly IRandomProvider _randomProvider = A.Fake<IRandomProvider>();
  private readonly RelationshipFactory _factory;

  #endregion

  #region Ctors

  public RelationshipFactoryTests()
  {
    _factory = new RelationshipFactory(_randomProvider);
  }

  #endregion

  #region Methods

  [Fact]
  public void Create_ShouldReturnEmpty_WhenRelationshipAlreadyExists()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var a = new CharacterBuilder().Build();
    var b = new CharacterBuilder().Build();
    var existing = new List<Relationship>
    {
      Relationship.Create(a.Id, b.Id, RelationshipType.Friend, now, now, 50)
    };

    // Act
    var result = _factory.Create(existing, a, b, RelationshipType.Friend, now);

    // Assert
    Assert.Empty(result);
  }

  [Fact]
  public void Create_ShouldGenerateReciprocal_FriendRelationships()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var a = new CharacterBuilder().WithBirthDate(new DateOnly(2000, 1, 1)).Build();
    var b = new CharacterBuilder().WithBirthDate(new DateOnly(2001, 1, 1)).Build();

    // rng: firstInteraction offset, lastInteraction offset, friendship value
    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).ReturnsNextFromSequence(0, 0, 75);

    // Act
    var rels = _factory.Create([], a, b, RelationshipType.Friend, now);

    // Assert
    Assert.Equal(2, rels.Count);
    var ab = rels.Single(r => r.SourceCharacterId == a.Id);
    var ba = rels.Single(r => r.SourceCharacterId == b.Id);

    Assert.Equal(RelationshipType.Friend, ab.Type);
    Assert.Equal(75, ab.Value);
    Assert.Equal(ab.History.FirstInteraction, ba.History.FirstInteraction);
    Assert.Equal(ab.History.LastInteraction, ba.History.LastInteraction);
  }

  [Fact]
  public void Create_ShouldGenerateEnemyRelationships_WithNegativeValue()
  {
    // Arrange
    var now = new DateOnly(2025, 1, 1);
    var a = new CharacterBuilder().WithBirthDate(new DateOnly(2002, 1, 1)).Build();
    var b = new CharacterBuilder().WithBirthDate(new DateOnly(2003, 1, 1)).Build();

    // rng: firstInt offset, lastInt offset, enemy value
    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).ReturnsNextFromSequence(1, 2, -60);

    // Act
    var rels = _factory.Create([], a, b, RelationshipType.Enemy, now);

    // Assert
    Assert.Equal(2, rels.Count);
    Assert.All(rels, r => Assert.Equal(RelationshipType.Enemy, r.Type));
    Assert.All(rels, r => Assert.InRange(r.Value, -100, -40));
  }

  [Theory]
  [InlineData(RelationshipType.Parent, RelationshipType.Child)]
  [InlineData(RelationshipType.Child, RelationshipType.Parent)]
  [InlineData(RelationshipType.Grandparent, RelationshipType.Grandchild)]
  [InlineData(RelationshipType.Grandchild, RelationshipType.Grandparent)]
  [InlineData(RelationshipType.Friend, RelationshipType.Friend)]
  public void GetReciprocal_ShouldReturnExpected(RelationshipType input, RelationshipType expected)
  {
    // Act
    var result = RelationshipFactory.GetReciprocal(input);

    // Assert
    Assert.Equal(expected, result);
  }

  [Fact]
  public void Create_ShouldSetFirstInteraction_ToLaterBirth_ForParentType()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var parent = new CharacterBuilder().WithBirthDate(new DateOnly(1990, 1, 1)).Build();
    var child = new CharacterBuilder().WithBirthDate(new DateOnly(2015, 1, 1)).Build();

    // Act
    var rels = _factory.Create([], parent, child, RelationshipType.Parent, now);

    // Assert
    var parentRel = rels.Single(r => r.SourceCharacterId == parent.Id);
    Assert.Equal(child.BirthDate, parentRel.History.FirstInteraction);
    Assert.True(parentRel.History.FirstInteraction <= parentRel.History.LastInteraction);
  }

  [Fact]
  public void Create_ShouldUseFallbackRange_ForUnrecognizedType()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var a = new CharacterBuilder().WithBirthDate(new DateOnly(1995, 1, 1)).Build();
    var b = new CharacterBuilder().WithBirthDate(new DateOnly(1996, 1, 1)).Build();

    // rng: firstInteraction offset, lastInteraction offset, fallback value
    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).ReturnsNextFromSequence(1, 2, 10);

    // Act
    var rels = _factory.Create([], a, b, RelationshipType.Acquaintance, now);

    // Assert
    Assert.Equal(2, rels.Count);
    Assert.All(rels, r => Assert.Equal(RelationshipType.Acquaintance, r.Type));
    Assert.All(rels, r => Assert.Equal(10, r.Value));
  }

  [Fact]
  public void Create_ShouldGenerateRomanticPartnerRelationship_WithCorrectValueRange()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var a = new CharacterBuilder().WithBirthDate(new DateOnly(1995, 1, 1)).Build();
    var b = new CharacterBuilder().WithBirthDate(new DateOnly(1996, 1, 1)).Build();

    // rng: firstInteraction offset, lastInteraction offset, romantic value
    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).ReturnsNextFromSequence(1, 2, 33);

    // Act
    var rels = _factory.Create([], a, b, RelationshipType.RomanticPartner, now);

    // Assert
    Assert.Equal(2, rels.Count);
    Assert.All(rels, r => Assert.Equal(RelationshipType.RomanticPartner, r.Type));
    Assert.All(rels, r => Assert.Equal(33, r.Value));
  }


  #endregion
}
