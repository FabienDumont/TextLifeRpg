using System.ComponentModel.DataAnnotations;

namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing a character in the game.
/// </summary>
public class Character
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  /// Name of the character.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Birthdate of the character.
  /// </summary>
  public DateOnly BirthDate { get; }

  /// <summary>
  /// Biological sex of the character.
  /// </summary>
  public BiologicalSex BiologicalSex { get; }

  /// <summary>
  /// Height in centimeters of the character.
  /// </summary>
  public int Height { get; }

  /// <summary>
  /// Weight in kilograms of the character.
  /// </summary>
  public int Weight { get; }

  /// <summary>
  /// Muscle mass in kilograms (fat-free lean mass) of the character.
  /// </summary>
  public int MuscleMass { get; private set; }

  /// <summary>
  /// List of trait identifiers assigned to the character.
  /// </summary>
  public List<Guid> TraitsId { get; } = [];

  /// <summary>
  /// Identifier of the character's current location (optional).
  /// </summary>
  public Guid? LocationId { get; private set; }

  /// <summary>
  /// Identifier of the character's current room (optional).
  /// </summary>
  public Guid? RoomId { get; private set; }

  /// <summary>
  /// Energy of the character.
  /// </summary>
  public int Energy { get; set; } = 100;

  /// <summary>
  /// Money of the character.
  /// </summary>
  public int Money { get; set; }

  /// <summary>
  /// Represents the character's attributes such as intelligence, strength, and charisma.
  /// </summary>
  public CharacterAttributes Attributes { get; private set; }

  /// <summary>
  /// Represents the character's job.
  /// </summary>
  public Guid? JobId { get; set; }

  /// <summary>
  /// The character's inventory entries.
  /// </summary>
  public List<InventoryEntry> InventoryEntries { get; } = [];

  /// <summary>
  /// The character's phone.
  /// </summary>
  public Phone Phone { get; private init; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private Character(
    Guid id, string name, DateOnly birthDate, BiologicalSex biologicalSex, int height, int weight, int muscleMass,
    CharacterAttributes attributes
  )
  {
    Id = id;
    Name = name;
    BirthDate = birthDate;
    BiologicalSex = biologicalSex;
    Height = height;
    Weight = weight;
    MuscleMass = muscleMass;
    Attributes = attributes;
    Phone = Phone.Create();
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static Character Create(
    string name, DateOnly birthDate, BiologicalSex biologicalSex, int height, int weight, int muscleMass,
    CharacterAttributes attributes
  )
  {
    return new Character(Guid.NewGuid(), name, birthDate, biologicalSex, height, weight, muscleMass, attributes);
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static Character Load(
    Guid id, string name, DateOnly birthDate, BiologicalSex biologicalSex, int height, int weight, int muscleMass,
    CharacterAttributes attributes
  )
  {
    return new Character(id, name, birthDate, biologicalSex, height, weight, muscleMass, attributes);
  }

  /// <summary>
  /// Adds traits to the character.
  /// </summary>
  public void AddTraits(IEnumerable<Guid> traitIds)
  {
    TraitsId.AddRange(traitIds);
  }

  /// <summary>
  /// Moves the character to a new location and room.
  /// </summary>
  public void MoveTo(Guid? locationId, Guid? roomId)
  {
    LocationId = locationId;
    RoomId = roomId;
  }

  /// <summary>
  /// Calculates the age of the character based on the specified game date and the character's birth date.
  /// </summary>
  /// <param name="gameDate">The date in the game on which the age is to be calculated.</param>
  /// <returns>The age of the character on the specified game date.</returns>
  public int GetAge(DateOnly gameDate)
  {
    var age = gameDate.Year - BirthDate.Year;
    if (gameDate < BirthDate.AddYears(age))
    {
      age--;
    }

    return age;
  }

  /// <summary>
  /// Assigns a job to the character by setting the job ID.
  /// </summary>
  /// <param name="jobId">The unique identifier of the job to assign to the character.</param>
  public void SetJob(Guid jobId)
  {
    JobId = jobId;
  }

  /// <summary>
  /// Adds inventory entries to the character's inventory.
  /// </summary>
  public void AddInventoryEntries(IEnumerable<InventoryEntry> inventoryEntries)
  {
    InventoryEntries.AddRange(inventoryEntries);
  }

  /// <summary>
  /// Adds an item to the character's inventory or updates the quantity if the item already exists in the inventory.
  /// </summary>
  /// <param name="itemId">The unique identifier of the item to be added to the inventory.</param>
  /// <param name="quantity">The quantity of the item to be added.</param>
  public void AddItemToInventory(Guid itemId, int quantity)
  {
    var entry = InventoryEntries.FirstOrDefault(i => i.ItemId == itemId);
    if (entry != null)
    {
      entry.Add(quantity);
    }
    else
    {
      InventoryEntries.Add(InventoryEntry.Create(itemId, quantity));
    }
  }

  /// <summary>
  /// Removes a specified quantity of an item from the character's inventory.
  /// If the item's quantity reaches zero, the item is removed from the inventory.
  /// </summary>
  /// <param name="itemId">The unique identifier of the item to be removed.</param>
  /// <param name="quantity">The amount of the item to be removed from the inventory.</param>
  public void RemoveItemFromInventory(Guid itemId, int quantity)
  {
    var entry = InventoryEntries.FirstOrDefault(i => i.ItemId == itemId);
    if (entry == null)
    {
      return;
    }

    entry.Remove(quantity);
    if (entry.Quantity == 0)
    {
      InventoryEntries.Remove(entry);
    }
  }

  #endregion
}

/// <summary>
/// Enumeration for biological sex.
/// </summary>
public enum BiologicalSex
{
  Male,
  Female
}

/// <summary>
/// Enumeration representing different levels of muscle mass.
/// </summary>
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

public enum CharacterColor
{
  Yellow,
  Blue,
  Pink,
  Purple
}
