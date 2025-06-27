using Microsoft.Extensions.DependencyInjection;
using TextLifeRpg.Application;
using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Application.Abstraction.Repositories;
using TextLifeRpg.Application.Services;

namespace TextLifeRpg.Application.Tests;

public class ServiceCollectionExtensionsTests
{
  #region Methods

  [Fact]
  public void AddApplication_ShouldRegisterTraitServiceAsSingleton()
  {
    // Arrange
    IServiceCollection services = new ServiceCollection();
    var fakeRepo = A.Fake<ITraitRepository>();
    services.AddSingleton(fakeRepo);

    // Act
    services.AddApplication();
    var provider = services.BuildServiceProvider();

    // Assert
    var service1 = provider.GetService<ITraitService>();
    var service2 = provider.GetService<ITraitService>();

    Assert.NotNull(service1);
    Assert.IsType<TraitService>(service1);
    Assert.Same(service1, service2);
  }

  #endregion
}
