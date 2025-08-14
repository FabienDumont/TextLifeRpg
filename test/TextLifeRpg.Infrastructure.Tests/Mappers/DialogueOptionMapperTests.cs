using TextLifeRpg.Domain;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Mappers;

namespace TextLifeRpg.Infrastructure.Tests.Mappers;

public class DialogueOptionMapperTests
{
  #region Methods

  [Fact]
  public void MapToDomain_ShouldMapDataModelToDomain()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string label = "Say goodbye";
    const int neededMinutes = 0;

    var dataModel = new DialogueOptionDataModel
    {
      Id = id,
      Label = label,
      NeededMinutes = neededMinutes
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(label, domain.Label);
    Assert.Equal(neededMinutes, domain.NeededMinutes);
  }

  [Fact]
  public void MapToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var id1 = Guid.NewGuid();
    const string label1 = "Say goodbye";
    const int neededMinutes1 = 0;
    var id2 = Guid.NewGuid();
    const string label2 = "Ask something";
    const int neededMinutes2 = 0;

    var dataModels = new List<DialogueOptionDataModel>
    {
      new()
      {
        Id = id1,
        Label = label1,
        NeededMinutes = neededMinutes1
      },
      new()
      {
        Id = id2,
        Label = label2,
        NeededMinutes = neededMinutes2
      }
    };

    // Act
    var domainModels = dataModels.ToDomainCollection();

    // Assert
    Assert.Equal(2, domainModels.Count);

    for (var i = 0; i < domainModels.Count; i++)
    {
      Assert.Equal(dataModels[i].Id, domainModels[i].Id);
      Assert.Equal(dataModels[i].Label, domainModels[i].Label);
      Assert.Equal(dataModels[i].NeededMinutes, domainModels[i].NeededMinutes);
    }
  }

  [Fact]
  public void MapToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string label = "Say goodbye";
    const int neededMinutes = 0;

    var domain = DialogueOption.Load(id, label, neededMinutes);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(label, dataModel.Label);
    Assert.Equal(neededMinutes, dataModel.NeededMinutes);
  }

  #endregion
}
