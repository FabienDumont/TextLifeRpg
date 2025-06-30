namespace TextLifeRpg.Domain.Tests.Helpers;

public class CharacterBuilder
{
  private string _name = $"NPC_{Guid.NewGuid()}";
  private DateOnly _birthDate = new(1990, 1, 1);
  private BiologicalSex _sex = BiologicalSex.Male;
  private int _height = 170;
  private int _weight = 70;
  private int _muscleMass = 5;
  private CharacterAttributes _characterAttributes = CharacterAttributes.Create(5, 5, 5);
  private Guid? _jobId;

  public CharacterBuilder WithName(string name)
  {
    _name = name;
    return this;
  }

  public CharacterBuilder WithBirthDate(DateOnly birthDate)
  {
    _birthDate = birthDate;
    return this;
  }

  public CharacterBuilder WithSex(BiologicalSex sex)
  {
    _sex = sex;
    return this;
  }

  public CharacterBuilder WithJob(Guid jobId)
  {
    _jobId = jobId;
    return this;
  }

  public Character Build()
  {
    var character = Character.Create(_name, _birthDate, _sex, _height, _weight, _muscleMass, _characterAttributes);
    if (_jobId is not null)
    {
      character.SetJob(_jobId.Value);
    }
    return character;
  }
}
