using TextLifeRpg.Domain;

namespace TextLifeRpg.Domain.Tests.Helpers;

public class CharacterBuilder
{
  private string _name = $"NPC_{Guid.NewGuid()}";
  private DateOnly _birthDate = new(1990, 1, 1);
  private BiologicalSex _sex = BiologicalSex.Male;
  private int _height = 170;
  private int _weight = 70;
  private int _muscleMass = 5;

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

  public CharacterBuilder WithHeight(int height)
  {
    _height = height;
    return this;
  }

  public Character Build()
  {
    return Character.Create(_name, _birthDate, _sex, _height, _weight, _muscleMass);
  }
}
