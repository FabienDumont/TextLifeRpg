using TextLifeRpg.Application.Abstraction;
using TextLifeRpg.Domain;

namespace TextLifeRpg.Application.Services;

public class ScheduleService(ILocationService locationService, IRandomProvider randomProvider) : IScheduleService
{
  public async Task<List<Schedule>> GenerateSchedulesAsync(
    IEnumerable<Character> characters, DateOnly currentDate, CancellationToken cancellationToken
  )
  {
    var streetLocation = await locationService.GetByNameAsync("Street", cancellationToken);

    if (streetLocation is null)
    {
      throw new InvalidOperationException("Street location not found.");
    }

    var streetWeeklySlots = new Dictionary<DayOfWeek, List<(TimeSpan Start, TimeSpan End)>>();

    foreach (var day in Enum.GetValues<DayOfWeek>())
    {
      var allowedDurations = new[]
      {
        TimeSpan.FromHours(1),
        TimeSpan.FromHours(2),
        TimeSpan.FromHours(3)
      };

      var slots = streetLocation.GetTimeSlots(day, allowedDurations);
      if (slots.Count > 0)
      {
        streetWeeklySlots[day] = slots;
      }
    }

    var schedules = new List<Schedule>();

    foreach (var character in characters)
    {
      var entries = new List<ScheduleEntry>();

      foreach (var day in Enum.GetValues<DayOfWeek>())
      {
        if (!streetWeeklySlots.TryGetValue(day, out var daySlots) || daySlots.Count == 0) continue;

        var (start, end) = daySlots.OrderBy(_ => randomProvider.NextDouble()).First();

        entries.Add(new ScheduleEntry(day, start, end, streetLocation.Id, null));
      }

      schedules.Add(Schedule.Create(character.Id, entries));
    }

    return schedules;
  }
}
