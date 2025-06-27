using TextLifeRpg.Application.Randomization;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Tests.Randomization;

public class RandomProviderTests
{
  #region Fields

  private readonly RandomProvider _provider = new();

  #endregion

  #region Methods

  [Fact]
  public void NextMinMax_ShouldReturnValueWithinRange()
  {
    // Arrange
    const int min = 1;
    const int max = 10;

    // Act
    var result = _provider.Next(min, max);

    // Assert
    Assert.InRange(result, min, max - 1);
  }

  [Fact]
  public void NextMax_ShouldReturnValueWithinRange()
  {
    // Arrange
    const int max = 10;

    // Act
    var result = _provider.Next(max);

    // Assert
    Assert.InRange(result, 0, max - 1);
  }

  [Fact]
  public void NextDouble_ShouldReturnValueBetweenZeroAndOne()
  {
    // Act
    var result = _provider.NextDouble();

    // Assert
    Assert.InRange(result, 0.0, 1.0);
  }

  [Theory]
  [InlineData(BiologicalSex.Male)]
  [InlineData(BiologicalSex.Female)]
  [InlineData((BiologicalSex) 42)]
  public void NextClampedHeight_ShouldReturnReasonableHeight(BiologicalSex sex)
  {
    // Act
    var height = _provider.NextClampedHeight(sex);

    // Assert
    Assert.InRange(height, 100, 220);
  }

  [Theory]
  [InlineData(BiologicalSex.Male, 160)]
  [InlineData(BiologicalSex.Male, 180)]
  [InlineData(BiologicalSex.Female, 150)]
  [InlineData(BiologicalSex.Female, 170)]
  [InlineData((BiologicalSex) 99, 165)] // unknown fallback case
  public void NextClampedWeight_ShouldReturnReasonableWeight(BiologicalSex sex, int heightInCm)
  {
    // Act
    var weight = _provider.NextClampedWeight(sex, heightInCm);

    // Assert
    Assert.InRange(weight, 40, 200);
  }

  [Theory]
  [InlineData(BiologicalSex.Male, 160)]
  [InlineData(BiologicalSex.Male, 180)]
  [InlineData(BiologicalSex.Female, 150)]
  [InlineData(BiologicalSex.Female, 170)]
  [InlineData((BiologicalSex)42, 165)] // unknown fallback case
  public void NextClampedMuscleMass_ShouldReturnReasonableMass(BiologicalSex sex, int heightInCm)
  {
    // Act
    var muscleMass = _provider.NextClampedMuscleMass(sex, heightInCm);

    // Assert
    Assert.InRange(muscleMass, 15, 40);
  }

  #endregion
}
