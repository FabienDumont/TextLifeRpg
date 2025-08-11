namespace TextLifeRpg.Domain;

public class GameFlowStep
{
  #region Properties

  public bool Delay { get; init; }
  public required Func<GameSave, Task> ExecuteAsync { get; init; }

  #endregion
}
