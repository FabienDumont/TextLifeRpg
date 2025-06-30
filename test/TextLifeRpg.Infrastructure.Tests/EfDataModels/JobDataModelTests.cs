using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Tests.EfDataModels;

public class JobDataModelTests
{
  #region Methods

  [Fact]
  public void Instanciation_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();

    // Act
    var job = new JobDataModel
    {
      Id = id,
      Name = string.Empty,
      HourIncome = int.MinValue,
      MaxWorkers = int.MinValue
    };

    // Assert
    Assert.Equal(id, job.Id);
    Assert.Equal(string.Empty, job.Name);
    Assert.Equal(int.MinValue, job.HourIncome);
    Assert.Equal(int.MinValue, job.MaxWorkers);
  }

  #endregion
}
