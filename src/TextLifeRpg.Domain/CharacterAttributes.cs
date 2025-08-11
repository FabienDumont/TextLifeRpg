namespace TextLifeRpg.Domain;

public class CharacterAttributes
{
  #region Properties

  public int Intelligence { get; private set; }
  public int Strength { get; private set; }
  public int Charisma { get; private set; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private CharacterAttributes(int intelligence, int strength, int charisma)
  {
    Intelligence = intelligence;
    Strength = strength;
    Charisma = charisma;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static CharacterAttributes Create(int intelligence, int strength, int charisma)
  {
    return new CharacterAttributes(intelligence, strength, charisma);
  }

  #endregion
}
