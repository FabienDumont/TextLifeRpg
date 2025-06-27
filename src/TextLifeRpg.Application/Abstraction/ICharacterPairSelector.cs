using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

public interface ICharacterPairSelector
{
  #region Methods

  IEnumerable<(Character, Character)> SelectPairs(List<Character> characters);

  public IEnumerable<(Character, Character)> SelectPairs(
    List<Character> characters, Func<Character, Character, bool> predicate
  );

  #endregion
}
