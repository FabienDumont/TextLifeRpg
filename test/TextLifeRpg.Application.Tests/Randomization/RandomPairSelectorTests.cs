using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Randomization;
using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Tests.Helpers;

namespace TextLifeRpg.Application.Tests.Randomization;

public class RandomPairSelectorTests
{
  #region Methods

  [Fact]
  public void SelectPairs_ShouldReturnAllUniqueUnorderedPairs_Shuffled()
  {
    // Arrange
    var char1 = new CharacterBuilder().Build();
    var char2 = new CharacterBuilder().Build();
    var char3 = new CharacterBuilder().Build();
    var characters = new List<Character> {char1, char2, char3};

    var randomProvider = A.Fake<IRandomProvider>();

    // Return constant values just to test that OrderBy runs
    A.CallTo(() => randomProvider.Next(0, int.MaxValue)).Returns(42);

    var selector = new RandomPairSelector(randomProvider);

    // Act
    var pairs = selector.SelectPairs(characters).ToList();

    // Assert
    var expectedPairs = new List<(Character, Character)>
    {
      (char1, char2),
      (char1, char3),
      (char2, char3)
    };

    // Check pair content regardless of order
    Assert.Equal(3, pairs.Count);
    foreach (var expected in expectedPairs)
    {
      Assert.Contains(pairs, actual => actual.Item1.Id == expected.Item1.Id && actual.Item2.Id == expected.Item2.Id);
    }

    // Verify that randomness was used
    A.CallTo(() => randomProvider.Next(0, int.MaxValue)).MustHaveHappened(3, Times.Exactly);
  }

  [Fact]
  public void SelectPairs_WithPredicate_ShouldReturnOnlyMatchingPairs_Shuffled()
  {
    // Arrange
    var older = new CharacterBuilder().WithBirthDate(new DateOnly(1980, 1, 1)).Build();
    var younger = new CharacterBuilder().WithBirthDate(new DateOnly(2005, 1, 1)).Build();
    var peer = new CharacterBuilder().WithBirthDate(new DateOnly(1995, 1, 1)).Build();
    var characters = new List<Character> {older, younger, peer};

    var randomProvider = A.Fake<IRandomProvider>();
    A.CallTo(() => randomProvider.Next(0, int.MaxValue)).Returns(42); // Force deterministic ordering

    var selector = new RandomPairSelector(randomProvider);

    // Predicate: Only allow if first is at least 14 years older than second
    bool Predicate(Character a, Character b)
    {
      return a.BirthDate.AddYears(18) <= b.BirthDate;
    }

    // Act
    var pairs = selector.SelectPairs(characters, Predicate).ToList();

    // Assert
    var expectedPairs = new List<(Character, Character)>
    {
      (older, younger)
    };

    Assert.Single(pairs);
    Assert.Equal(expectedPairs[0].Item1.Id, pairs[0].Item1.Id);
    Assert.Equal(expectedPairs[0].Item2.Id, pairs[0].Item2.Id);

    A.CallTo(() => randomProvider.Next(0, int.MaxValue)).MustNotHaveHappened();
  }

  [Fact]
  public void SelectPairs_WithPredicate_ShouldShuffleMatchingPairs()
  {
    // Arrange
    var older = new CharacterBuilder().WithBirthDate(new DateOnly(1970, 1, 1)).Build();
    var mid1 = new CharacterBuilder().WithBirthDate(new DateOnly(1990, 1, 1)).Build();
    var mid2 = new CharacterBuilder().WithBirthDate(new DateOnly(1992, 1, 1)).Build();

    var characters = new List<Character> {older, mid1, mid2};

    var randomProvider = A.Fake<IRandomProvider>();
    A.CallTo(() => randomProvider.Next(0, int.MaxValue)).Returns(42); // deterministic sort

    var selector = new RandomPairSelector(randomProvider);

    // Act
    var pairs = selector.SelectPairs(characters, Predicate).ToList();

    // Assert
    var expectedPairs = new List<(Character, Character)>
    {
      (older, mid1),
      (older, mid2)
    };

    Assert.Equal(2, pairs.Count);
    foreach (var expected in expectedPairs)
    {
      Assert.Contains(pairs, actual => actual.Item1.Id == expected.Item1.Id && actual.Item2.Id == expected.Item2.Id);
    }

    A.CallTo(() => randomProvider.Next(0, int.MaxValue)).MustHaveHappened(2, Times.Exactly);
    return;

    // Predicate: Must be at least 18 years older
    bool Predicate(Character a, Character b)
    {
      return a.BirthDate.AddYears(18) <= b.BirthDate;
    }
  }

  #endregion
}
