using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class JobMapperTests
{
  #region Methods

  [Fact]
  public void MapToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();

    var dataModel = new JobDataModel
    {
      Id = id,
      Name = string.Empty,
      HourIncome = int.MinValue,
      MaxWorkers = int.MinValue
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(string.Empty, domain.Name);
    Assert.Equal(int.MinValue, domain.HourIncome);
  }

  [Fact]
  public void MapToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var dataModels = new List<JobDataModel>
    {
      new() {Id = Guid.NewGuid(), Name = string.Empty, HourIncome = int.MinValue, MaxWorkers = int.MinValue},
      new() {Id = Guid.NewGuid(), Name = string.Empty, HourIncome = int.MaxValue, MaxWorkers = int.MinValue}
    };

    // Act
    var domainModels = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domainModels.Count);
    Assert.Equal(dataModels[0].Id, domainModels[0].Id);
    Assert.Equal(dataModels[0].Name, domainModels[0].Name);
    Assert.Equal(dataModels[0].HourIncome, domainModels[0].HourIncome);
    Assert.Equal(dataModels[0].MaxWorkers, domainModels[0].MaxWorkers);
    Assert.Equal(dataModels[1].Id, domainModels[1].Id);
    Assert.Equal(dataModels[1].Name, domainModels[1].Name);
    Assert.Equal(dataModels[1].HourIncome, domainModels[1].HourIncome);
    Assert.Equal(dataModels[1].MaxWorkers, domainModels[1].MaxWorkers);
  }

  [Fact]
  public void MapToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();

    var domain = Job.Load(id, string.Empty, int.MinValue, int.MinValue);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(string.Empty, dataModel.Name);
    Assert.Equal(int.MinValue, dataModel.HourIncome);
    Assert.Equal(int.MinValue, dataModel.MaxWorkers);
  }

  #endregion
}
