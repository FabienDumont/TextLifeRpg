using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

public interface IRelationshipFactory
{
  #region Methods

  List<Relationship> Create(
    List<Relationship> existingRelationships, Character sourceCharacter, Character targetCharacter,
    RelationshipType type, DateOnly currentDate
  );

  #endregion
}
