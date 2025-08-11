namespace TextLifeRpg.Domain.Tests;

public class JobTests
{
  #region Methods

  [Fact]
  public void Create_ShouldInitialize()
  {
    // Act
    var job = Job.Create(string.Empty, int.MinValue, int.MinValue);

    // Assert
    Assert.NotNull(job);
    Assert.NotEqual(Guid.Empty, job.Id);
    Assert.Equal(string.Empty, job.Name);
    Assert.Equal(int.MinValue, job.HourIncome);
    Assert.Equal(int.MinValue, job.MaxWorkers);
  }

  [Fact]
  public void Load_ShouldInitializeWithGivenValues()
  {
    // Arrange
    var id = Guid.NewGuid();

    // Act
    var job = Job.Load(id, string.Empty, int.MinValue, int.MinValue);

    // Assert
    Assert.Equal(id, job.Id);
    Assert.Equal(string.Empty, job.Name);
    Assert.Equal(int.MinValue, job.HourIncome);
    Assert.Equal(int.MinValue, job.MaxWorkers);
  }

  #endregion
}
