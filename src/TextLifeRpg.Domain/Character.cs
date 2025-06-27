using System.ComponentModel.DataAnnotations;

namespace TextLifeRpg.Domain;

/// <summary>
///   Domain class representing a character in the game.
/// </summary>
public class Character
{
  #region Properties

  /// <summary>
  ///   Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  ///   Name of the character.
  /// </summary>
  public string Name { get; }

  /// <summary>
  ///   Birthdate of the character.
  /// </summary>
  public DateOnly BirthDate { get; }

  /// <summary>
  ///   Biological sex of the character.
  /// </summary>
  public BiologicalSex BiologicalSex { get; }

  /// <summary>
  ///   Height in centimeters of the character.
  /// </summary>
  public int Height { get; }

  /// <summary>
  ///   Weight in kilograms of the character.
  /// </summary>
  public int Weight { get; }

  /// <summary>
  ///   Muscle mass in kilograms (fat-free lean mass) of the character.
  /// </summary>
  public int MuscleMass { get; private set; }

  /// <summary>
  ///   List of trait identifiers assigned to the character.
  /// </summary>
  public List<Guid> TraitsId { get; } = [];

  /// <summary>
  ///   Identifier of the character's current location (optional).
  /// </summary>
  public Guid? LocationId { get; private set; }

  /// <summary>
  ///   Identifier of the character's current room (optional).
  /// </summary>
  public Guid? RoomId { get; private set; }

  /// <summary>
  ///   Energy of the character.
  /// </summary>
  public int Energy { get; set; } = 100;

  /// <summary>
  ///   Money of the character.
  /// </summary>
  public int Money { get; set; }

  #endregion

  #region Ctors

  /// <summary>
  ///   Private constructor used internally.
  /// </summary>
  private Character(
    Guid id, string name, DateOnly birthDate, BiologicalSex biologicalSex, int height, int weight, int muscleMass
  )
  {
    Id = id;
    Name = name;
    BirthDate = birthDate;
    BiologicalSex = biologicalSex;
    Height = height;
    Weight = weight;
    MuscleMass = muscleMass;
  }

  #endregion

  #region Methods

  /// <summary>
  ///   Factory method to create a new instance.
  /// </summary>
  public static Character Create(
    string name, DateOnly birthDate, BiologicalSex biologicalSex, int height, int weight, int muscleMass
  )
  {
    return new Character(Guid.NewGuid(), name, birthDate, biologicalSex, height, weight, muscleMass);
  }

  /// <summary>
  ///   Factory method to load an existing instance from persistence.
  /// </summary>
  public static Character Load(
    Guid id, string name, DateOnly birthDate, BiologicalSex biologicalSex, int height, int weight, int muscleMass
  )
  {
    return new Character(id, name, birthDate, biologicalSex, height, weight, muscleMass);
  }

  /// <summary>
  ///   Adds traits to the character.
  /// </summary>
  public void AddTraits(IEnumerable<Guid> traitIds)
  {
    TraitsId.AddRange(traitIds);
  }

  /// <summary>
  ///   Moves the character to a new location and room.
  /// </summary>
  public void MoveTo(Guid? locationId, Guid? roomId)
  {
    LocationId = locationId;
    RoomId = roomId;
  }

  public int GetAge(DateOnly gameDate)
  {
    var age = gameDate.Year - BirthDate.Year;
    if (gameDate < BirthDate.AddYears(age))
    {
      age--;
    }

    return age;
  }

  #endregion
}

/// <summary>
///   Enumeration for biological sex.
/// </summary>
public enum BiologicalSex
{
  Male,
  Female
}

public enum MuscleMassOption
{
  [Display(Name = "Very low")]
  VeryLow,

  [Display(Name = "Low")]
  Low,

  [Display(Name = "Average")]
  Average,

  [Display(Name = "High")]
  High,

  [Display(Name = "Very high")]
  VeryHigh
}
