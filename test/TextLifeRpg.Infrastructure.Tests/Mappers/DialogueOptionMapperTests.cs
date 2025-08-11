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

    var dataModel = new DialogueOptionDataModel
    {
      Id = id,
      Label = label
    };

    // Act
    var domain = dataModel.ToDomain();

    // Assert
    Assert.Equal(id, domain.Id);
    Assert.Equal(label, domain.Label);
  }

  [Fact]
  public void MapToDomainCollection_ShouldMapDataModelCollectionToDomainCollection()
  {
    // Arrange
    var id1 = Guid.NewGuid();
    const string label1 = "Say goodbye";
    var id2 = Guid.NewGuid();
    const string label2 = "Ask something";

    var dataModels = new List<DialogueOptionDataModel>
    {
      new()
      {
        Id = id1,
        Label = label1
      },
      new()
      {
        Id = id2,
        Label = label2
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
    }
  }

  [Fact]
  public void MapToDataModel_ShouldMapDomainToDataModel()
  {
    // Arrange
    var id = Guid.NewGuid();
    const string label = "Say goodbye";

    var domain = DialogueOption.Load(id, label);

    // Act
    var dataModel = domain.ToDataModel();

    // Assert
    Assert.Equal(id, dataModel.Id);
    Assert.Equal(label, dataModel.Label);
  }

  #endregion
}
