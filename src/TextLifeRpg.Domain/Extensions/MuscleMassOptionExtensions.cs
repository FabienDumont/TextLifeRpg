namespace TextLifeRpg.Domain.Extensions;

/// <summary>
/// Provides extension methods for converting between <see cref="MuscleMassOption"/>
/// values and their equivalent integer representations in kilograms.
/// </summary>
public static class MuscleMassOptionExtensions
{
  /// <summary>
  /// Converts a specified MuscleMassOption into its corresponding weight in kilograms.
  /// </summary>
  /// <param name="option">The MuscleMassOption to be converted.</param>
  /// <returns>The weight in kilograms that corresponds to the given MuscleMassOption.</returns>
  public static int ToKg(this MuscleMassOption option) =>
    option switch
    {
      MuscleMassOption.VeryLow => 15,
      MuscleMassOption.Low => 18,
      MuscleMassOption.Average => 22,
      MuscleMassOption.High => 27,
      MuscleMassOption.VeryHigh => 32,
      _ => 22
    };

  /// <summary>
  /// Converts a value in kilograms to its corresponding MuscleMassOption.
  /// </summary>
  /// <param name="kg">The weight in kilograms to be converted.</param>
  /// <returns>The MuscleMassOption that corresponds to the given weight in kilograms.</returns>
  public static MuscleMassOption FromKg(int kg) =>
    kg switch
    {
      <= 16 => MuscleMassOption.VeryLow,
      <= 19 => MuscleMassOption.Low,
      <= 24 => MuscleMassOption.Average,
      <= 30 => MuscleMassOption.High,
      _ => MuscleMassOption.VeryHigh
    };
}
