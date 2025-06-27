using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class LocationOpeningHoursMapperTests
{
  #region Methods

  [Fact]
  public void ToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    const DayOfWeek dayOfWeek = DayOfWeek.Monday;
    var opensAt = new TimeSpan(8, 0, 0);
    var closesAt = new TimeSpan(18, 0, 0);

    var dataModel = new LocationOpeningHoursDataModel
    {
      Id = id,
      LocationId = locationId,
      DayOfWeek = dayOfWeek,
      OpensAt = opensAt,
      ClosesAt = closesAt
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(locationId, domain.LocationId);
    Assert.Equal(dayOfWeek, domain.DayOfWeek);
    Assert.Equal(opensAt, domain.OpensAt);
    Assert.Equal(closesAt, domain.ClosesAt);
  }

  [Fact]
  public void ToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var models = new List<LocationOpeningHoursDataModel>
    {
      new()
      {
        Id = Guid.NewGuid(),
        LocationId = Guid.NewGuid(),
        DayOfWeek = DayOfWeek.Tuesday,
        OpensAt = new TimeSpan(9, 0, 0),
        ClosesAt = new TimeSpan(17, 0, 0)
      },
      new()
      {
        Id = Guid.NewGuid(),
        LocationId = Guid.NewGuid(),
        DayOfWeek = DayOfWeek.Wednesday,
        OpensAt = new TimeSpan(10, 0, 0),
        ClosesAt = new TimeSpan(16, 0, 0)
      }
    };

    // Act
    var domains = models.ToDomainCollection();

    // Assert
    Assert.Equal(2, domains.Count);
    for (var i = 0; i < domains.Count; i++)
    {
      Assert.Equal(models[i].Id, domains[i].Id);
      Assert.Equal(models[i].LocationId, domains[i].LocationId);
      Assert.Equal(models[i].DayOfWeek, domains[i].DayOfWeek);
      Assert.Equal(models[i].OpensAt, domains[i].OpensAt);
      Assert.Equal(models[i].ClosesAt, domains[i].ClosesAt);
    }
  }

  [Fact]
  public void ToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    var locationId = Guid.NewGuid();
    var day = DayOfWeek.Thursday;
    var opens = new TimeSpan(7, 0, 0);
    var closes = new TimeSpan(15, 0, 0);

    var domain = LocationOpeningHours.Load(id, locationId, day, opens, closes);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(locationId, dataModel.LocationId);
    Assert.Equal(day, dataModel.DayOfWeek);
    Assert.Equal(opens, dataModel.OpensAt);
    Assert.Equal(closes, dataModel.ClosesAt);
  }

  [Fact]
  public void ToDataModelCollection_ShouldMapDomainCollectionToDataModelCollection()
  {
    // Arrange
    var domains = new List<LocationOpeningHours>
    {
      LocationOpeningHours.Load(
        Guid.NewGuid(), Guid.NewGuid(), DayOfWeek.Friday, new TimeSpan(8, 0, 0), new TimeSpan(16, 0, 0)
      ),
      LocationOpeningHours.Load(
        Guid.NewGuid(), Guid.NewGuid(), DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(13, 0, 0)
      )
    };

    // Act
    var dataModels = domains.ToDataModelCollection();

    // Assert
    Assert.Equal(2, dataModels.Count);
    for (var i = 0; i < dataModels.Count; i++)
    {
      Assert.Equal(domains[i].Id, dataModels[i].Id);
      Assert.Equal(domains[i].LocationId, dataModels[i].LocationId);
      Assert.Equal(domains[i].DayOfWeek, dataModels[i].DayOfWeek);
      Assert.Equal(domains[i].OpensAt, dataModels[i].OpensAt);
      Assert.Equal(domains[i].ClosesAt, dataModels[i].ClosesAt);
    }
  }

  #endregion
}
