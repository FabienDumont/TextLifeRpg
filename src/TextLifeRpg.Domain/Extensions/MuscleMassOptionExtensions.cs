namespace TextLifeRpg.Domain.Extensions;

public static class MuscleMassOptionExtensions
{
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
