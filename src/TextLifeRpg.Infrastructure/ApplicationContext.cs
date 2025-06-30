using Microsoft.EntityFrameworkCore;
using TextLifeRpg.Infrastructure.EfDataModels;
using TextLifeRpg.Infrastructure.Seeders;

namespace TextLifeRpg.Infrastructure;

public class ApplicationContext : DbContext
{
  #region Properties

  /// <summary>
  /// Represents all traits in the database.
  /// </summary>
  public virtual DbSet<TraitDataModel> Traits { get; init; }

  /// <summary>
  /// Represents pairs of incompatible traits.
  /// </summary>
  public virtual DbSet<TraitIncompatibilityDataModel> TraitIncompatibilities { get; init; }

  /// <summary>
  /// Represents all greetings used in NPC interactions.
  /// </summary>
  public virtual DbSet<GreetingDataModel> Greetings { get; init; }

  /// <summary>
  /// Represents all conditions used in NPC interactions.
  /// </summary>
  public virtual DbSet<ConditionDataModel> Conditions { get; init; }

  /// <summary>
  /// Represents all locations in the world.
  /// </summary>
  public virtual DbSet<LocationDataModel> Locations { get; init; }

  /// <summary>
  /// Represents all locations' opening hours in the world.
  /// </summary>
  public virtual DbSet<LocationOpeningHoursDataModel> LocationOpeningHours { get; init; }

  /// <summary>
  /// Represents rooms inside locations.
  /// </summary>
  public virtual DbSet<RoomDataModel> Rooms { get; init; }

  /// <summary>
  /// Represents movements between locations or rooms.
  /// </summary>
  public virtual DbSet<MovementDataModel> Movements { get; init; }

  /// <summary>
  /// Represents narration texts tied to movements.
  /// </summary>
  public virtual DbSet<MovementNarrationDataModel> MovementNarrations { get; init; }

  /// <summary>
  /// Represents narration texts.
  /// </summary>
  public virtual DbSet<NarrationDataModel> Narrations { get; init; }

  /// <summary>
  /// Represents exploration actions.
  /// </summary>
  public virtual DbSet<ExplorationActionDataModel> ExplorationActions { get; init; }

  /// <summary>
  /// Represents exploration actions' results.
  /// </summary>
  public virtual DbSet<ExplorationActionResultDataModel> ExplorationActionResults { get; init; }

  /// <summary>
  /// Represents exploration action results' narrations.
  /// </summary>
  public virtual DbSet<ExplorationActionResultNarrationDataModel> ExplorationActionResultNarrations { get; init; }

  /// <summary>
  /// Represents all jobs in the database.
  /// </summary>
  public virtual DbSet<JobDataModel> Jobs { get; init; }

  #endregion

  #region Ctors

  /// <summary>
  /// Initializes a new instance of <see cref="ApplicationContext" /> with options.
  /// </summary>
  public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
  {
  }

  /// <summary>
  /// Parameterless constructor for design-time tools and migrations.
  /// </summary>
  public ApplicationContext()
  {
  }

  #endregion

  #region Methods

  /// <summary>
  /// Seeds the database with initial data if not already populated.
  /// </summary>
  public async Task InitializeDataAsync()
  {
    if (await Traits.AnyAsync().ConfigureAwait(false))
    {
      return;
    }

    var seeders = new IDataSeeder[]
    {
      new TraitSeeder(),
      new GreetingSeeder(),
      new LocationSeeder(),
      new ExplorationActionSeeder(),
      new NarrationSeeder(),
      new JobSeeder()
    };

    foreach (var seeder in seeders)
    {
      await seeder.SeedAsync(this);
      await SaveChangesAsync();
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<LocationOpeningHoursDataModel>().HasOne(oh => oh.Location).WithMany(l => l.OpeningHours)
      .HasForeignKey(oh => oh.LocationId).OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<LocationDataModel>().Navigation(l => l.OpeningHours).AutoInclude();

    modelBuilder.Entity<RoomDataModel>().HasOne(n => n.Location).WithMany().HasForeignKey(n => n.LocationId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<MovementNarrationDataModel>().HasOne(n => n.Movement).WithMany()
      .HasForeignKey(n => n.MovementId).OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<TraitIncompatibilityDataModel>().HasOne(t => t.Trait1).WithMany().HasForeignKey(t => t.Trait1Id)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<TraitIncompatibilityDataModel>().HasOne(t => t.Trait2).WithMany().HasForeignKey(t => t.Trait2Id)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<ExplorationActionDataModel>().HasOne(e => e.Location).WithMany()
      .HasForeignKey(e => e.LocationId).OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<ExplorationActionDataModel>().HasOne(e => e.Room).WithMany().HasForeignKey(e => e.RoomId)
      .OnDelete(DeleteBehavior.SetNull);

    modelBuilder.Entity<ExplorationActionResultDataModel>().HasOne(r => r.ExplorationAction).WithMany()
      .HasForeignKey(r => r.ExplorationActionId).OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<ExplorationActionResultNarrationDataModel>().HasOne(n => n.ExplorationActionResult).WithMany()
      .HasForeignKey(n => n.ExplorationActionResultId).OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<MovementDataModel>().HasOne(m => m.FromLocation).WithMany().HasForeignKey(m => m.FromLocationId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<MovementDataModel>().HasOne(m => m.ToLocation).WithMany().HasForeignKey(m => m.ToLocationId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<MovementDataModel>().HasOne(m => m.FromRoom).WithMany().HasForeignKey(m => m.FromRoomId)
      .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<MovementDataModel>().HasOne(m => m.ToRoom).WithMany().HasForeignKey(m => m.ToRoomId)
      .OnDelete(DeleteBehavior.Cascade);
  }

  #endregion
}
