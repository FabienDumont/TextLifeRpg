using TextLifeRpg.Domain.Constants;

namespace TextLifeRpg.Domain.Tests.Constants;

public class JobNamesTests
{
  #region Methods

  [Fact]
  public void All_ShouldContain_AllDefinedJobNames()
  {
    // Arrange
    var expected = new[]
    {
      JobNames.GarbageCollector,
      JobNames.Janitor,
      JobNames.DeliveryDriver,
      JobNames.CollegeTeacher
    };

    // Act
    var actual = JobNames.All;

    // Assert
    Assert.Equal(expected.Length, actual.Count);
    foreach (var name in expected)
    {
      Assert.Contains(name, actual);
    }
  }

  [Fact]
  public void JobNames_ShouldBeUnique()
  {
    // Act
    var uniqueNames = JobNames.All.Distinct().ToList();

    // Assert
    Assert.Equal(JobNames.All.Count, uniqueNames.Count);
  }

  #endregion
}
