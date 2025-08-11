namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing a saved game state.
/// </summary>
public class GameSave
{
  #region Properties

  /// <summary>
  /// Unique identifier.
  /// </summary>
  public Guid Id { get; }

  /// <summary>
  /// Name of the save file.
  /// </summary>
  public string Name => $"{PlayerCharacter.Name}_{World.CurrentDate:yyyyMMdd_HHmmss}";

  /// <summary>
  /// World state at the time of the save.
  /// </summary>
  public World World { get; }

  /// <summary>
  /// Identifier of the player character in the world.
  /// </summary>
  public Guid PlayerCharacterId { get; }

  /// <summary>
  /// The player character referenced by the save.
  /// </summary>
  public Character PlayerCharacter => World.Characters.First(c => c.Id == PlayerCharacterId);

  /// <summary>
  /// Identifier for the NPC currently interacting with the player.
  /// </summary>
  public Guid? InteractingNpcId { get; private set; }

  /// <summary>
  /// Indicates the type of interaction with a NPC.
  /// </summary>
  public NpcInteractionType? NpcInteractionType { get; private set; }

  /// <summary>
  /// The NPC currently being interacted with in the game.
  /// </summary>
  public Character? InteractingNpc => World.Characters.FirstOrDefault(c => c.Id == InteractingNpcId);

  public List<DialogueOption> PendingDialogueOptions { get; } = new();

  /// <summary>
  /// Timestamp of when the save was created.
  /// </summary>
  public DateTime SavedAt { get; } = DateTime.UtcNow;

  /// <summary>
  /// List of text lines displayed to the user (with color formatting).
  /// </summary>
  public List<TextLine> TextLines { get; private init; } = [];

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private GameSave(Guid id, Guid playerCharacterId, World world)
  {
    Id = id;
    PlayerCharacterId = playerCharacterId;
    World = world;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static GameSave Load(Guid id, Guid playerCharacterId, World world, List<TextLine> textLines)
  {
    if (world.Characters.All(c => c.Id != playerCharacterId))
    {
      throw new InvalidOperationException("Player character not found in character list.");
    }

    return new GameSave(id, playerCharacterId, world)
    {
      TextLines = textLines
    };
  }

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static GameSave Create(Character playerCharacter, World world)
  {
    ArgumentNullException.ThrowIfNull(playerCharacter);

    return new GameSave(Guid.NewGuid(), playerCharacter.Id, world);
  }

  /// <summary>
  /// Adds a line of styled text to the game save.
  /// </summary>
  private void AddTextLine(TextLine textLine)
  {
    TextLines.Add(textLine);
  }

  /// <summary>
  /// Adds a new text entry with multiple parts (each part can have its own color) to the save.
  /// </summary>
  /// <param name="textParts">A list of TextPart objects representing the text and colors.</param>
  public void AddText(List<TextPart> textParts)
  {
    // Create a new TextLine with the provided list of TextParts
    var textLine = new TextLine(textParts);

    // Add the created TextLine to the TextLines collection
    AddTextLine(textLine);
  }

  /// <summary>
  /// Clears the current text log.
  /// </summary>
  public void ResetText()
  {
    TextLines.Clear();
  }

  /// <summary>
  /// Initiates a dialogue interaction with a specified NPC.
  /// </summary>
  /// <param name="npcId">The unique identifier of the NPC to start a dialogue with.</param>
  public void StartDialogue(Guid npcId)
  {
    InteractingNpcId = npcId;
    NpcInteractionType = Domain.NpcInteractionType.Dialogue;
  }

  /// <summary>
  /// Ends the current interaction, resetting the interacting NPC and interaction type.
  /// </summary>
  public void EndInteraction()
  {
    InteractingNpcId = null;
    NpcInteractionType = null;
  }

  public void ClearPendingDialogueOptions()
  {
    PendingDialogueOptions.Clear();
  }

  #endregion
}

public enum NpcInteractionType
{
  Dialogue
}
