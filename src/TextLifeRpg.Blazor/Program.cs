using TextLifeRpg.Application;
using TextLifeRpg.Blazor.Components;
using TextLifeRpg.Blazor.Stores;
using TextLifeRpg.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddInfrastructure("Data/database.db");
builder.Services.AddApplication();
builder.Services.AddScoped<GameSaveStore>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

  try
  {
    var created = await db.Database.EnsureCreatedAsync();

    if (created)
    {
      Console.WriteLine("[DB] Created new database. Seeding data...");
      await db.InitializeDataAsync();
    }
    else
    {
      Console.WriteLine("[DB] Database already exists. Skipping seeding.");
    }

    Console.WriteLine("[DB] Ready.");
  }
  catch (Exception ex)
  {
    Console.WriteLine($"[DB] Initialization failed: {ex.Message}");
  }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
