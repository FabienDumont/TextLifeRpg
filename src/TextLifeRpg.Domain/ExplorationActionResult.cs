namespace TextLifeRpg.Domain;

/// <summary>
///   Domain class representing an exploration action result.
/// </summary>
public class ExplorationActionResult
{
  #region Properties

  /// <summary>
  ///   Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  ///   Exploration action identifier.
  /// </summary>
  public Guid ExplorationActionId { get; }

  /// <summary>
  ///   Flag should the required minutes of the exploration action be added.
  /// </summary>
  public bool AddMinutes { get; set; }

  /// <summary>
  ///   Change in energy after performing the action.
  /// </summary>
  public int? EnergyChange { get; }

  /// <summary>
  ///   Change in money after performing the action.
  /// </summary>
  public int? MoneyChange { get; }

  #endregion

  #region Ctors

  /// <summary>
  ///   Private constructor used internally.
  /// </summary>
  private ExplorationActionResult(
    Guid id, Guid explorationActionId, bool addMinutes, int? energyChange, int? moneyChange
  )
  {
    Id = id;
    ExplorationActionId = explorationActionId;
    AddMinutes = addMinutes;
    EnergyChange = energyChange;
    MoneyChange = moneyChange;
  }

  #endregion

  #region Methods

  /// <summary>
  ///   Factory method to create a new instance.
  /// </summary>
  public static ExplorationActionResult Create(
    Guid explorationActionId, bool addMinutes, int? energyChange, int? moneyChange
  )
  {
    return new ExplorationActionResult(Guid.NewGuid(), explorationActionId, addMinutes, energyChange, moneyChange);
  }

  /// <summary>
  ///   Factory method to load an existing instance from persistence.
  /// </summary>
  public static ExplorationActionResult Load(
    Guid id, Guid explorationActionId, bool addMinutes, int? energyChange, int? moneyChange
  )
  {
    return new ExplorationActionResult(id, explorationActionId, addMinutes, energyChange, moneyChange);
  }

  #endregion
}
