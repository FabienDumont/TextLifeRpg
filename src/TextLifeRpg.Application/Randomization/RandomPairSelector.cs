using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Randomization;

public class RandomPairSelector(IRandomProvider randomProvider) : ICharacterPairSelector
{
  #region Implementation of ICharacterPairSelector

  public IEnumerable<(Character, Character)> SelectPairs(List<Character> characters)
  {
    return characters.SelectMany((a, i) => characters.Skip(i + 1).Select(b => (a, b)))
      .OrderBy(_ => randomProvider.Next(0, int.MaxValue));
  }

  public IEnumerable<(Character, Character)> SelectPairs(
    List<Character> characters, Func<Character, Character, bool> predicate
  )
  {
    return characters.SelectMany((a, i) => characters.Skip(i + 1).Select(b => (a, b)))
      .Where(pair => predicate(pair.a, pair.b)).OrderBy(_ => randomProvider.Next(0, int.MaxValue));
  }

  #endregion
}
