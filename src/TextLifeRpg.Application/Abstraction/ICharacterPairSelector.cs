using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Interface for selecting pairs of characters from a collection.
/// </summary>
public interface ICharacterPairSelector
{
  #region Methods

  /// Selects pairs of characters from a provided list based on specific rules or logic.
  /// <param name="characters">The list of characters from which pairs will be selected.</param>
  /// <return>A collection of character pairs represented as tuples, where each tuple contains two characters.</return>
  IEnumerable<(Character, Character)> SelectPairs(List<Character> characters);

  #endregion
}
