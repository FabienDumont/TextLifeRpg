using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Randomization;

/// <summary>
/// Represents a class responsible for selecting and shuffling unique unordered pairs of characters from a given list.
/// </summary>
public class RandomPairSelector(IRandomProvider randomProvider) : ICharacterPairSelector
{
  #region Implementation of ICharacterPairSelector

  /// <summary>
  /// Selects all unique unordered pairs of <see cref="Character"/> objects from the provided list and shuffles them.
  /// </summary>
  /// <param name="characters">The list of characters from which to generate unordered pairs.</param>
  /// <returns>A collection of unique unordered pairs of characters in random order.</returns>
  public IEnumerable<(Character, Character)> SelectPairs(List<Character> characters)
  {
    return characters.SelectMany((a, i) => characters.Skip(i + 1).Select(b => (a, b)))
      .OrderBy(_ => randomProvider.Next(0, int.MaxValue));
  }

  #endregion
}
