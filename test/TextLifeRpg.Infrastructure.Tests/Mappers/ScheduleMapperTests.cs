using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.JsonDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class ScheduleMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapCorrectly()
  {
    // Arrange
    var characterId = Guid.NewGuid();
    const DayOfWeek day1 = DayOfWeek.Monday;
    var startHour1 = new TimeSpan(8, 0, 0);
    var endHour1 = new TimeSpan(9, 0, 0);
    var locationId1 = Guid.NewGuid();
    var roomId1 = Guid.NewGuid();
    const DayOfWeek day2 = DayOfWeek.Tuesday;
    var startHour2 = new TimeSpan(14, 0, 0);
    var endHour2 = new TimeSpan(15, 0, 0);
    var locationId2 = Guid.NewGuid();
    var roomId2 = Guid.NewGuid();
    var dataModel = new ScheduleDataModel
    {
      CharacterId = characterId,
      Entries =
      [
        new ScheduleEntryDataModel
          {Day = day1, StartHour = startHour1, EndHour = endHour1, LocationId = locationId1, RoomId = roomId1},
        new ScheduleEntryDataModel
          {Day = day2, StartHour = startHour2, EndHour = endHour2, LocationId = locationId2, RoomId = roomId2}
      ]
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(characterId, domain.CharacterId);
    Assert.Equal(2, domain.Entries.Count);
    Assert.Equal(day1, domain.Entries[0].Day);
    Assert.Equal(startHour1, domain.Entries[0].StartHour);
    Assert.Equal(endHour1, domain.Entries[0].EndHour);
    Assert.Equal(locationId1, domain.Entries[0].LocationId);
    Assert.Equal(roomId1, domain.Entries[0].RoomId);
    Assert.Equal(day2, domain.Entries[1].Day);
    Assert.Equal(startHour2, domain.Entries[1].StartHour);
    Assert.Equal(endHour2, domain.Entries[1].EndHour);
    Assert.Equal(locationId2, domain.Entries[1].LocationId);
    Assert.Equal(roomId2, domain.Entries[1].RoomId);
  }

  [Fact]
  public void ToDataModel_ShouldMapCorrectly()
  {
    // Arrange

    var characterId = Guid.NewGuid();
    const DayOfWeek day1 = DayOfWeek.Monday;
    var startHour1 = new TimeSpan(8, 0, 0);
    var endHour1 = new TimeSpan(9, 0, 0);
    var locationId1 = Guid.NewGuid();
    var roomId1 = Guid.NewGuid();
    const DayOfWeek day2 = DayOfWeek.Tuesday;
    var startHour2 = new TimeSpan(14, 0, 0);
    var endHour2 = new TimeSpan(15, 0, 0);
    var locationId2 = Guid.NewGuid();
    var roomId2 = Guid.NewGuid();
    var domain = Schedule.Create(
      characterId, new List<ScheduleEntry>
      {
        new(day1, startHour1, endHour1, locationId1, roomId1),
        new(day2, startHour2, endHour2, locationId2, roomId2)
      }
    );

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(characterId, dataModel.CharacterId);
    Assert.Equal(2, dataModel.Entries.Count);
    Assert.Equal(day1, dataModel.Entries[0].Day);
    Assert.Equal(startHour1, dataModel.Entries[0].StartHour);
    Assert.Equal(endHour1, dataModel.Entries[0].EndHour);
    Assert.Equal(locationId1, dataModel.Entries[0].LocationId);
    Assert.Equal(roomId1, dataModel.Entries[0].RoomId);
    Assert.Equal(day2, dataModel.Entries[1].Day);
    Assert.Equal(startHour2, dataModel.Entries[1].StartHour);
    Assert.Equal(endHour2, dataModel.Entries[1].EndHour);
    Assert.Equal(locationId2, dataModel.Entries[1].LocationId);
    Assert.Equal(roomId2, dataModel.Entries[1].RoomId);
  }

  [Fact]
  public void ToDomainCollection_ShouldMapAllItems()
  {
    // Arrange
    var dataModels = new List<ScheduleDataModel>
    {
      new()
      {
        CharacterId = Guid.NewGuid(),
        Entries =
        [
          new ScheduleEntryDataModel
          {
            Day = DayOfWeek.Monday, StartHour = new TimeSpan(8, 0, 0), EndHour = new TimeSpan(9, 0, 0),
            LocationId = Guid.NewGuid(), RoomId = Guid.NewGuid()
          }
        ]
      },
      new()
      {
        CharacterId = Guid.NewGuid(),
        Entries =
        [
          new ScheduleEntryDataModel
          {
            Day = DayOfWeek.Monday, StartHour = new TimeSpan(8, 0, 0), EndHour = new TimeSpan(9, 0, 0),
            LocationId = Guid.NewGuid(), RoomId = Guid.NewGuid()
          }
        ]
      }
    };

    // Act
    var result = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(dataModels[0].CharacterId, result[0].CharacterId);
    Assert.Equal(dataModels[1].CharacterId, result[1].CharacterId);
  }

  [Fact]
  public void ToDataModelCollection_ShouldMapAllItems()
  {
    // Arrange
    var domains = new List<Schedule>
    {
      Schedule.Create(
        Guid.NewGuid(), new List<ScheduleEntry>
        {
          new(DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0), Guid.NewGuid(), Guid.NewGuid()),
          new(DayOfWeek.Tuesday, new TimeSpan(14, 0, 0), new TimeSpan(15, 0, 0), Guid.NewGuid(), Guid.NewGuid())
        }
      ),
      Schedule.Create(
        Guid.NewGuid(), new List<ScheduleEntry>
        {
          new(DayOfWeek.Monday, new TimeSpan(8, 0, 0), new TimeSpan(9, 0, 0), Guid.NewGuid(), Guid.NewGuid()),
          new(DayOfWeek.Tuesday, new TimeSpan(14, 0, 0), new TimeSpan(15, 0, 0), Guid.NewGuid(), Guid.NewGuid())
        }
      )
    };

    // Act
    var result = domains.ToDataModelCollection();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(domains[0].CharacterId, result[0].CharacterId);
    Assert.Equal(domains[1].CharacterId, result[1].CharacterId);
  }

  #endregion
}
