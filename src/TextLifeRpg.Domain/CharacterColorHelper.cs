namespace TextLifeRpg.Domain;

/// <summary>
/// Provides helper methods for character-related color display functionality.
/// </summary>
public static class CharacterColorHelper
{
  #region Methods

  /// <summary>
  /// Determines the display color for a character based on their biological sex and player relationship.
  /// </summary>
  /// <param name="character">The character for whom the color key is being determined.</param>
  /// <param name="playerId">The unique identifier of the player character.</param>
  /// <returns>
  /// A <see cref="CharacterColor"/> representing the appropriate color
  /// based on the character's biological sex and whether the character matches the player.
  /// </returns>
  public static CharacterColor GetColorKey(Character character, Guid playerId)
  {
    if (character.Id == playerId) return CharacterColor.Yellow;

    return character.BiologicalSex switch
    {
      BiologicalSex.Male => CharacterColor.Blue,
      BiologicalSex.Female => CharacterColor.Pink,
      _ => CharacterColor.Purple
    };
  }

  #endregion
}
