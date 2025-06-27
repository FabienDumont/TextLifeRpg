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

  #endregion
}
