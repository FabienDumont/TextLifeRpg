using System.ComponentModel.DataAnnotations;

namespace TextLifeRpg.Domain;

/// <summary>
///   Domain class representing the game's configuration settings.
/// </summary>
public class GameSettings
{
  #region Properties

  /// <summary>
  ///   Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  ///   Density of NPC population when starting a new game.
  /// </summary>
  public NpcDensity NpcDensity { get; }

  #endregion

  #region Ctors

  /// <summary>
  ///   Private constructor used internally.
  /// </summary>
  private GameSettings(Guid id, NpcDensity npcDensity)
  {
    Id = id;
    NpcDensity = npcDensity;
  }

  #endregion

  #region Methods

  /// <summary>
  ///   Factory method to load an existing instance from persistence.
  /// </summary>
  public static GameSettings Load(Guid id, NpcDensity npcDensity)
  {
    return new GameSettings(id, npcDensity);
  }

  /// <summary>
  ///   Factory method to create a new instance.
  /// </summary>
  public static GameSettings Create(NpcDensity npcDensity)
  {
    return new GameSettings(Guid.NewGuid(), npcDensity);
  }

  /// <summary>
  ///   Gets the number of NPCs to generate for the current density.
  /// </summary>
  public int GetNpcCount()
  {
    return NpcDensity switch
    {
      NpcDensity.VeryLow => 20,
      NpcDensity.Low => 30,
      NpcDensity.Average => 40,
      NpcDensity.High => 50,
      NpcDensity.VeryHigh => 60,
      _ => 40
    };
  }

  #endregion
}

/// <summary>
///   Enumeration controlling how many NPCs are generated.
/// </summary>
public enum NpcDensity
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
