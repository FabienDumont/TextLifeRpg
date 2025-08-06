namespace TextLifeRpg.Domain;

public class GameFlowStep
{
  #region Properties

  public required Func<GameSave, Task> ExecuteAsync { get; init; }

  #endregion
}
