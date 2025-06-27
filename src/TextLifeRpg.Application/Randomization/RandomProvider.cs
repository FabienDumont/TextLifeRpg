using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Randomization;

/// <summary>
/// Provides a wrapper around <see cref="System.Random" /> to allow injection and testing of randomness.
/// </summary>
public class RandomProvider : IRandomProvider
{
  #region Fields

  private readonly Random _rnd = new();

  #endregion

  #region Implementation of IRandomProvider

  /// <inheritdoc />
  public int Next(int min, int max)
  {
    return _rnd.Next(min, max);
  }

  /// <inheritdoc />
  public double NextDouble()
  {
    return _rnd.NextDouble();
  }

  /// <inheritdoc />
  public int NextClampedHeight(BiologicalSex sex)
  {
    double mean = sex switch
    {
      BiologicalSex.Male => 175,
      BiologicalSex.Female => 162,
      _ => 170
    };

    var stdDev = sex switch
    {
      BiologicalSex.Male => 7,
      BiologicalSex.Female => 6,
      _ => 6.5
    };

    var height = NextGaussian(mean, stdDev);

    return (int) Math.Clamp(height, 100, 220);
  }

  /// <inheritdoc />
  public int NextClampedWeight(BiologicalSex sex, int height)
  {
    // Rough BMI-based mean targeting: BMI = weight / (height/100)^2
    // Aim for avg BMI ~22–26
    var heightM = height / 100.0;
    var meanWeight = heightM * heightM * (sex == BiologicalSex.Male ? 24.5 : 23.5);

    // Slightly higher std dev for males
    var stdDev = sex switch
    {
      BiologicalSex.Male => 12,
      BiologicalSex.Female => 10,
      _ => 11
    };

    var weight = NextGaussian(meanWeight, stdDev);
    return (int) Math.Clamp(weight, 40, 200);
  }

  /// <inheritdoc />
  public int NextClampedMuscleMass(BiologicalSex sex, int height)
  {
    var heightM = height / 100.0;

    var meanFfmi = sex switch
    {
      BiologicalSex.Male => 20.5,
      BiologicalSex.Female => 18.5,
      _ => 19.5
    };

    const double stdDevFfmi = 1.5;

    var ffmi = NextGaussian(meanFfmi, stdDevFfmi);
    var leanMass = ffmi * (heightM * heightM) * 0.4; // Skeletal is roughly 30-45% of fat-free mass

    return (int) Math.Clamp(leanMass, 15, 40);
  }

  #endregion

  #region Methods

  /// <summary>
  /// Generates a random value following a Gaussian (normal) distribution with the specified mean and standard deviation.
  /// </summary>
  /// <param name="mean">The mean (average) value of the Gaussian distribution.</param>
  /// <param name="stdDev">The standard deviation, determining the spread or variation in the distribution.</param>
  /// <returns>A random value following a Gaussian distribution.</returns>
  private double NextGaussian(double mean, double stdDev)
  {
    // Box-Muller transform
    var u1 = 1.0 - _rnd.NextDouble();
    var u2 = 1.0 - _rnd.NextDouble();
    var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
    return mean + stdDev * randStdNormal;
  }

  #endregion
}
