using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class ScheduleEntryMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapCorrectly()
  {
    // Arrange
    var dataModel = new ScheduleEntryDataModel
    {
      Day = DayOfWeek.Monday, StartHour = new TimeSpan(8, 0, 0), EndHour = new TimeSpan(9, 0, 0),
      LocationId = Guid.NewGuid(), RoomId = Guid.NewGuid()
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(dataModel.StartHour, domain.StartHour);
    Assert.Equal(dataModel.LocationId, domain.LocationId);
    Assert.Equal(dataModel.RoomId, domain.RoomId);
  }

  [Fact]
  public void ToDataModel_ShouldMapCorrectly()
  {
    // Arrange
    var domain = new ScheduleEntry(
      DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0), Guid.NewGuid(), Guid.NewGuid()
    );

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(domain.StartHour, dataModel.StartHour);
    Assert.Equal(domain.LocationId, dataModel.LocationId);
    Assert.Equal(domain.RoomId, dataModel.RoomId);
  }

  [Fact]
  public void ToDomainCollection_ShouldMapAllItems()
  {
    // Arrange
    var dataModels = new List<ScheduleEntryDataModel>
    {
      new()
      {
        Day = DayOfWeek.Monday, StartHour = new TimeSpan(8, 0, 0), EndHour = new TimeSpan(9, 0, 0),
        LocationId = Guid.NewGuid(), RoomId = Guid.NewGuid()
      },
      new()
      {
        Day = DayOfWeek.Tuesday, StartHour = new TimeSpan(14, 0, 0), EndHour = new TimeSpan(15, 0, 0),
        LocationId = Guid.NewGuid(), RoomId = Guid.NewGuid()
      }
    };

    // Act
    var result = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(dataModels.Count, result.Count);
    for (var i = 0; i < dataModels.Count; i++)
    {
      Assert.Equal(dataModels[i].StartHour, result[i].StartHour);
      Assert.Equal(dataModels[i].LocationId, result[i].LocationId);
      Assert.Equal(dataModels[i].RoomId, result[i].RoomId);
    }
  }

  [Fact]
  public void ToDataModelCollection_ShouldMapAllItems()
  {
    // Arrange
    var domains = new List<ScheduleEntry>
    {
      new(DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0), Guid.NewGuid(), Guid.NewGuid()),
      new(DayOfWeek.Tuesday, new TimeSpan(14, 0, 0), new TimeSpan(15, 0, 0), Guid.NewGuid(), Guid.NewGuid())
    };

    // Act
    var result = domains.ToDataModelCollection();

    // Assert
    Assert.Equal(domains.Count, result.Count);
    for (var i = 0; i < domains.Count; i++)
    {
      Assert.Equal(domains[i].StartHour, result[i].StartHour);
      Assert.Equal(domains[i].LocationId, result[i].LocationId);
      Assert.Equal(domains[i].RoomId, result[i].RoomId);
    }
  }

  #endregion
}
