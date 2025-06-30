using TextLifeRpg.Domain;

namespace TextLifeRpg.Infrastructure.JsonDataModels;

/// <summary>
/// JSON data model representing a character for serialization.
/// </summary>
public class CharacterDataModel
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// Name of the character.
  /// </summary>
  public string Name { get; init; } = string.Empty;

  /// <summary>
  /// The birthdate of the character.
  /// </summary>
  public DateOnly BirthDate { get; init; }

  /// <summary>
  /// Biological sex of the character.
  /// </summary>
  public BiologicalSex BiologicalSex { get; init; }

  /// <summary>
  /// Height of the character in centimeters.
  /// </summary>
  public int Height { get; init; }

  /// <summary>
  /// Weight of the character in kilograms.
  /// </summary>
  public int Weight { get; init; }

  /// <summary>
  /// Muscle mass of the character in kilograms.
  /// </summary>
  public int MuscleMass { get; init; }

  /// <summary>
  /// Identifiers of traits assigned to the character.
  /// </summary>
  public List<Guid> TraitsId { get; init; } = [];

  /// <summary>
  /// Identifier of the current location (optional).
  /// </summary>
  public Guid? LocationId { get; set; }

  /// <summary>
  /// Identifier of the current room (optional).
  /// </summary>
  public Guid? RoomId { get; set; }

  /// <summary>
  /// Energy of the character.
  /// </summary>
  public int Energy { get; init; }

  /// <summary>
  /// Money of the character.
  /// </summary>
  public int Money { get; init; }

  /// <summary>
  /// Represents the attributes of a character, encapsulating various properties that define the character's capabilities.
  /// </summary>
  public CharacterAttributesDataModel Attributes { get; init; } = new();

  /// <summary>
  /// Identifier of the character's job.
  /// </summary>
  public Guid? JobId { get; set; }

  #endregion
}
