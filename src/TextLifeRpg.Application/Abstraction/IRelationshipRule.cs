using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Defines a rule for generating relationships between characters in a system.
/// </summary>
public interface IRelationshipRule
{
  #region Methods

  /// <summary>
  /// Generates a list of relationships based on the given characters, existing relationships,
  /// and the current date using specific rule logic.
  /// </summary>
  /// <param name="characters">The list of characters to evaluate for generating relationships.</param>
  /// <param name="existingRelationships">The list of existing relationships that may influence the new relationships generated.</param>
  /// <param name="currentDate">The current date to be considered in the rule's logic for relationship generation.</param>
  /// <returns>A list of newly generated relationships.</returns>
  List<Relationship> Generate(
    List<Character> characters, List<Relationship> existingRelationships, DateOnly currentDate
  );

  #endregion
}
