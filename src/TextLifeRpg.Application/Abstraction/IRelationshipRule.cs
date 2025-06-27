using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

public interface IRelationshipRule
{
  #region Methods

  List<Relationship> Generate(
    List<Character> characters, List<Relationship> existingRelationships, DateOnly currentDate
  );

  #endregion
}
