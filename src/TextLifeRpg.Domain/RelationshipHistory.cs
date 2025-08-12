namespace TextLifeRpg.Domain;

/// <summary>
/// Domain class representing a relationship's history between a source character and a target character.
/// </summary>
public class RelationshipHistory
{
  #region Fields

  private readonly List<string> _learnedFacts = [];

  #endregion

  #region Properties

  /// <summary>
  /// The timestamp of the first interaction between the source and target characters.
  /// </summary>
  public DateOnly FirstInteraction { get; }

  /// <summary>
  /// The timestamp of the most recent interaction between the source and target characters.
  /// </summary>
  public DateOnly LastInteraction { get; private set; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor for internal use.
  /// </summary>
  private RelationshipHistory(DateOnly firstInteraction, DateOnly lastInteraction)
  {
    FirstInteraction = firstInteraction;
    LastInteraction = lastInteraction;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static RelationshipHistory Create(DateOnly firstInteraction, DateOnly lastInteraction)
  {
    return new RelationshipHistory(firstInteraction, lastInteraction);
  }

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static RelationshipHistory Load(DateOnly firstInteraction, DateOnly lastInteraction)
  {
    return new RelationshipHistory(firstInteraction, lastInteraction);
  }

  /// <summary>
  /// Updates the last interaction timestamp.
  /// </summary>
  public void UpdateLastInteraction(DateOnly date)
  {
    LastInteraction = date;
  }

  /// <summary>
  /// Checks if a specific fact identified by the given key has been learned.
  /// </summary>
  /// <param name="fact">The unique identifier of the fact to check.</param>
  /// <returns>True if the fact with the specified key has been learned; otherwise, false.</returns>
  public bool HasLearnedFact(string fact)
  {
    return _learnedFacts.Contains(fact);
  }

  /// <summary>
  /// Records a fact that has been learned, stores it with the specified key and value, and updates the last interaction date.
  /// </summary>
  /// <param name="fact">The unique identifier for the fact being learned.</param>
  public void LearnFact(string fact)
  {
    if (!HasLearnedFact(fact))
    {
      _learnedFacts.Add(fact);
    }
  }

  #endregion
}
