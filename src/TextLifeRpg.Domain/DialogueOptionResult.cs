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
  /// Indicates if minutes should be added as part of the dialogue option result.
  /// </summary>
  public bool AddMinutes { get; }

  /// <summary>
  /// Indicates the target character relationship value change.
  /// </summary>
  public int? TargetRelationshipValueChange { get; }

  /// <summary>
  /// Represents a fact that can be learned as a result of a dialogue option.
  /// </summary>
  public Fact? ActorLearnFact { get; }

  /// <summary>
  /// Represents a special action between an acting character and a target character.
  /// </summary>
  public ActorTargetSpecialAction? ActorTargetSpecialAction { get; }

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
    Guid id, Guid dialogueOptionId, bool addMinutes, int? targetRelationshipValueChange, Fact? actorLearnFact,
    ActorTargetSpecialAction? actorTargetSpecialAction, bool endDialogue
  )
  {
    Id = id;
    DialogueOptionId = dialogueOptionId;
    AddMinutes = addMinutes;
    TargetRelationshipValueChange = targetRelationshipValueChange;
    ActorLearnFact = actorLearnFact;
    ActorTargetSpecialAction = actorTargetSpecialAction;
    EndDialogue = endDialogue;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static DialogueOptionResult Create(
    Guid dialogueOptionId, bool addMinutes, int? targetRelationshipValueChange, Fact? learnFact,
    ActorTargetSpecialAction? actorTargetSpecialAction, bool endDialogue
  )
  {
    return new DialogueOptionResult(
      Guid.NewGuid(), dialogueOptionId, addMinutes, targetRelationshipValueChange, learnFact, actorTargetSpecialAction,
      endDialogue
    );
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static DialogueOptionResult Load(
    Guid id, Guid dialogueOptionId, bool addMinutes, int? targetRelationshipValueChange, Fact? learnFact,
    ActorTargetSpecialAction? actorTargetSpecialAction, bool endDialogue
  )
  {
    return new DialogueOptionResult(
      id, dialogueOptionId, addMinutes, targetRelationshipValueChange, learnFact, actorTargetSpecialAction, endDialogue
    );
  }

  #endregion
}

public enum ActorTargetSpecialAction
{
  AddTargetPhoneNumber
}
