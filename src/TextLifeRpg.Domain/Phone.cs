namespace TextLifeRpg.Domain;

public class Phone
{
  #region Properties

  /// <summary>
  /// The contacts in the phone.
  /// </summary>
  public List<PhoneContact> Contacts { get; } = [];

  #endregion

  #region Ctors

  /// <summary>
  /// Private constructor used internally.
  /// </summary>
  private Phone(List<Character> characters)
  {
    Contacts.AddRange(characters.Select(PhoneContact.Create));
  }

  #endregion

  #region Methods

  /// <summary>
  /// Factory method to load an existing instance from persistence.
  /// </summary>
  public static Phone Load(List<Character> contacts)
  {
    return new Phone(contacts);
  }

  /// <summary>
  /// Factory method to create a new instance.
  /// </summary>
  public static Phone Create()
  {
    return new Phone([]);
  }

  /// <summary>
  /// Adds a character to the phone's contact list.
  /// </summary>
  /// <param name="character">The character to add to the contacts.</param>
  public void AddToContacts(Character character)
  {
    Contacts.Add(PhoneContact.Create(character));
  }

  /// <summary>
  /// Removes a character from the phone's contact list.
  /// </summary>
  /// <param name="characterId">The character identifier.</param>
  public void RemoveFromContacts(Guid characterId)
  {
    Contacts.RemoveAll(c => c.Character.Id == characterId);
  }

  #endregion
}
