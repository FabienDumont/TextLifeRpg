using TextLifeRpg.Domain;
using TextLifeRpg.Domain.Extensions;

namespace TextLifeRpg.Domain.Tests.Extensions;

public class MuscleMassOptionExtensionsTests
{
  [Theory]
  [InlineData(MuscleMassOption.VeryLow, 15)]
  [InlineData(MuscleMassOption.Low, 18)]
  [InlineData(MuscleMassOption.Average, 22)]
  [InlineData(MuscleMassOption.High, 27)]
  [InlineData(MuscleMassOption.VeryHigh, 32)]
  public void ToKg_ShouldReturnExpectedValue(MuscleMassOption option, int expected)
  {
    // Act
    var result = option.ToKg();

    // Assert
    Assert.Equal(expected, result);
  }

  [Fact]
  public void ToKg_UnknownEnumValue_ShouldReturnDefault()
  {
    // Arrange
    const MuscleMassOption invalidOption = (MuscleMassOption) (-1);

    // Act
    var result = invalidOption.ToKg();

    // Assert
    Assert.Equal(22, result);
  }

  [Theory]
  [InlineData(10, MuscleMassOption.VeryLow)]
  [InlineData(16, MuscleMassOption.VeryLow)]
  [InlineData(17, MuscleMassOption.Low)]
  [InlineData(19, MuscleMassOption.Low)]
  [InlineData(20, MuscleMassOption.Average)]
  [InlineData(24, MuscleMassOption.Average)]
  [InlineData(25, MuscleMassOption.High)]
  [InlineData(30, MuscleMassOption.High)]
  [InlineData(31, MuscleMassOption.VeryHigh)]
  [InlineData(40, MuscleMassOption.VeryHigh)]
  public void FromKg_ShouldMapKgToCorrectOption(int kg, MuscleMassOption expected)
  {
    var result = MuscleMassOptionExtensions.FromKg(kg);
    Assert.Equal(expected, result);
  }
}
