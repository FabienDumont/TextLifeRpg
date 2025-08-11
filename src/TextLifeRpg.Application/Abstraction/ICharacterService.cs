using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Service interface for managing characters.
/// </summary>
public interface ICharacterService
{
  #region Methods

  /// <summary>
  /// Creates a new character with randomized attributes.
  /// </summary>
  /// <returns>A randomly generated character.</returns>
  Task<Character> CreateRandomCharacterAsync(World world, CancellationToken cancellationToken);

  /// <summary>
  /// Gets the attraction value of a character towards another.
  /// </summary>
  int GetAttractionValue(Character source, Character target, DateOnly gameDate);

  /// <summary>
  /// Creates a child from two characters.
  /// </summary>
  Task<Character> CreateChildAsync(
    Character mother, Character father, World world, CancellationToken cancellationToken
  );

  #endregion
}
