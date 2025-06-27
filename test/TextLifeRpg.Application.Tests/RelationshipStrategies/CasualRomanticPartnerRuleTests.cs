using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.RelationshipStrategies;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.RelationshipStrategies;

public class CasualRomanticPartnerRuleTests
{
  #region Fields

  private readonly ICharacterPairSelector _characterPairSelector = A.Fake<ICharacterPairSelector>();

  private readonly ICharacterService _characterService = A.Fake<ICharacterService>();
  private readonly IRandomProvider _randomProvider = A.Fake<IRandomProvider>();
  private readonly IRelationshipFactory _relationshipFactory = A.Fake<IRelationshipFactory>();

  #endregion

  #region Methods

  [Fact]
  public void Generate_ShouldReturnEmpty_WhenLessThanEightCharacters()
  {
    // Arrange
    var chars = Enumerable.Range(1, 7).Select(_ => new CharacterBuilder().Build()).ToList();
    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns([]);
    var rule = new CasualRomanticPartnerRule(
      _randomProvider, _characterPairSelector, _characterService, _relationshipFactory
    );

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
    var chars = Enumerable.Range(1, 16).Select(_ => new CharacterBuilder().Build()).ToList(); // maxPairs = 2
    var pairs = new List<(Character, Character)>
    {
      (chars[0], chars[1]),
      (chars[2], chars[3]),
      (chars[4], chars[5]),
      (chars[6], chars[7])
    };
    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns(pairs);
    A.CallTo(() => _characterService.GetAttractionValue(A<Character>._, A<Character>._, A<DateOnly>._)).Returns(100);

    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).Returns(0);

    A.CallTo(() => _relationshipFactory.Create(
        A<List<Relationship>>._, A<Character>._, A<Character>._, RelationshipType.CasualRomanticPartner, now
      )
    ).ReturnsLazily(call =>
      {
        var x = call.GetArgument<Character>(1) ?? throw new InvalidOperationException("x is null");
        var y = call.GetArgument<Character>(2) ?? throw new InvalidOperationException("y is null");

        return
        [
          Relationship.Create(x.Id, y.Id, RelationshipType.CasualRomanticPartner, now, now, 90),
          Relationship.Create(y.Id, x.Id, RelationshipType.CasualRomanticPartner, now, now, 90)
        ];
      }
    ).NumberOfTimes(1);

    var rule = new CasualRomanticPartnerRule(
      _randomProvider, _characterPairSelector, _characterService, _relationshipFactory
    );

    // Act
    var result = rule.Generate(chars, [], now);

    // Assert
    var expectedCount = chars.Count / 8;
    Assert.Equal(expectedCount, result.Count);
  }

  [Fact]
  public void Generate_ShouldCreateReciprocalCasualRomanticRelationships()
  {
    // Arrange
    var now = new DateOnly(2025, 6, 16);
    var a = new CharacterBuilder().Build();
    var b = new CharacterBuilder().Build();
    var chars = new List<Character> {a, b}
      .Concat(Enumerable.Range(0, 6).Select(_ => new CharacterBuilder().Build())).ToList(); // 8 total characters

    A.CallTo(() => _characterPairSelector.SelectPairs(chars)).Returns([(a, b)]);
    A.CallTo(() => _characterService.GetAttractionValue(a, b, A<DateOnly>._)).Returns(70); // passes 40-threshold
    A.CallTo(() => _randomProvider.Next(A<int>._, A<int>._)).Returns(0);

    var relA = Relationship.Create(a.Id, b.Id, RelationshipType.CasualRomanticPartner, now, now, 50);
    var relB = Relationship.Create(b.Id, a.Id, RelationshipType.CasualRomanticPartner, now, now, 50);

    A.CallTo(() => _relationshipFactory.Create(
        A<List<Relationship>>._, a, b, RelationshipType.CasualRomanticPartner, now
      )
    ).Returns([relA, relB]);

    var rule = new CasualRomanticPartnerRule(
      _randomProvider, _characterPairSelector, _characterService, _relationshipFactory
    );

    // Act
    var result = rule.Generate(chars, [], now);

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Contains(relA, result);
    Assert.Contains(relB, result);
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
    var rule = new CasualRomanticPartnerRule(
      _randomProvider, _characterPairSelector, _characterService, _relationshipFactory
    );

    // Act
    var result = rule.Generate(chars, [], new DateOnly(2025, 6, 16));

    // Assert
    Assert.Empty(result); // should skip this pair
  }

  #endregion
}
