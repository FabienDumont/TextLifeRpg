using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

public interface IParentChildPairSelector
{
  #region Methods

  IEnumerable<(Character Parent, Character Child)> SelectParentChildPairs(
    List<Character> characters, DateOnly currentDate
  );

  #endregion
}
