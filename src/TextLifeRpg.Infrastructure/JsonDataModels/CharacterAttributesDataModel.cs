namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
/// JSON data model representing a character's attributes for serialization.
/// </summary>
public class CharacterAttributesDataModel
{
  #region Properties

  public int Intelligence { get; init; }
  public int Strength { get; init; }
  public int Charisma { get; init; }

  #endregion
}
