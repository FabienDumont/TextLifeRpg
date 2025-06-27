using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Abstraction;

public interface IRandomProvider
{
  #region Methods

  int Next(int min, int max);
  int Next(int max);
  double NextDouble();
  int NextClampedHeight(BiologicalSex sex);
  int NextClampedWeight(BiologicalSex sex, int height);
  int NextClampedMuscleMass(BiologicalSex sex, int height);

  #endregion
}
