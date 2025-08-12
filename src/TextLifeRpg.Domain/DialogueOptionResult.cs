namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing a dialogue option result.
/// </summary>
public class DialogueOptionResult
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  /// Dialogue option identifier.
  /// </summary>
  public Guid DialogueOptionId { get; }

  /// <summary>
  /// Indicates the target character relationship value change.
  /// </summary>
  public int? TargetRelationshipValueChange { get; }

  /// <summary>
  /// Represents a fact that can be learned as a result of a dialogue option.
  /// </summary>
  public string? ActorLearnFact { get; }

  /// <summary>
  /// Indicates whether the dialogue ends after selecting this option.
  /// </summary>
  public bool EndDialogue { get; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private DialogueOptionResult(
    Guid id, Guid dialogueOptionId, int? targetRelationshipValueChange, string? actorLearnFact, bool endDialogue
  )
  {
    Id = id;
    DialogueOptionId = dialogueOptionId;
    TargetRelationshipValueChange = targetRelationshipValueChange;
    ActorLearnFact = actorLearnFact;
    EndDialogue = endDialogue;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static DialogueOptionResult Create(
    Guid dialogueOptionId, int? targetRelationshipValueChange, string? learnFact, bool endDialogue
  )
  {
    return new DialogueOptionResult(
      Guid.NewGuid(), dialogueOptionId, targetRelationshipValueChange, learnFact, endDialogue
    );
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static DialogueOptionResult Load(
    Guid id, Guid dialogueOptionId, int? targetRelationshipValueChange, string? learnFact, bool endDialogue
  )
  {
    return new DialogueOptionResult(id, dialogueOptionId, targetRelationshipValueChange, learnFact, endDialogue);
  }

  #endregion
}
