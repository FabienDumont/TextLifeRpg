using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.RelationshipStrategies;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.RelationshipStrategies;

public class SpouseRuleTests
{
  #region Fields

  private readonly ICharacterPairSelector _characterPairSelector = A.Fake<ICharacterPairSelector>();

  private readonly ICharacterService _characterService = A.Fake<ICharacterService>();
  private readonly IRandomProvider _randomProvider = A.Fake<IRandomProvider>();
  private readonly IRelationshipFactory _relationshipFactory = A.Fake<IRelationshipFactory>();

  #endregion

  #region Methods

  [Fact]
  public void Generate_ShouldNotExceedMaxPairs()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var chars = Enumerable.Range(0, 16).Select(_ => new CharacterBuilder().Build()).ToList();

    var pairs = new List<(Character, Character)>
    {
      (chars[0], chars[1]),
      (chars[2], chars[3]),
      (chars[4], chars[5]),
      (chars[6], chars[7])
    };

    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns(pairs);
    A.CallTo(() => _characterService.GetAttractionValue(A<Character>._, A<Character>._, now)).Returns(100);
    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).Returns(0);

    A.CallTo(() => _relationshipFactory.Create(
        A<List<Relationship>>._, A<Character>._, A<Character>._, RelationshipType.Spouse, now
      )
    ).ReturnsLazily(call =>
      {
        var x = call.GetArgument<Character>(1) ?? throw new InvalidOperationException("x is null");
        var y = call.GetArgument<Character>(2) ?? throw new InvalidOperationException("y is null");

        return
        [
          Relationship.Create(x.Id, y.Id, RelationshipType.Spouse, now, now, 90),
          Relationship.Create(y.Id, x.Id, RelationshipType.Spouse, now, now, 90)
        ];
      }
    ).NumberOfTimes(1);

    var rule = new SpouseRule(_randomProvider, _characterPairSelector, _characterService, _relationshipFactory);

    // Act
    var result = rule.Generate(chars, [], now);

    // Assert
    Assert.Equal(2, result.Count);
  }

  [Fact]
  public void Generate_ShouldCreateReciprocalRomanticRelationships()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var a = new CharacterBuilder().Build();
    var b = new CharacterBuilder().Build();
    var chars = new List<Character> {a, b}
      .Concat(Enumerable.Range(0, 6).Select(_ => new CharacterBuilder().Build())).ToList(); // 8 total characters

    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns([(a, b)]);
    A.CallTo(() => _characterService.GetAttractionValue(a, b, A<DateOnly>._)).Returns(70);
    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).Returns(0);
    var relA = Relationship.Create(a.Id, b.Id, RelationshipType.Spouse, now, now, 100);
    var relB = Relationship.Create(b.Id, a.Id, RelationshipType.Spouse, now, now, 100);

    A.CallTo(() => _relationshipFactory.Create(A<List<Relationship>>._, a, b, RelationshipType.Spouse, now))
      .Returns([relA, relB]);

    var rule = new SpouseRule(_randomProvider, _characterPairSelector, _characterService, _relationshipFactory);

    // Act
    var result = rule.Generate(chars, [], now);

    // Assert
    Assert.Equal(2, result.Count);
    var relAb = result.Single(r => r.SourceCharacterId == a.Id);
    var relBa = result.Single(r => r.SourceCharacterId == b.Id);

    Assert.Equal(b.Id, relAb.TargetCharacterId);
    Assert.Equal(RelationshipType.Spouse, relAb.Type);

    Assert.Equal(a.Id, relBa.TargetCharacterId);
    Assert.Equal(RelationshipType.Spouse, relBa.Type);
  }

  [Fact]
  public void Generate_ShouldSkipPairs_WhenAttractionTooLow()
  {
    // Arrange
    var a = new CharacterBuilder().Build();
    var b = new CharacterBuilder().Build();
    var chars = new List<Character> {a, b}
      .Concat(Enumerable.Range(0, 6).Select(_ => new CharacterBuilder().Build())).ToList(); // 8 characters total

    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns([(a, b)]);
    A.CallTo(() => _characterService.GetAttractionValue(a, b, A<DateOnly>._)).Returns(20); // below threshold
    var rule = new SpouseRule(_randomProvider, _characterPairSelector, _characterService, _relationshipFactory);

    // Act
    var result = rule.Generate(chars, [], new DateOnly(2025, 6, 16));

    // Assert
    Assert.Empty(result); // should skip this pair
  }

  [Fact]
  public void Generate_ShouldSkipPairs_WhenAlreadyCommitted()
  {
    // Arrange
    var a = new CharacterBuilder().Build();
    var b = new CharacterBuilder().Build();
    var chars = new List<Character> {a, b}
      .Concat(Enumerable.Range(0, 6).Select(_ => new CharacterBuilder().Build())).ToList(); // 8 characters total

    var existing = new List<Relationship>
    {
      Relationship.Create(
        a.Id, Guid.NewGuid(), RelationshipType.RomanticPartner, new DateOnly(2020, 1, 1), new DateOnly(2025, 1, 1), 75
      ),
      Relationship.Create(
        b.Id, Guid.NewGuid(), RelationshipType.Spouse, new DateOnly(2019, 1, 1), new DateOnly(2025, 1, 1), 90
      )
    };

    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns([(a, b)]);
    A.CallTo(() => _characterService.GetAttractionValue(a, b, A<DateOnly>._)).Returns(100); // would normally qualify
    var rule = new SpouseRule(_randomProvider, _characterPairSelector, _characterService, _relationshipFactory);

    // Act
    var result = rule.Generate(chars, existing, new DateOnly(2025, 6, 16));

    // Assert
    Assert.Empty(result); // should skip due to already being committed
  }

  [Fact]
  public void Generate_ShouldSkipSubsequentPairs_WhenCharacterMarriesEarlierInSameRun()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var chars = Enumerable.Range(0, 16).Select(_ => new CharacterBuilder().Build()).ToList();
    var a = chars[0];
    var b = chars[1];
    var c = chars[2];

    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns([(a, b), (a, c)]);
    A.CallTo(() => _characterService.GetAttractionValue(A<Character>._, A<Character>._, now)).Returns(90);
    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).Returns(0);

    A.CallTo(() => _relationshipFactory.Create(
        A<List<Relationship>>._, A<Character>._, A<Character>._, RelationshipType.Spouse, now
      )
    ).ReturnsLazily(call =>
      {
        var x = call.GetArgument<Character>(1) ?? throw new InvalidOperationException("x is null");
        var y = call.GetArgument<Character>(2) ?? throw new InvalidOperationException("y is null");

        return
        [
          Relationship.Create(x.Id, y.Id, RelationshipType.Spouse, now, now, 90),
          Relationship.Create(y.Id, x.Id, RelationshipType.Spouse, now, now, 90)
        ];
      }
    ).NumberOfTimes(1);

    var rule = new SpouseRule(_randomProvider, _characterPairSelector, _characterService, _relationshipFactory);

    // Act
    var result = rule.Generate(chars, [], now);

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Contains(result, r => r.SourceCharacterId == a.Id && r.TargetCharacterId == b.Id);
    Assert.Contains(result, r => r.SourceCharacterId == b.Id && r.TargetCharacterId == a.Id);
  }

  [Fact]
  public void Generate_ShouldSkipPair_WhenFactoryReturnsNoRelationships()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var chars = Enumerable.Range(0, 8).Select(_ => new CharacterBuilder().Build()).ToList();
    var a = chars[0];
    var b = chars[1];

    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns([(a, b)]);
    A.CallTo(() => _characterService.GetAttractionValue(a, b, now)).Returns(90);
    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).Returns(0);

    A.CallTo(() => _relationshipFactory.Create(A<List<Relationship>>._, a, b, RelationshipType.Spouse, now))
      .Returns([]); // triggers results.Count <= 0

    var rule = new SpouseRule(_randomProvider, _characterPairSelector, _characterService, _relationshipFactory);

    // Act
    var result = rule.Generate(chars, [], now);

    // Assert
    Assert.Empty(result);
  }

  #endregion
}
