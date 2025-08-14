namespace TextLifeRpg.Domain;

public class PhoneContact
{
  #region Properties

  /// <summary>
  /// The character.
  /// </summary>
  public Character Character { get; private set; }

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private PhoneContact(Character character)
  {
    Character = character;
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static PhoneContact Create(Character character)
  {
    return new PhoneContact(character);
  }

  #endregion
}
