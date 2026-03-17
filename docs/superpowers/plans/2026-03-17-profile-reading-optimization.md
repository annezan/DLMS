# Profile Reading Optimization Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Optimize DLMS profile reading with incremental reads and per-profile prioritization to reduce data staleness from 12h to <1h.

**Architecture:** Add a `MeterProfileReadHistory` table to track last-read timestamps per meter/profile. Replace the single batch profile read with sequential per-profile reads (P3 > P1 > Monthly > P2 > Events), each with its own timeout and incremental date range. Add a unique index on `Gxdlmsprofilgenericdetails` to replace in-memory deduplication.

**Tech Stack:** C# .NET 8, Entity Framework Core, SQL Server, EFCore.BulkExtensions, Gurux DLMS

**Spec:** `docs/superpowers/specs/2026-03-17-profile-reading-optimization-design.md`

---

## Chunk 1: Data Layer (Entity + Repository + Migration)

### Task 1: SQL Migration Script

**Files:**
- Create: `scripts/migrate-profile-optimization.sql`

- [ ] **Step 1: Create migration script**

```sql
-- =============================================
-- Migration: Profile Reading Optimization
-- Date: 2026-03-17
-- =============================================

USE [db_ac2526_dlmsdb_v1_Test];
GO

-- 1. Table MeterProfileReadHistory
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'MeterProfileReadHistory')
BEGIN
    CREATE TABLE [dbo].[MeterProfileReadHistory] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [CompteurSerial] NVARCHAR(50) NOT NULL,
        [ProfileObis] NVARCHAR(20) NOT NULL,
        [LastReadUpTo] DATETIME2 NOT NULL,
        [LastReadAt] DATETIME2 NOT NULL,
        [RowsRead] INT NOT NULL DEFAULT 0,
        [ReadDurationMs] BIGINT NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NULL,
        [UpdatedAt] DATETIME2 NULL,
        [DeletedAt] DATETIME2 NULL,
        [CreatedBy] NVARCHAR(100) NOT NULL DEFAULT '',
        [UpdatedBy] NVARCHAR(100) NOT NULL DEFAULT '',
        [DeletedBy] NVARCHAR(100) NOT NULL DEFAULT '',
        [IsArchive] BIT NOT NULL DEFAULT 0,
        CONSTRAINT [UQ_MeterProfile] UNIQUE ([CompteurSerial], [ProfileObis])
    );

    CREATE INDEX [IX_MeterProfileReadHistory_Serial]
        ON [dbo].[MeterProfileReadHistory]([CompteurSerial]);

    PRINT 'Table MeterProfileReadHistory created.';
END
GO

-- 2. Unique index on Gxdlmsprofilgenericdetails (prerequisite for dedup removal)
-- Step 2a: Remove existing duplicates (keep most recent Id)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_ProfileDetail_NoDup')
BEGIN
    PRINT 'Cleaning duplicates from Gxdlmsprofilgenericdetails...';

    WITH cte AS (
        SELECT [Id],
               ROW_NUMBER() OVER (
                   PARTITION BY [NumeroCompteur], [GxdlmsprofilgenericId], [CodeObisId], [DateEnr]
                   ORDER BY [Id] DESC
               ) AS rn
        FROM [dbo].[Gxdlmsprofilgenericdetails]
        WHERE [DateEnr] IS NOT NULL
    )
    DELETE FROM cte WHERE rn > 1;

    PRINT 'Duplicates cleaned.';

    -- Step 2b: Create unique filtered index
    CREATE UNIQUE INDEX [UQ_ProfileDetail_NoDup]
        ON [dbo].[Gxdlmsprofilgenericdetails]([NumeroCompteur], [GxdlmsprofilgenericId], [CodeObisId], [DateEnr])
        WHERE [DateEnr] IS NOT NULL;

    PRINT 'Unique index UQ_ProfileDetail_NoDup created.';
END
GO

-- 3. Seed profile reading configuration
IF NOT EXISTS (SELECT 1 FROM [dbo].[ReadingConfigurations] WHERE [Cle] = 'ProfilePriority_1')
BEGIN
    INSERT INTO [dbo].[ReadingConfigurations] ([Cle], [Valeur], [Description], [GroupeConfig], [CreatedAt], [CreatedBy], [IsArchive])
    VALUES
        ('ProfilePriority_1', '1.0.99.3.0.255', 'Profil 3 (24h) — priorite 1', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_1.0.99.3.0.255', '30', 'Timeout en secondes pour Profil 3', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileFallback_1.0.99.3.0.255', '48', 'Fallback max en heures pour Profil 3', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_2', '1.0.99.1.0.255', 'Profil 1 (1h) — priorite 2', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_1.0.99.1.0.255', '60', 'Timeout en secondes pour Profil 1', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileFallback_1.0.99.1.0.255', '48', 'Fallback max en heures pour Profil 1', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_3', '1.0.99.2.0.255', 'Profil 2 (5min) — priorite 3', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_1.0.99.2.0.255', '120', 'Timeout en secondes pour Profil 2', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileFallback_1.0.99.2.0.255', '12', 'Fallback max en heures pour Profil 2', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_4', '0.0.98.1.0.255', 'Fault record — priorite 4', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_0.0.98.1.0.255', '20', 'Timeout en secondes pour Fault record', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileFallback_0.0.98.1.0.255', '48', 'Fallback max en heures pour Fault record', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_5', '0.0.99.98.0.255', 'Event 0 — priorite 5', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_0.0.99.98.0.255', '20', 'Timeout pour Event 0', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileFallback_0.0.99.98.0.255', '48', 'Fallback max en heures pour Event 0', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_6', '0.0.99.98.1.255', 'Event 1 — priorite 6', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_0.0.99.98.1.255', '20', 'Timeout pour Event 1', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_7', '0.0.99.98.2.255', 'Event 2 — priorite 7', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_0.0.99.98.2.255', '20', 'Timeout pour Event 2', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_8', '0.0.99.98.3.255', 'Event 3 — priorite 8', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_0.0.99.98.3.255', '20', 'Timeout pour Event 3', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_9', '0.0.99.98.4.255', 'Event 4 — priorite 9', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_0.0.99.98.4.255', '20', 'Timeout pour Event 4', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_10', '0.0.99.98.5.255', 'Event 5 — priorite 10', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_0.0.99.98.5.255', '20', 'Timeout pour Event 5', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_11', '0.0.99.98.6.255', 'Event 6 — priorite 11', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_0.0.99.98.6.255', '20', 'Timeout pour Event 6', 'ProfileReading', GETDATE(), 'migration', 0),

        ('ProfilePriority_12', '0.0.99.98.7.255', 'Event 7 — priorite 12', 'ProfileReading', GETDATE(), 'migration', 0),
        ('ProfileTimeout_0.0.99.98.7.255', '20', 'Timeout pour Event 7', 'ProfileReading', GETDATE(), 'migration', 0);

    PRINT 'Profile reading configuration seeded.';
END
GO
```

- [ ] **Step 2: Commit**

```bash
git add scripts/migrate-profile-optimization.sql
git commit -m "feat: add migration script for profile reading optimization"
```

---

### Task 2: MeterProfileReadHistory Entity

**Files:**
- Create: `DLMS_MODELS/ReadingDomain/Entities/MeterProfileReadHistory.cs`

- [ ] **Step 1: Create entity file**

```csharp
using DLMS_MODELS.Bases;

namespace DLMS_MODELS.ReadingDomain.Entities;

public class MeterProfileReadHistory : AuditableEntity
{
    public int Id { get; set; }
    public string CompteurSerial { get; set; } = string.Empty;
    public string ProfileObis { get; set; } = string.Empty;
    public DateTime LastReadUpTo { get; set; }
    public DateTime LastReadAt { get; set; }
    public int RowsRead { get; set; }
    public long ReadDurationMs { get; set; }
}
```

- [ ] **Step 2: Register in DbContext**

Modify: `DLMS_DAL/Datas/DLMSDBContext.cs:82` — add after line 82 (inside `#region Multi-Pass Reading`):

```csharp
public virtual DbSet<MeterProfileReadHistory> MeterProfileReadHistories { get; set; }
```

Add the using at the top if not already present (ReadingDomain.Entities is already imported via existing entities).

- [ ] **Step 3: Add entity configuration in OnModelCreating**

Modify: `DLMS_DAL/Datas/DLMSDBContext.cs` — add after the existing Multi-Pass entity configs:

```csharp
modelBuilder.Entity<MeterProfileReadHistory>(entity =>
{
    entity.ToTable("MeterProfileReadHistory");
    entity.HasIndex(e => new { e.CompteurSerial, e.ProfileObis }).IsUnique();
    entity.HasIndex(e => e.CompteurSerial);
});
```

- [ ] **Step 4: Verify build**

Run: `dotnet build DLMS_SERVICE/DLMS_SERVICE.csproj`
Expected: Build succeeded

- [ ] **Step 5: Commit**

```bash
git add DLMS_MODELS/ReadingDomain/Entities/MeterProfileReadHistory.cs DLMS_DAL/Datas/DLMSDBContext.cs
git commit -m "feat: add MeterProfileReadHistory entity and DbContext registration"
```

---

### Task 3: MeterProfileReadHistory Repository

**Files:**
- Create: `DLMS_DAL/ReadingDomainDal/Repositories/Queries/IMeterProfileReadHistoryQueryRepository.cs`
- Create: `DLMS_DAL/ReadingDomainDal/Repositories/Queries/MeterProfileReadHistoryQueryRepository.cs`
- Create: `DLMS_DAL/ReadingDomainDal/Repositories/Commands/IMeterProfileReadHistoryCommandRepository.cs`
- Create: `DLMS_DAL/ReadingDomainDal/Repositories/Commands/MeterProfileReadHistoryCommandRepository.cs`
- Modify: `DLMS_DAL/DependencyInjection.cs:111`

- [ ] **Step 1: Create query interface**

```csharp
using DLMS_MODELS.ReadingDomain.Entities;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Queries;

public interface IMeterProfileReadHistoryQueryRepository
{
    Task<MeterProfileReadHistory?> GetLastReadAsync(string compteurSerial, string profileObis);
    Task<List<MeterProfileReadHistory>> GetAllForMeterAsync(string compteurSerial);
}
```

- [ ] **Step 2: Create query implementation**

```csharp
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.ReadingDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Queries;

public class MeterProfileReadHistoryQueryRepository
    : QueryBaseRepository<MeterProfileReadHistory>, IMeterProfileReadHistoryQueryRepository
{
    public MeterProfileReadHistoryQueryRepository(DLMSDBContext context) : base(context)
    {
    }

    public async Task<MeterProfileReadHistory?> GetLastReadAsync(string compteurSerial, string profileObis)
    {
        return await _context.MeterProfileReadHistories
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.CompteurSerial == compteurSerial && h.ProfileObis == profileObis);
    }

    public async Task<List<MeterProfileReadHistory>> GetAllForMeterAsync(string compteurSerial)
    {
        return await _context.MeterProfileReadHistories
            .AsNoTracking()
            .Where(h => h.CompteurSerial == compteurSerial)
            .ToListAsync();
    }
}
```

- [ ] **Step 3: Create command interface**

```csharp
using DLMS_MODELS.ReadingDomain.Entities;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Commands;

public interface IMeterProfileReadHistoryCommandRepository
{
    Task UpsertAsync(string compteurSerial, string profileObis, DateTime lastReadUpTo, int rowsRead, long durationMs);
}
```

- [ ] **Step 4: Create command implementation**

```csharp
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.ReadingDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Commands;

public class MeterProfileReadHistoryCommandRepository
    : CommandRepository<MeterProfileReadHistory>, IMeterProfileReadHistoryCommandRepository
{
    public MeterProfileReadHistoryCommandRepository(DLMSDBContext context) : base(context)
    {
    }

    public async Task UpsertAsync(string compteurSerial, string profileObis, DateTime lastReadUpTo, int rowsRead, long durationMs)
    {
        var existing = await _context.MeterProfileReadHistories
            .FirstOrDefaultAsync(h => h.CompteurSerial == compteurSerial && h.ProfileObis == profileObis);

        if (existing != null)
        {
            existing.LastReadUpTo = lastReadUpTo;
            existing.LastReadAt = DateTime.Now;
            existing.RowsRead = rowsRead;
            existing.ReadDurationMs = durationMs;
            existing.UpdatedAt = DateTime.Now;
        }
        else
        {
            await _context.MeterProfileReadHistories.AddAsync(new MeterProfileReadHistory
            {
                CompteurSerial = compteurSerial,
                ProfileObis = profileObis,
                LastReadUpTo = lastReadUpTo,
                LastReadAt = DateTime.Now,
                RowsRead = rowsRead,
                ReadDurationMs = durationMs,
                CreatedAt = DateTime.Now
            });
        }

        await _context.SaveChangesAsync();
    }
}
```

- [ ] **Step 5: Register in DI container**

Modify: `DLMS_DAL/DependencyInjection.cs:111` — add after the existing Reading Domain registrations:

```csharp
services.AddTransient<IMeterProfileReadHistoryQueryRepository, MeterProfileReadHistoryQueryRepository>();
services.AddTransient<IMeterProfileReadHistoryCommandRepository, MeterProfileReadHistoryCommandRepository>();
```

Add the using statements at the top of the file:
```csharp
using DLMS_DAL.ReadingDomainDal.Repositories.Commands;  // already exists (line 46)
using DLMS_DAL.ReadingDomainDal.Repositories.Queries;    // already exists (line 47)
```

- [ ] **Step 6: Verify build**

Run: `dotnet build DLMS_SERVICE/DLMS_SERVICE.csproj`
Expected: Build succeeded

- [ ] **Step 7: Commit**

```bash
git add DLMS_DAL/ReadingDomainDal/Repositories/ DLMS_DAL/DependencyInjection.cs
git commit -m "feat: add MeterProfileReadHistory repository with upsert"
```

---

## Chunk 2: Service Layer (ProfileReadResult + ProcessAndSaveSingleProfile + ProfileConfig)

### Task 4: Add ProfileReadResult to SessionModels

**Files:**
- Modify: `DLMS_SERVICE/Services/MultiPass/SessionModels.cs:21`

- [ ] **Step 1: Add ProfileReadResult class and update MeterReadOutcome**

Add after `MeterReadOutcome` class (after line 21):

```csharp
public class ProfileReadResult
{
    public string ProfileObis { get; set; } = "";
    public bool Success { get; set; }
    public int RowsRead { get; set; }
    public long DurationMs { get; set; }
    public string Error { get; set; } = "";
}
```

Add to `MeterReadOutcome` class (inside, after line 20):

```csharp
public List<ProfileReadResult> ProfileResults { get; set; } = new();
```

- [ ] **Step 2: Verify build**

Run: `dotnet build DLMS_SERVICE/DLMS_SERVICE.csproj`
Expected: Build succeeded

- [ ] **Step 3: Commit**

```bash
git add DLMS_SERVICE/Services/MultiPass/SessionModels.cs
git commit -m "feat: add ProfileReadResult and ProfileResults to MeterReadOutcome"
```

---

### Task 5: ProfileReadingConfig Helper

**Files:**
- Create: `DLMS_SERVICE/Services/MultiPass/ProfileReadingConfig.cs`

- [ ] **Step 1: Create config helper**

This loads profile reading configuration from the `ReadingConfiguration` table and provides defaults.

```csharp
using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE.Services.MultiPass;

public class ProfileReadingEntry
{
    public int Priority { get; set; }
    public string ProfileObis { get; set; } = "";
    public int TimeoutSeconds { get; set; }
    public int FallbackMaxHours { get; set; }
}

public interface IProfileReadingConfig
{
    Task<List<ProfileReadingEntry>> GetOrderedProfilesAsync();
}

public class ProfileReadingConfig : IProfileReadingConfig
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProfileReadingConfig> _logger;
    private List<ProfileReadingEntry>? _cached;

    // Defaults if DB config is missing
    private static readonly List<ProfileReadingEntry> Defaults = new()
    {
        new() { Priority = 1, ProfileObis = "1.0.99.3.0.255", TimeoutSeconds = 30, FallbackMaxHours = 48 },
        new() { Priority = 2, ProfileObis = "1.0.99.1.0.255", TimeoutSeconds = 60, FallbackMaxHours = 48 },
        new() { Priority = 3, ProfileObis = "1.0.99.2.0.255", TimeoutSeconds = 120, FallbackMaxHours = 12 },
        new() { Priority = 4, ProfileObis = "0.0.98.1.0.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 5, ProfileObis = "0.0.99.98.0.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 6, ProfileObis = "0.0.99.98.1.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 7, ProfileObis = "0.0.99.98.2.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 8, ProfileObis = "0.0.99.98.3.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 9, ProfileObis = "0.0.99.98.4.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 10, ProfileObis = "0.0.99.98.5.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 11, ProfileObis = "0.0.99.98.6.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
        new() { Priority = 12, ProfileObis = "0.0.99.98.7.255", TimeoutSeconds = 20, FallbackMaxHours = 48 },
    };

    public ProfileReadingConfig(IServiceProvider serviceProvider, ILogger<ProfileReadingConfig> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<List<ProfileReadingEntry>> GetOrderedProfilesAsync()
    {
        if (_cached != null) return _cached;

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();

            var configs = await context.ReadingConfigurations
                .AsNoTracking()
                .Where(c => c.GroupeConfig == "ProfileReading" && !c.IsArchive)
                .ToListAsync();

            if (configs.Count == 0)
            {
                _logger.LogWarning("No ProfileReading config found in DB, using defaults");
                _cached = Defaults;
                return _cached;
            }

            var result = new List<ProfileReadingEntry>();
            var priorityConfigs = configs.Where(c => c.Cle.StartsWith("ProfilePriority_")).OrderBy(c => c.Cle);

            foreach (var pc in priorityConfigs)
            {
                var obis = pc.Valeur;
                var priorityNum = int.Parse(pc.Cle.Replace("ProfilePriority_", ""));

                var timeoutConfig = configs.FirstOrDefault(c => c.Cle == $"ProfileTimeout_{obis}");
                var fallbackConfig = configs.FirstOrDefault(c => c.Cle == $"ProfileFallback_{obis}");

                result.Add(new ProfileReadingEntry
                {
                    Priority = priorityNum,
                    ProfileObis = obis,
                    TimeoutSeconds = timeoutConfig != null ? int.Parse(timeoutConfig.Valeur) : 30,
                    FallbackMaxHours = fallbackConfig != null ? int.Parse(fallbackConfig.Valeur) : 48,
                });
            }

            _cached = result.OrderBy(r => r.Priority).ToList();
            return _cached;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading ProfileReading config, using defaults");
            _cached = Defaults;
            return _cached;
        }
    }
}
```

- [ ] **Step 2: Register in DI**

Modify: `DLMS_SERVICE/Program.cs:144` — add after the MultiPass section (after `services.AddTransient<IReadingCycleManager, ReadingCycleManager>();`):

```csharp
services.AddSingleton<IProfileReadingConfig, ProfileReadingConfig>();
```

Add the using at the top of Program.cs:
```csharp
using DLMS_SERVICE.Services.MultiPass;
```

- [ ] **Step 3: Verify build**

Run: `dotnet build DLMS_SERVICE/DLMS_SERVICE.csproj`
Expected: Build succeeded

- [ ] **Step 4: Commit**

```bash
git add DLMS_SERVICE/Services/MultiPass/ProfileReadingConfig.cs
git commit -m "feat: add ProfileReadingConfig with DB-backed priority/timeout config"
```

---

### Task 6: ProcessAndSaveSingleProfileAsync

**Files:**
- Modify: `DLMS_SERVICE/Services/DLMSHardwareService.cs:30` (interface)
- Modify: `DLMS_SERVICE/Services/DLMSHardwareService.cs:317` (add new method)

- [ ] **Step 1: Add method to interface**

Modify: `DLMS_SERVICE/Services/DLMSHardwareService.cs:30` — add to `IDLMSHardwareService` interface:

```csharp
Task<int> ProcessAndSaveSingleProfileAsync(string data, string serialNumber, string profileObis);
```

- [ ] **Step 2: Implement the method**

Add new method in `DLMSHardwareService` class. This mirrors the exact parsing logic of the existing `ProcessAndSaveProfileDataAsync` (lines 401-527) but for a single profile, and uses `BulkInsertAsync` with `InsertIfNotExists` instead of in-memory deduplication.

**Key data format**: `ReadRowsByRangeAsync` returns JSON where each entry has:
- `entry.Key` = array of rows, each row is `IEnumerable<object>` of column values
- `entry.Value` = array of OBIS column names (strings)
- `processedCount < 4` means register profile (Profils 1/2/3 + fault), `>= 4` means event profile

Since we read one profile at a time, `isEventProfile` is determined by the `profileObis` parameter.

```csharp
/// <summary>
/// Processes and saves data for a single DLMS profile. Uses BulkInsert with InsertIfNotExists
/// instead of in-memory deduplication. Returns number of rows processed.
/// </summary>
public async Task<int> ProcessAndSaveSingleProfileAsync(string data, string serialNumber, string profileObis)
{
    if (string.IsNullOrEmpty(data) || data == "Lecture impossible") return 0;

    try
    {
        var entries = JsonConvert.DeserializeObject<List<KeyValuePair<object[], object[]>>>(data);
        if (entries == null || entries.Count == 0) return 0;

        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();

        // Lookup profile generic via CodeObis join (same pattern as line 344-352)
        var profilGeneric = await context.Gxdlmsprofilgenerics
            .Join(context.CodeObis,
                pg => pg.CodeObisId,
                co => co.Id,
                (pg, co) => new { ProfilGeneric = pg, CodeObisValue = co.Value })
            .FirstOrDefaultAsync(x => x.CodeObisValue == profileObis);

        if (profilGeneric == null)
        {
            _logger.LogWarning("Profil non trouve pour {ProfileObis}", profileObis);
            return 0;
        }

        // Preload OBIS codes from entry.Value (column names)
        var allObisValues = entries
            .SelectMany(e => e.Value.Select(v => v?.ToString()))
            .Where(s => !string.IsNullOrEmpty(s))
            .Distinct()
            .ToList();

        var codeObisDict = (await context.CodeObis
            .Where(c => allObisValues.Contains(c.Value))
            .ToListAsync())
            .GroupBy(c => c.Value)
            .ToDictionary(g => g.Key, g => g.First());

        // Determine if this is a register profile (Profil 1/2/3 + fault) or event profile
        var registerProfiles = new HashSet<string>
        {
            "1.0.99.1.0.255", "1.0.99.2.0.255", "1.0.99.3.0.255", "0.0.98.1.0.255"
        };
        bool isRegisterProfile = registerProfiles.Contains(profileObis);

        // Preload events dict if needed
        Dictionary<int, Events> eventsDict = null!;
        if (!isRegisterProfile)
        {
            eventsDict = (await context.Events
                .Where(e => e.Category == "EVENTS_GROUP_ALL_REGISTERS")
                .ToListAsync())
                .GroupBy(e => e.Value)
                .ToDictionary(g => g.Key, g => g.First());
        }

        context.ChangeTracker.AutoDetectChangesEnabled = false;
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        var detailsToAdd = new List<Gxdlmsprofilgenericdetail>();
        var eventsToAdd = new List<Gxdlmsprofilgenericdetailsevent>();

        // Process the single entry (ReadRowsByRange with 1 OBIS returns 1 entry)
        foreach (var entry in entries.Take(1))
        {
            if (entry.Key.Length == 0 || entry.Value.Length == 0) continue;

            DateTime dateUtc = DateTime.UtcNow;

            // entry.Key = rows, each row is IEnumerable<object> values
            foreach (var row in entry.Key)
            {
                if (row is not IEnumerable<object> values) continue;
                var array = values.ToArray();

                // entry.Value = OBIS column names
                for (int i = 0; i < entry.Value.Length; i++)
                {
                    var objStr = entry.Value[i]?.ToString() ?? string.Empty;

                    if (isRegisterProfile)
                    {
                        var detailprofil = new Gxdlmsprofilgenericdetail();
                        var realValue = ((JValue)array[i]).Value;
                        bool isNumeric = realValue is decimal || realValue is int
                            || realValue is long || realValue is Int64 || realValue is Int32;

                        if (i == 0)
                        {
                            if (DateTime.TryParse(array[i].ToString(), out DateTime dateValue))
                            {
                                long unixTs = ((DateTimeOffset)dateValue).ToUnixTimeSeconds();
                                dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTs).UtcDateTime;
                            }
                            else
                            {
                                _logger.LogWarning("Format de date invalide: {Date}", array[i]);
                                continue;
                            }
                        }

                        detailprofil.Value = isNumeric ? Convert.ToDecimal(realValue).ToString() : realValue?.ToString();
                        detailprofil.CodeObisId = codeObisDict.TryGetValue(objStr, out var co) ? co.Id : 0;
                        detailprofil.DateEnr = dateUtc;
                        detailprofil.GxdlmsprofilgenericId = profilGeneric.ProfilGeneric.Id;
                        detailprofil.NumeroCompteur = serialNumber;
                        detailprofil.IsArchive = false;
                        detailsToAdd.Add(detailprofil);
                    }
                    else
                    {
                        var evt = new Gxdlmsprofilgenericdetailsevent();

                        if (objStr.StartsWith("0.0.96.11."))
                        {
                            int valconvert = Convert.ToInt32(array[i]);
                            if (eventsDict.TryGetValue(valconvert, out var eventResult))
                                evt.EventId = eventResult?.Id;
                            else
                                evt.EventId = null;
                            evt.Value = array[i]?.ToString() ?? string.Empty;
                        }
                        else
                        {
                            evt.Value = array[i]?.ToString() ?? string.Empty;
                            evt.EventId = null;
                        }

                        if (i == 0)
                        {
                            if (DateTime.TryParse(array[i].ToString(), out DateTime dateValue))
                            {
                                long unixTs = ((DateTimeOffset)dateValue).ToUnixTimeSeconds();
                                dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTs).UtcDateTime;
                            }
                            else
                            {
                                _logger.LogWarning("Format de date invalide: {Date}", array[i]);
                                continue;
                            }
                        }

                        evt.DateEnr = dateUtc;
                        evt.CodeObisId = codeObisDict.TryGetValue(objStr, out var co) ? co.Id : 0;
                        evt.GxdlmsprofilgenericId = profilGeneric.ProfilGeneric.Id;
                        evt.NumeroCompteur = serialNumber;
                        evt.IsArchive = false;
                        eventsToAdd.Add(evt);
                    }
                }
            }
        }

        int totalInserted = 0;

        // BulkInsert with InsertIfNotExists (relies on UQ_ProfileDetail_NoDup index)
        if (detailsToAdd.Count > 0)
        {
            await context.BulkInsertAsync(detailsToAdd, new BulkConfig
            {
                SetOutputIdentity = false,
                BatchSize = BatchSize,
                InsertIfNotExists = true,
                PropertiesToIncludeOnCompare = new List<string>
                {
                    nameof(Gxdlmsprofilgenericdetail.NumeroCompteur),
                    nameof(Gxdlmsprofilgenericdetail.GxdlmsprofilgenericId),
                    nameof(Gxdlmsprofilgenericdetail.CodeObisId),
                    nameof(Gxdlmsprofilgenericdetail.DateEnr)
                }
            });
            totalInserted += detailsToAdd.Count;
        }

        if (eventsToAdd.Count > 0)
        {
            await context.BulkInsertAsync(eventsToAdd, new BulkConfig
            {
                SetOutputIdentity = false,
                BatchSize = BatchSize,
                InsertIfNotExists = true
            });
            totalInserted += eventsToAdd.Count;
        }

        return totalInserted;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erreur sauvegarde profil {Obis} pour {Serial}", profileObis, serialNumber);
        return 0;
    }
}
```

- [ ] **Step 3: Verify build**

Run: `dotnet build DLMS_SERVICE/DLMS_SERVICE.csproj`
Expected: Build succeeded

- [ ] **Step 4: Commit**

```bash
git add DLMS_SERVICE/Services/DLMSHardwareService.cs
git commit -m "feat: add ProcessAndSaveSingleProfileAsync with InsertIfNotExists"
```

---

## Chunk 3: Core Logic (ReadProfilesSequentialAsync + Wiring)

### Task 7: ReadProfilesSequentialAsync — Core Method

**Files:**
- Modify: `DLMS_SERVICE/Services/DLMSParallelReadService.cs`

This is the most critical task. We add the new method that reads profiles one-by-one with incremental dates and per-profile timeouts.

- [ ] **Step 1: Add dependencies to DLMSParallelReadService constructor**

Modify: `DLMS_SERVICE/Services/DLMSParallelReadService.cs:37-66`

Add fields:
```csharp
private readonly IProfileReadingConfig _profileConfig;
```

Add to constructor parameters:
```csharp
IProfileReadingConfig profileConfig
```

Add to constructor body:
```csharp
_profileConfig = profileConfig ?? throw new ArgumentNullException(nameof(profileConfig));
```

- [ ] **Step 2: Add ReadSingleProfileAsync private helper**

Add after `ReadProfileDataAsync` (after line 1058):

```csharp
private async Task<ReadResult> ReadSingleProfileAsync(
    IDLMSCommunicationSession session,
    string profileObis,
    DateTime dateStart,
    DateTime dateEnd,
    int timeoutSeconds,
    CancellationToken ct)
{
    try
    {
        session.ReadObjects.Clear();
        session.ReadObjects.AddRange(ParseObjects($"{profileObis}:2"));

        using var profileCts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
        using var combined = CancellationTokenSource.CreateLinkedTokenSource(ct, profileCts.Token);

        var reader = new NonStaticReaderCommunication();
        var readTask = Task.Run(
            () => reader.ReadRowsByRangeAsync(session, dateStart.ToString(), dateEnd.ToString()),
            combined.Token);

        var result = await readTask;

        return new ReadResult
        {
            Success = !string.IsNullOrEmpty(result) && result != "Lecture impossible",
            Data = result,
            ErrorMessage = result == "Lecture impossible" ? $"Echec lecture profil {profileObis}" : ""
        };
    }
    catch (OperationCanceledException) when (!ct.IsCancellationRequested)
    {
        _logger.LogWarning("Timeout ({Timeout}s) lecture profil {Obis}", timeoutSeconds, profileObis);
        return new ReadResult { Success = false, ErrorMessage = $"Timeout {timeoutSeconds}s" };
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erreur lecture profil {Obis}", profileObis);
        return new ReadResult { Success = false, ErrorMessage = ex.Message };
    }
}
```

- [ ] **Step 3: Add ReadProfilesSequentialAsync method**

Add after the helper from step 2:

```csharp
private async Task<List<ProfileReadResult>> ReadProfilesSequentialAsync(
    IDLMSCommunicationSession session,
    string serial,
    CancellationToken ct)
{
    var results = new List<ProfileReadResult>();
    var profiles = await _profileConfig.GetOrderedProfilesAsync();
    var now = DateTime.Now;

    // Load all history for this meter in one query
    List<MeterProfileReadHistory> histories;
    using (var scope = _serviceProvider.CreateScope())
    {
        var queryRepo = scope.ServiceProvider.GetRequiredService<IMeterProfileReadHistoryQueryRepository>();
        histories = await queryRepo.GetAllForMeterAsync(serial);
    }

    foreach (var profile in profiles)
    {
        if (ct.IsCancellationRequested) break;

        var profileResult = new ProfileReadResult { ProfileObis = profile.ProfileObis };
        var sw = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            // Calculate incremental date range
            var history = histories.FirstOrDefault(h => h.ProfileObis == profile.ProfileObis);
            var fallbackLimit = now.AddHours(-profile.FallbackMaxHours);

            DateTime dateStart;
            if (history?.LastReadUpTo != null)
            {
                // Clock drift guard: if LastReadUpTo is in the future, reset
                if (history.LastReadUpTo > now.AddMinutes(15))
                {
                    _logger.LogWarning("Clock drift detected for {Serial}/{Obis}: LastReadUpTo={Last} > now+15min, resetting",
                        serial, profile.ProfileObis, history.LastReadUpTo);
                    dateStart = fallbackLimit;
                }
                else
                {
                    dateStart = history.LastReadUpTo > fallbackLimit ? history.LastReadUpTo : fallbackLimit;
                }
            }
            else
            {
                dateStart = fallbackLimit;
            }

            // Round dateEnd based on profile interval
            DateTime dateEnd;
            if (profile.ProfileObis == "1.0.99.2.0.255") // 5-min profile
                dateEnd = new DateTime(now.Year, now.Month, now.Day, now.Hour, (now.Minute / 5) * 5, 0);
            else if (profile.ProfileObis == "1.0.99.1.0.255") // 1-hour profile
                dateEnd = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            else if (profile.ProfileObis == "1.0.99.3.0.255") // 24-hour profile
                dateEnd = now.Date;
            else
                dateEnd = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);

            // Skip if nothing new to read
            if (dateStart >= dateEnd)
            {
                _logger.LogDebug("Skip profil {Obis} pour {Serial}: dateStart >= dateEnd ({Start} >= {End})",
                    profile.ProfileObis, serial, dateStart, dateEnd);
                profileResult.Success = true;
                profileResult.RowsRead = 0;
                results.Add(profileResult);
                continue;
            }

            // Read this single profile
            var readResult = await ReadSingleProfileAsync(session, profile.ProfileObis, dateStart, dateEnd, profile.TimeoutSeconds, ct);

            if (readResult.Success)
            {
                // Save to DB
                var rowsInserted = await _hardwareService.ProcessAndSaveSingleProfileAsync(
                    readResult.Data, serial, profile.ProfileObis);

                sw.Stop();
                profileResult.Success = true;
                profileResult.RowsRead = rowsInserted;
                profileResult.DurationMs = sw.ElapsedMilliseconds;

                // Update LastReadUpTo — only after successful persistence
                using var scope = _serviceProvider.CreateScope();
                var cmdRepo = scope.ServiceProvider.GetRequiredService<IMeterProfileReadHistoryCommandRepository>();
                await cmdRepo.UpsertAsync(serial, profile.ProfileObis, dateEnd, rowsInserted, sw.ElapsedMilliseconds);

                _logger.LogDebug("Profil {Obis} lu pour {Serial}: {Rows} lignes en {Ms}ms ({Start} -> {End})",
                    profile.ProfileObis, serial, rowsInserted, sw.ElapsedMilliseconds, dateStart, dateEnd);
            }
            else
            {
                sw.Stop();
                profileResult.Error = readResult.ErrorMessage;
                profileResult.DurationMs = sw.ElapsedMilliseconds;

                _logger.LogWarning("Echec profil {Obis} pour {Serial}: {Error}",
                    profile.ProfileObis, serial, readResult.ErrorMessage);

                // If timeout caused HDLC corruption, try to recover
                if (readResult.ErrorMessage.Contains("Timeout"))
                {
                    try
                    {
                        session.Reader?.Disconnect();
                        session.Reader?.InitializeConnection();
                        _logger.LogDebug("HDLC reconnecte apres timeout profil {Obis}", profile.ProfileObis);
                    }
                    catch (Exception reconnEx)
                    {
                        _logger.LogWarning(reconnEx, "HDLC reconnexion echouee apres timeout profil {Obis}, arret lectures profils", profile.ProfileObis);
                        results.Add(profileResult);
                        break; // Stop reading remaining profiles for this meter
                    }
                }
            }
        }
        catch (Exception ex)
        {
            sw.Stop();
            profileResult.Error = ex.Message;
            profileResult.DurationMs = sw.ElapsedMilliseconds;
            _logger.LogError(ex, "Exception profil {Obis} pour {Serial}", profile.ProfileObis, serial);
        }

        results.Add(profileResult);
    }

    return results;
}
```

- [ ] **Step 4: Add required usings at top of file**

```csharp
using DLMS_DAL.ReadingDomainDal.Repositories.Queries;
using DLMS_DAL.ReadingDomainDal.Repositories.Commands;
using DLMS_MODELS.ReadingDomain.Entities;
```

- [ ] **Step 5: Verify build**

Run: `dotnet build DLMS_SERVICE/DLMS_SERVICE.csproj`
Expected: Build succeeded

- [ ] **Step 6: Commit**

```bash
git add DLMS_SERVICE/Services/DLMSParallelReadService.cs
git commit -m "feat: add ReadProfilesSequentialAsync with incremental reads and per-profile timeouts"
```

---

### Task 8: Wire Up ReadSingleMeterOnSessionAsync

**Files:**
- Modify: `DLMS_SERVICE/Services/DLMSParallelReadService.cs:764-776`

- [ ] **Step 1: Replace batch profile read with sequential reads**

Replace lines 764-776 (the section `// 6. Read profiles` through `await _hardwareService.ProcessAndSaveProfileDataAsync`):

**Old code (lines 764-776):**
```csharp
// 6. Read profiles
var now = DateTime.Now;
session.ReadObjects.Clear();
session.ReadObjects.AddRange(ParseObjects(
    "1.0.99.1.0.255:2;1.0.99.2.0.255:2;1.0.99.3.0.255:2;0.0.98.1.0.255:2;0.0.99.98.0.255:2;0.0.99.98.1.255:2;0.0.99.98.2.255:2;0.0.99.98.3.255:2;0.0.99.98.4.255:2;0.0.99.98.5.255:2;0.0.99.98.6.255:2;0.0.99.98.7.255:2"));

var profileResult = await ReadProfileDataAsync(session, now.Date,
    new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0), combinedCts.Token);

if (profileResult.Success)
{
    await _hardwareService.ProcessAndSaveProfileDataAsync(profileResult.Data, serial);
}
```

**New code:**
```csharp
// 6. Read profiles — sequential, prioritized, incremental
var profileResults = await ReadProfilesSequentialAsync(session, serial, combinedCts.Token);
outcome.ProfileResults = profileResults;
```

- [ ] **Step 2: Update success criteria**

The original code unconditionally sets `Success = true` after instant data + profiles. Keep this behavior: if instant data was read successfully, the meter is marked as read. Profile results are tracked separately in `ProfileResults` for monitoring, but do NOT gate overall success.

Lines 778-780 remain unchanged:
```csharp
// Success — instant data was read, profiles are tracked separately in ProfileResults
outcome.Success = true;
outcome.ResultCategory = DLMS_MODELS.ReadingDomain.Enums.MeterReadingResult.Lu;
```

This ensures the multi-pass orchestrator does not retry meters where instant data + some profiles succeeded.

- [ ] **Step 3: Verify build**

Run: `dotnet build DLMS_SERVICE/DLMS_SERVICE.csproj`
Expected: Build succeeded

- [ ] **Step 4: Commit**

```bash
git add DLMS_SERVICE/Services/DLMSParallelReadService.cs
git commit -m "feat: wire sequential profile reading into ReadSingleMeterOnSessionAsync"
```

---

### Task 9: Wire Up ReadMeterWithExistingSessionAsync

**Files:**
- Modify: `DLMS_SERVICE/Services/DLMSParallelReadService.cs:956-960`

- [ ] **Step 1: Replace batch profile read in second code path**

`ReadMeterWithExistingSessionAsync` is a void method with no `MeterReadOutcome` return. Replace the profile reading block and its success handler.

**Old code (lines 956-960 + the success handler that follows):**
```csharp
session.ReadObjects.Clear();
session.ReadObjects.AddRange(ParseObjects(
    "1.0.99.1.0.255:2;1.0.99.2.0.255:2;1.0.99.3.0.255:2;0.0.98.1.0.255:2;0.0.99.98.0.255:2;0.0.99.98.1.255:2;0.0.99.98.2.255:2;0.0.99.98.3.255:2;0.0.99.98.4.255:2;0.0.99.98.5.255:2;0.0.99.98.6.255:2;0.0.99.98.7.255:2"));

var profileResult = await ReadProfileDataAsync(session, dateStart, dateEnd, combinedCts.Token);

if (profileResult.Success)
{
    await _hardwareService.ProcessAndSaveProfileDataAsync(profileResult.Data, serial);
}
```

**New code:**
```csharp
// Profile reading — sequential, prioritized, incremental (same as ReadSingleMeterOnSessionAsync)
var profileResults = await ReadProfilesSequentialAsync(session, serial, combinedCts.Token);
var profilesRead = profileResults.Count(p => p.Success);
_logger.LogDebug("{Serial}: {Count}/{Total} profils lus via ReadMeterWithExistingSession",
    serial, profilesRead, profileResults.Count);
```

No `outcome.ProfileResults` assignment since this method has no outcome object. The profile results are logged and `MeterProfileReadHistory` is updated inside `ReadProfilesSequentialAsync`.

- [ ] **Step 2: Verify build**

Run: `dotnet build DLMS_SERVICE/DLMS_SERVICE.csproj`
Expected: Build succeeded

- [ ] **Step 3: Commit**

```bash
git add DLMS_SERVICE/Services/DLMSParallelReadService.cs
git commit -m "feat: wire sequential profile reading into ReadMeterWithExistingSessionAsync"
```

---

## Chunk 4: Verification & Cleanup

### Task 10: Full Build + Integration Verification

**Files:**
- No new files

- [ ] **Step 1: Full solution build**

Run: `dotnet build`
Expected: Build succeeded with 0 errors

- [ ] **Step 2: Verify DI wiring**

Check that the service starts without DI errors by reviewing that all new interfaces are registered:
- `IMeterProfileReadHistoryQueryRepository` in DependencyInjection.cs
- `IMeterProfileReadHistoryCommandRepository` in DependencyInjection.cs
- `IProfileReadingConfig` in service registration
- `IProfileReadingConfig` injected into `DLMSParallelReadService` constructor

- [ ] **Step 3: Review all changes**

Run: `git diff master --stat`
Verify all expected files are modified/created.

- [ ] **Step 4: Final commit if any fixes needed**

```bash
git add -A
git commit -m "fix: resolve build/DI issues from profile optimization"
```

---

### Task 11: Run Migration on Test DB

**Files:**
- `scripts/migrate-profile-optimization.sql`

- [ ] **Step 1: Execute migration**

Run the migration script on the test database `db_ac2526_dlmsdb_v1_Test`. This must be done before the first run.

**Verify:**
- Table `MeterProfileReadHistory` exists
- Index `UQ_ProfileDetail_NoDup` exists on `Gxdlmsprofilgenericdetails`
- `ReadingConfigurations` table has ProfileReading entries

- [ ] **Step 2: Verify with queries**

```sql
SELECT COUNT(*) FROM MeterProfileReadHistory; -- should be 0
SELECT name FROM sys.indexes WHERE object_id = OBJECT_ID('Gxdlmsprofilgenericdetails') AND name LIKE '%NoDup%';
SELECT * FROM ReadingConfigurations WHERE GroupeConfig = 'ProfileReading';
```

---

### Task 12: Integration Test

- [ ] **Step 1: Run service with a small set of meters**

Start the DLMS_SERVICE with the multi-pass orchestrator and observe the logs. Expected log patterns:

```
[INF] Profil 1.0.99.3.0.255 lu pour {Serial}: {N} lignes en {Ms}ms
[INF] Profil 1.0.99.1.0.255 lu pour {Serial}: {N} lignes en {Ms}ms
[INF] Profil 1.0.99.2.0.255 lu pour {Serial}: {N} lignes en {Ms}ms
```

Or on timeout:
```
[WRN] Timeout (120s) lecture profil 1.0.99.2.0.255
[WRN] Echec profil 1.0.99.2.0.255 pour {Serial}: Timeout 120s
```

- [ ] **Step 2: Verify incremental behavior**

After the first cycle session completes, check:
```sql
SELECT * FROM MeterProfileReadHistory ORDER BY LastReadAt DESC;
```

Each meter should have up to 12 rows (one per profile) with `LastReadUpTo` set.

On the next session, logs should show smaller date ranges and fewer rows read.

- [ ] **Step 3: Verify data in Gxdlmsprofilgenericdetails**

```sql
-- Check that new data was inserted without duplicates
SELECT NumeroCompteur, GxdlmsprofilgenericId, COUNT(*) as cnt
FROM Gxdlmsprofilgenericdetails
WHERE DateEnr >= DATEADD(HOUR, -1, GETDATE())
GROUP BY NumeroCompteur, GxdlmsprofilgenericId;
```
