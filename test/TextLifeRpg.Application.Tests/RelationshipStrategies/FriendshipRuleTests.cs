using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.RelationshipStrategies;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.RelationshipStrategies;

public class FriendshipRuleTests
{
  #region Fields

  private readonly ICharacterPairSelector _characterPairSelector = A.Fake<ICharacterPairSelector>();

  private readonly IRandomProvider _randomProvider = A.Fake<IRandomProvider>();
  private readonly IRelationshipFactory _relationshipFactory = A.Fake<IRelationshipFactory>();

  #endregion

  #region Methods

  [Fact]
  public void Generate_ShouldReturnEmpty_WhenLessThanFiveCharacters()
  {
    // Arrange
    var chars = Enumerable.Range(1, 4).Select(_ => new CharacterBuilder().Build()).ToList();
    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns([]);
    var rule = new FriendshipRule(_randomProvider, _characterPairSelector, _relationshipFactory);

    // Act
    var result = rule.Generate(chars, [], new DateOnly(2025, 6, 16));

    // Assert
    Assert.Empty(result);
  }

  [Fact]
  public void Generate_ShouldNotExceedMaxPairs()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var chars = Enumerable.Range(1, 10).Select(_ => new CharacterBuilder().Build()).ToList();
    var pairs = new List<(Character, Character)>
    {
      (chars[0], chars[1]),
      (chars[2], chars[3]),
      (chars[4], chars[5]),
      (chars[6], chars[7])
    };
    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns(pairs);

    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).Returns(0);

    A.CallTo(() => _relationshipFactory.Create(
        A<List<Relationship>>._, A<Character>._, A<Character>._, RelationshipType.Friend, now
      )
    ).ReturnsLazily(call =>
      {
        var x = call.GetArgument<Character>(1) ?? throw new InvalidOperationException("x is null");
        var y = call.GetArgument<Character>(2) ?? throw new InvalidOperationException("y is null");

        return
        [
          Relationship.Create(x.Id, y.Id, RelationshipType.Friend, now, now, 90),
          Relationship.Create(y.Id, x.Id, RelationshipType.Friend, now, now, 90)
        ];
      }
    ).NumberOfTimes(1);

    var rule = new FriendshipRule(_randomProvider, _characterPairSelector, _relationshipFactory);

    // Act
    var result = rule.Generate(chars, [], now);

    // Assert
    var expectedCount = chars.Count / 5;
    Assert.Equal(expectedCount, result.Count);
  }

  [Fact]
  public void Generate_ShouldCreateReciprocalFriendRelationships()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var a = new CharacterBuilder().Build();
    var b = new CharacterBuilder().Build();
    var chars = new List<Character> {a, b}
      .Concat(Enumerable.Range(0, 3).Select(_ => new CharacterBuilder().Build())).ToList();
    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns([(a, b)]);

    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).Returns(0);
    var relA = Relationship.Create(a.Id, b.Id, RelationshipType.Friend, now, now, 100);
    var relB = Relationship.Create(b.Id, a.Id, RelationshipType.Friend, now, now, 100);

    A.CallTo(() => _relationshipFactory.Create(A<List<Relationship>>._, a, b, RelationshipType.Friend, now))
      .Returns([relA, relB]);

    var rule = new FriendshipRule(_randomProvider, _characterPairSelector, _relationshipFactory);

    // Act
    var result = rule.Generate(chars, [], now);

    // Assert
    Assert.Equal(2, result.Count);
    var relAb = result.Single(r => r.SourceCharacterId == a.Id);
    var relBa = result.Single(r => r.SourceCharacterId == b.Id);

    Assert.Equal(b.Id, relAb.TargetCharacterId);
    Assert.Equal(RelationshipType.Friend, relAb.Type);

    Assert.Equal(a.Id, relBa.TargetCharacterId);
    Assert.Equal(RelationshipType.Friend, relBa.Type);
  }

  #endregion
}
