namespace TextLifeRpg.Domain;

public static class CharacterColorHelper
{
  #region Methods

  /// <summary>
  ///   Gets the display color for the character name based on biological sex.
  /// </summary>
  /// <param name="character">The character whose name color is determined.</param>
  /// <param name="playerCharacterId">The identifier of the player character.</param>
  /// <returns>A color string used in the UI (e.g. "blue", "pink").</returns>
  public static string GetCharacterColor(Character character, Guid playerCharacterId)
  {
    if (character.Id == playerCharacterId)
    {
      return "yellow";
    }

    return character.BiologicalSex switch
    {
      BiologicalSex.Male => "blue",
      BiologicalSex.Female => "pink",
      _ => "purple"
    };
  }

  #endregion
}
