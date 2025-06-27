using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

/// <summary>
/// Defines a contract for providing random values within specified ranges or rules.
/// </summary>
public interface IRandomProvider
{
  #region Methods

  /// <summary>
  /// Generates a random integer within the specified range.
  /// </summary>
  /// <param name="min">The inclusive lower bound of the random number to generate.</param>
  /// <param name="max">The exclusive upper bound of the random number to generate.</param>
  /// <returns>A random integer that is greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
  int Next(int min, int max);

  /// <summary>
  /// Generates a random double-precision floating-point number between 0.0 and 1.0.
  /// </summary>
  /// <returns>A double-precision floating-point number that is greater than or equal to 0.0 and less than 1.0.</returns>
  double NextDouble();

  /// <summary>
  /// Generates a random height within a plausible range based on the specified biological sex.
  /// </summary>
  /// <param name="sex">The biological sex used to determine the range of possible heights.</param>
  /// <returns>A random height clamped to a valid range for the specified <paramref name="sex"/>.</returns>
  int NextClampedHeight(BiologicalSex sex);

  /// <summary>
  /// Generates a random weight value clamped within a range that considers the biological sex and height provided.
  /// </summary>
  /// <param name="sex">The biological sex of the individual, which influences the weight range.</param>
  /// <param name="height">The height of the individual, used to calculate the appropriate clamped weight.</param>
  /// <returns>A random integer representing the weight, adjusted based on the specified biological sex and height.</returns>
  int NextClampedWeight(BiologicalSex sex, int height);

  /// <summary>
  /// Generates a clamped muscle mass value based on the biological sex and height of the character.
  /// </summary>
  /// <param name="sex">The biological sex of the character.</param>
  /// <param name="height">The height of the character.</param>
  /// <returns>An integer representing the clamped muscle mass value for the specified biological sex and height.</returns>
  int NextClampedMuscleMass(BiologicalSex sex, int height);

  #endregion
}
