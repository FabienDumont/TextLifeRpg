namespace TextLifeRpg.Infrastructure.JsonDefinitions;

public class ExplorationActionDefinition
{
  public required string Label { get; set; }
  public required string LocationName { get; set; }
  public required string RoomName { get; set; }
  public required int NeededMinutes { get; set; }
  public List<ExplorationActionResultDefinition> Results { get; set; } = [];
}

/// <summary>
/// Defines the result of an exploration action within the game. Represents the potential
/// outcomes or effects of performing an action in a specific exploration scenario.
/// </summary>
public class ExplorationActionResultDefinition
{
  public required bool AddMinutes { get; set; }
  public int? EnergyChange { get; set; }
  public List<ExplorationActionResultNarrationDefinition> ResultNarrations { get; set; } = [];
}

public class ExplorationActionResultNarrationDefinition
{
  public required string Text { get; set; }
  public List<ExplorationActionConditionDefinition> Conditions { get; set; } = [];
}

public class ExplorationActionConditionDefinition
{
  public ConditionComparisonDefinition? ActorEnergy { get; set; }
  // TimeOfDay, etc.
}
