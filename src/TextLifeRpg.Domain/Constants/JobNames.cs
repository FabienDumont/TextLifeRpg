namespace TextLifeRpg.Domain.Constants;

public static class JobNames
{
  public const string GarbageCollector = "Garbage collector";
  public const string Janitor = "Janitor";
  public const string DeliveryDriver = "Delivery driver";
  public const string CollegeTeacher = "College teacher";

  public static readonly IReadOnlyList<string> All = [GarbageCollector, Janitor, DeliveryDriver, CollegeTeacher];
}
