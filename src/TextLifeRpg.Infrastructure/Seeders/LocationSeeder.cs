using TextLifeRpg.Infrastructure.EfDataModels;

namespace TextLifeRpg.Infrastructure.Seeders;

/// <summary>
/// Location data seeder.
/// </summary>
public class LocationSeeder : IDataSeeder
{
  #region Implementation of IDataSeeder

  /// <inheritdoc />
  public async Task SeedAsync(ApplicationContext context)
  {
    var homeId = Guid.NewGuid();
    var streetId = Guid.NewGuid();
    var collegeId = Guid.NewGuid();

    await context.Locations.AddRangeAsync(
      new LocationDataModel {Id = homeId, Name = "Home"},
      new LocationDataModel {Id = streetId, Name = "Street"},
      new LocationDataModel {Id = collegeId, Name = "College"}
    );

    var bedroomId = Guid.NewGuid();
    var livingRoomId = Guid.NewGuid();

    await context.Rooms.AddRangeAsync(
      new RoomDataModel {Id = bedroomId, LocationId = homeId, Name = "Bedroom", IsPlayerSpawn = true},
      new RoomDataModel {Id = livingRoomId, LocationId = homeId, Name = "Living Room", IsPlayerSpawn = false}
    );

    await context.SaveChangesAsync().ConfigureAwait(false);

    var movementHomeLivingRoomToHomeBedroom = new MovementDataModel
    {
      Id = Guid.NewGuid(),
      FromLocationId = homeId,
      FromRoomId = livingRoomId,
      ToLocationId = homeId,
      ToRoomId = bedroomId,
      RequiredItemId = null
    };
    var movementHomeBedroomToHomeLivingRoom = new MovementDataModel
    {
      Id = Guid.NewGuid(),
      FromLocationId = homeId,
      FromRoomId = bedroomId,
      ToLocationId = homeId,
      ToRoomId = livingRoomId,
      RequiredItemId = null
    };
    var movementHomeLivingRoomToStreet = new MovementDataModel
    {
      Id = Guid.NewGuid(),
      FromLocationId = homeId,
      FromRoomId = livingRoomId,
      ToLocationId = streetId,
      ToRoomId = null,
      RequiredItemId = null
    };
    var movementStreetToHomeLivingRoom = new MovementDataModel
    {
      Id = Guid.NewGuid(),
      FromLocationId = streetId,
      FromRoomId = null,
      ToLocationId = homeId,
      ToRoomId = livingRoomId,
      RequiredItemId = null
    };
    var movementStreetToCollege = new MovementDataModel
    {
      Id = Guid.NewGuid(),
      FromLocationId = streetId,
      FromRoomId = null,
      ToLocationId = collegeId,
      ToRoomId = null,
      RequiredItemId = null
    };
    var movementCollegeToStreet = new MovementDataModel
    {
      Id = Guid.NewGuid(),
      FromLocationId = collegeId,
      FromRoomId = null,
      ToLocationId = streetId,
      ToRoomId = null,
      RequiredItemId = null
    };

    await context.Movements.AddRangeAsync(
      movementHomeLivingRoomToHomeBedroom, movementHomeBedroomToHomeLivingRoom, movementHomeLivingRoomToStreet,
      movementStreetToHomeLivingRoom, movementStreetToCollege, movementCollegeToStreet
    );

    await context.SaveChangesAsync().ConfigureAwait(false);

    var openingHours =
      new[] {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday}
        .Select(day => new LocationOpeningHoursDataModel
          {
            Id = Guid.NewGuid(),
            LocationId = collegeId,
            DayOfWeek = day,
            OpensAt = new TimeSpan(8, 0, 0),
            ClosesAt = new TimeSpan(18, 0, 0)
          }
        ).ToList();

    await context.LocationOpeningHours.AddRangeAsync(openingHours);

    const string streetMovementNarration = "You step outside to the street.";

    await context.MovementNarrations.AddRangeAsync(
      new MovementNarrationDataModel
      {
        Id = Guid.NewGuid(),
        MovementId = movementHomeLivingRoomToHomeBedroom.Id,
        Text = "You enter your bedroom."
      }, new MovementNarrationDataModel
      {
        Id = Guid.NewGuid(),
        MovementId = movementHomeBedroomToHomeLivingRoom.Id,
        Text = "You walk into the living room."
      }, new MovementNarrationDataModel
      {
        Id = Guid.NewGuid(),
        MovementId = movementHomeLivingRoomToStreet.Id,
        Text = streetMovementNarration
      }, new MovementNarrationDataModel
      {
        Id = Guid.NewGuid(),
        MovementId = movementStreetToHomeLivingRoom.Id,
        Text = "You return to your home."
      }, new MovementNarrationDataModel
      {
        Id = Guid.NewGuid(),
        MovementId = movementStreetToCollege.Id,
        Text = "You enter the college."
      }, new MovementNarrationDataModel
      {
        Id = Guid.NewGuid(),
        MovementId = movementCollegeToStreet.Id,
        Text = streetMovementNarration
      }
    );
  }

  #endregion
}
