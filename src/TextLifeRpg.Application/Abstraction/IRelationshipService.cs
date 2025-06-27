using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
///   Service interface for managing relationships.
/// </summary>
public interface IRelationshipService
{
  #region Methods

  /// <summary>
  ///   Generate relationships
  /// </summary>
  List<Relationship> GenerateRelationships(List<Character> characters, DateOnly currentDate);

  /// <summary>
  ///   Generate children from couples.
  /// </summary>
  Task<(List<Character> children, List<Relationship> relationships)> GenerateChildrenFromCouplesAsync(
    List<(Character parentA, Character parentB)> couples, DateOnly currentDate
  );

  #endregion
}
