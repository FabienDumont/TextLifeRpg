using TextLifeRpg.Application.InternalUtilities;
using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
///
/// </summary>
public static class CharacterAttributesMapper
{
  #region Methods

  /// <summary>
  /// Maps a JSON data model to its domain counterpart.
  /// </summary>
  public static CharacterAttributes ToDomain(this CharacterAttributesDataModel dataModel)
  {
    return CharacterAttributes.Create(dataModel.Intelligence, dataModel.Strength, dataModel.Charisma);
    ;
  }

  /// <summary>
  /// Maps a domain model to its JSON data model counterpart.
  /// </summary>
  public static CharacterAttributesDataModel ToDataModel(this CharacterAttributes domain)
  {
    return domain.Map(u => new CharacterAttributesDataModel
      {
        Intelligence = u.Intelligence,
        Strength = u.Strength,
        Charisma = u.Charisma
      }
    );
  }

  #endregion
}
