using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Defines a factory interface for creating relationships between characters.
/// </summary>
public interface IRelationshipFactory
{
  #region Methods

  /// <summary>
  /// Creates a new list of relationships based on the parameters.
  /// </summary>
  /// <param name="existingRelationships">The current list of existing relationships.</param>
  /// <param name="sourceCharacter">The character initiating the relationship.</param>
  /// <param name="targetCharacter">The target character of the relationship.</param>
  /// <param name="type">The type of the relationship being created.</param>
  /// <param name="currentDate">The date on which the relationship is being created.</param>
  /// <returns>A new list of relationships after applying the creation logic.</returns>
  List<Relationship> Create(
    List<Relationship> existingRelationships, Character sourceCharacter, Character targetCharacter,
    RelationshipType type, DateOnly currentDate
  );

  #endregion
}
