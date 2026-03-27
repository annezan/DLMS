# Mode --profiles enrichi : scalers + TC/TT

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Enrichir le mode `--profiles` du test d'intégration pour loguer les scalers DLMS, les rapports TC/TT, et produire 3 vues par valeur (brut, scalé, avec TC×TT) pour comparaison avec les exports constructeur.

**Architecture:** Après connexion HDLC et chargement des scalers via `GetScalersAndUnits()`, on lit les 4 registres TC/TT (`1.0.0.4.2-6.255`) puis on enrichit chaque profil lu avec les conversions. Le JSON de sortie contient les métadonnées (scalers, TC/TT) et les 3 vues de données.

**Tech Stack:** C# .NET 8, Gurux.DLMS (GXDLMSRegister.Scaler, GXDLMSRegister.Unit), Newtonsoft.Json

---

## File Structure

| Action | File | Responsabilité |
|--------|------|----------------|
| Modify | `DLMS_IntegrationTest/Program.cs` | Enrichir modèles, RunProfilesTest, rapport console |

Toutes les modifications sont dans un seul fichier — le test d'intégration.

---

### Task 1: Enrichir les modèles de données

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs:92-111`

- [ ] **Step 1: Ajouter les usings Gurux nécessaires**

Après la ligne `using DLMS_COMMUNICATION.Reader;` (ligne 13), ajouter :

```csharp
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects;
```

- [ ] **Step 2: Enrichir la classe MeterProfileReport**

Ajouter les champs TC/TT et scalers au modèle `MeterProfileReport` (ligne 103) :

```csharp
class MeterProfileReport
{
    public string Serial { get; set; } = "";
    public string Ip { get; set; } = "";
    public bool ConnectionSuccess { get; set; }
    public long HdlcMs { get; set; }
    public List<ProfileReadResult> Profiles { get; set; } = new();
    public string Error { get; set; } = "";
    // Nouveaux champs
    public Dictionary<string, object?> TcTtValues { get; set; } = new();
    public Dictionary<string, ScalerInfo> Scalers { get; set; } = new();
}
```

- [ ] **Step 3: Ajouter la classe ScalerInfo**

Juste avant `MeterProfileReport`, ajouter :

```csharp
class ScalerInfo
{
    public double Scaler { get; set; }
    public string Unit { get; set; } = "";
    public int UnitCode { get; set; }
}
```

- [ ] **Step 4: Enrichir ProfileReadResult pour stocker les données converties**

Remplacer le champ `RawData` par des champs structurés dans `ProfileReadResult` :

```csharp
class ProfileReadResult
{
    public string ProfileObis { get; set; } = "";
    public string ProfileName { get; set; } = "";
    public bool Success { get; set; }
    public int RowCount { get; set; }
    public long DurationMs { get; set; }
    public string Error { get; set; } = "";
    public string? RawData { get; set; }
    // Nouveaux champs
    public string[]? Columns { get; set; }
    public List<object[]>? RowsRaw { get; set; }
    public List<object[]>? RowsScaled { get; set; }
    public List<object[]>? RowsWithTcTt { get; set; }
}
```

- [ ] **Step 5: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat(profiles): enrichir modèles avec scalers et TC/TT"
```

---

### Task 2: Lire les scalers et TC/TT après connexion HDLC

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — méthode `RunProfilesTest`, après `session.Reader!.InitializeConnection()` (ligne 2047)

- [ ] **Step 1: Ajouter les OBIS codes TC/TT en constante**

Juste après `ProfileDefinitions` (ligne 1967), ajouter :

```csharp
private static readonly string[] TcTtObis = new[]
{
    "1.0.0.4.2.255",  // CT ratio numerator
    "1.0.0.4.3.255",  // CT ratio denominator / VT
    "1.0.0.4.5.255",  // VT ratio numerator
    "1.0.0.4.6.255",  // VT ratio denominator
};
```

- [ ] **Step 2: Après HDLC OK, charger scalers et lire TC/TT**

Après la ligne `Console.WriteLine($"  HDLC OK ({hdlcSw.ElapsedMilliseconds}ms)");` (ligne 2051), ajouter :

```csharp
                // 2b. Charger scalers
                session.Reader.GetScalersAndUnits();
                session.ScalersLoaded = true;

                // Extraire les scalers de tous les registres connus
                var registerObjects = session.Client.Objects.GetObjects(
                    new ObjectType[] { ObjectType.Register, ObjectType.ExtendedRegister, ObjectType.DemandRegister });
                foreach (var obj in registerObjects)
                {
                    double scaler = 1.0;
                    int unitCode = 0;
                    if (obj is GXDLMSRegister reg)
                    {
                        scaler = reg.Scaler;
                        unitCode = (int)reg.Unit;
                    }
                    else if (obj is GXDLMSExtendedRegister extReg)
                    {
                        scaler = extReg.Scaler;
                        unitCode = (int)extReg.Unit;
                    }
                    else if (obj is GXDLMSDemandRegister demReg)
                    {
                        scaler = demReg.Scaler;
                        unitCode = (int)demReg.Unit;
                    }
                    report.Scalers[obj.LogicalName] = new ScalerInfo
                    {
                        Scaler = scaler,
                        Unit = ((Unit)unitCode).ToString(),
                        UnitCode = unitCode
                    };
                }

                Console.WriteLine($"  Scalers charges: {report.Scalers.Count} registres");
                // Afficher quelques scalers clés
                foreach (var s in report.Scalers.Where(s =>
                    s.Key.StartsWith("1.0.1.8") || s.Key.StartsWith("1.0.2.7") ||
                    s.Key.StartsWith("1.0.31.7") || s.Key.StartsWith("1.0.32.7") ||
                    s.Key.StartsWith("1.0.14.7")).Take(8))
                {
                    Console.WriteLine($"    {s.Key,-20} scaler={s.Value.Scaler}  unit={s.Value.Unit}");
                }

                // 2c. Lire TC/TT
                foreach (var obisCode in TcTtObis)
                {
                    try
                    {
                        var obj = session.Client.Objects.FindByLN(ObjectType.None, obisCode);
                        if (obj != null)
                        {
                            var val = session.Reader.Read(obj, 2);
                            report.TcTtValues[obisCode] = val;
                            Console.WriteLine($"    {obisCode} = {val}");
                        }
                        else
                        {
                            report.TcTtValues[obisCode] = null;
                            Console.WriteLine($"    {obisCode} = (non trouve)");
                        }
                    }
                    catch (Exception ex)
                    {
                        report.TcTtValues[obisCode] = $"ERR: {ex.Message}";
                        Console.WriteLine($"    {obisCode} = ERREUR: {ex.Message}");
                    }
                }

                // Calculer le produit TC×TT pour l'affichage
                double tcVal = 1, ttVal = 1;
                if (report.TcTtValues.TryGetValue("1.0.0.4.2.255", out var tcRaw) && tcRaw is IConvertible tcConv)
                    tcVal = tcConv.ToDouble(null);
                if (report.TcTtValues.TryGetValue("1.0.0.4.3.255", out var ttRaw) && ttRaw is IConvertible ttConv)
                    ttVal = ttConv.ToDouble(null);
                Console.WriteLine($"  TC={tcVal}, TT={ttVal}, TC×TT={tcVal * ttVal}");
```

- [ ] **Step 3: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat(profiles): lire scalers et TC/TT après connexion HDLC"
```

---

### Task 3: Appliquer les conversions aux données de profil

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — dans la boucle `foreach (var (obis, name, timeout) in ProfileDefinitions)`, après le bloc `profileResult.Success = true;` (ligne 2107)

- [ ] **Step 1: Ajouter une méthode helper pour déterminer si TC×TT s'applique**

Après la méthode `RunProfilesTest`, ajouter :

```csharp
    /// <summary>
    /// Détermine si le rapport TC×TT s'applique à une grandeur OBIS.
    /// Energy (x.x.*.8.*) et Power (x.x.*.7.*) = Oui (×TC×TT)
    /// Current (x.x.3*.7.*) = ×TC seulement
    /// Voltage (x.x.3*.7.* quand C=32/52/72) = ×TT seulement
    /// Frequency, angle, status, clock = Non
    /// </summary>
    private static (double factor, string label) GetTcTtFactor(string obisCode, double tc, double tt)
    {
        // Parse OBIS: A.B.C.D.E.F
        var parts = obisCode.Split('.');
        if (parts.Length < 6) return (1.0, "×1");
        if (!int.TryParse(parts[2], out int c) || !int.TryParse(parts[3], out int d))
            return (1.0, "×1");

        // Energy registers (D=8): ×TC×TT
        if (d == 8) return (tc * tt, $"×TC×TT({tc * tt})");

        // Power registers (D=7):
        if (d == 7)
        {
            // Current: C=31,51,71 → ×TC
            if (c == 31 || c == 51 || c == 71) return (tc, $"×TC({tc})");
            // Voltage: C=32,52,72 → ×TT
            if (c == 32 || c == 52 || c == 72) return (tt, $"×TT({tt})");
            // Frequency (C=14), phase angle (C=81): ×1
            if (c == 14 || c == 81) return (1.0, "×1");
            // Other power (C=1-4, 21-24, 41-44, 61-64): ×TC×TT
            return (tc * tt, $"×TC×TT({tc * tt})");
        }

        return (1.0, "×1");
    }
```

- [ ] **Step 2: Enrichir le traitement après lecture réussie d'un profil**

Dans `RunProfilesTest`, remplacer le bloc qui commence à `profileResult.Success = true;` (lignes 2107-2121) par :

```csharp
                        else
                        {
                            profileResult.Success = true;
                            profileResult.RawData = result;

                            // Deserialiser les données brutes
                            var entries = Newtonsoft.Json.JsonConvert.DeserializeObject<
                                List<KeyValuePair<object[], object[]>>>(result);
                            var rowCount = entries?.Sum(e => e.Key.Length) ?? 0;
                            profileResult.RowCount = rowCount;

                            // Extraire colonnes et rows
                            if (entries != null && entries.Count > 0)
                            {
                                var entry = entries[0];
                                profileResult.Columns = entry.Value.Select(v => v?.ToString() ?? "").ToArray();

                                profileResult.RowsRaw = new List<object[]>();
                                profileResult.RowsScaled = new List<object[]>();
                                profileResult.RowsWithTcTt = new List<object[]>();

                                // Préparer scalers et TC/TT pour chaque colonne
                                var colScalers = new double[profileResult.Columns.Length];
                                var colTcTtFactors = new double[profileResult.Columns.Length];
                                for (int ci = 0; ci < profileResult.Columns.Length; ci++)
                                {
                                    var colObis = profileResult.Columns[ci];
                                    colScalers[ci] = report.Scalers.TryGetValue(colObis, out var si) ? si.Scaler : 1.0;
                                    colTcTtFactors[ci] = GetTcTtFactor(colObis, tcVal, ttVal).factor;
                                }

                                foreach (var row in entry.Key)
                                {
                                    if (row is not Newtonsoft.Json.Linq.JArray jArr)
                                        continue;
                                    var rawRow = jArr.Select(v => v.Type == Newtonsoft.Json.Linq.JTokenType.Null ? (object)0 : v.ToObject<object>()!).ToArray();
                                    profileResult.RowsRaw.Add(rawRow);

                                    // Scaled row
                                    var scaledRow = new object[rawRow.Length];
                                    var tcttRow = new object[rawRow.Length];
                                    for (int ci = 0; ci < rawRow.Length; ci++)
                                    {
                                        if (ci < colScalers.Length && rawRow[ci] is IConvertible conv)
                                        {
                                            try
                                            {
                                                double raw = conv.ToDouble(null);
                                                double scaled = raw * colScalers[ci];
                                                double withTcTt = scaled * colTcTtFactors[ci];
                                                scaledRow[ci] = scaled;
                                                tcttRow[ci] = withTcTt;
                                            }
                                            catch
                                            {
                                                scaledRow[ci] = rawRow[ci];
                                                tcttRow[ci] = rawRow[ci];
                                            }
                                        }
                                        else
                                        {
                                            scaledRow[ci] = rawRow[ci];
                                            tcttRow[ci] = rawRow[ci];
                                        }
                                    }
                                    profileResult.RowsScaled.Add(scaledRow);
                                    profileResult.RowsWithTcTt.Add(tcttRow);
                                }
                            }

                            Console.WriteLine($"OK — {rowCount} lignes ({sw.ElapsedMilliseconds}ms)");

                            // Sauvegarder JSON enrichi (voir Task 4)
                            var fileName = $"{meter.Serial}_{obis.Replace(".", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                            var filePath = Path.Combine(outputDir, fileName);
                            await File.WriteAllTextAsync(filePath, result);
                        }
```

- [ ] **Step 3: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat(profiles): appliquer conversion scalers + TC×TT aux données"
```

---

### Task 4: Sauvegarder le JSON enrichi par compteur

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — remplacer la sauvegarde fichier brut par un JSON structuré

- [ ] **Step 1: Remplacer la sauvegarde fichier dans le bloc de lecture**

Dans le code de la Task 3, remplacer les lignes de sauvegarde JSON (les 3 dernières lignes du bloc `else`) par :

```csharp
                            // Sauvegarder le JSON brut (rétro-compatibilité)
                            var fileName = $"{meter.Serial}_{obis.Replace(".", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                            var filePath = Path.Combine(outputDir, fileName);
                            await File.WriteAllTextAsync(filePath, result);
```

*(Pas de changement ici — on garde la sauvegarde brute.)

- [ ] **Step 2: Après la boucle des profils, sauvegarder un fichier JSON enrichi par compteur**

Juste avant `// Disconnect` (ligne 2144), ajouter :

```csharp
                // Sauvegarder JSON enrichi par compteur
                var enrichedReport = new
                {
                    serial = meter.Serial,
                    ip = meter.Ip,
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    tc_tt = report.TcTtValues.ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value?.ToString() ?? "null"),
                    scalers = report.Scalers.ToDictionary(
                        kv => kv.Key,
                        kv => new { kv.Value.Scaler, kv.Value.Unit, kv.Value.UnitCode }),
                    profiles = report.Profiles.Where(p => p.Success).Select(p => new
                    {
                        obis = p.ProfileObis,
                        name = p.ProfileName,
                        columns = p.Columns,
                        rows_count = p.RowCount,
                        duration_ms = p.DurationMs,
                        first_rows_comparison = p.RowsRaw?.Take(3).Select((raw, idx) => new
                        {
                            brut = raw,
                            scale = p.RowsScaled?[idx],
                            avec_tc_tt = p.RowsWithTcTt?[idx]
                        })
                    })
                };

                var enrichedJson = Newtonsoft.Json.JsonConvert.SerializeObject(enrichedReport, Newtonsoft.Json.Formatting.Indented);
                var enrichedPath = Path.Combine(outputDir, $"{meter.Serial}_enriched_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                await File.WriteAllTextAsync(enrichedPath, enrichedJson);
                Console.WriteLine($"  JSON enrichi: {enrichedPath}");
```

- [ ] **Step 3: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat(profiles): sauvegarder JSON enrichi avec comparaison brut/scalé/TC_TT"
```

---

### Task 5: Enrichir le rapport console final

**Files:**
- Modify: `DLMS_IntegrationTest/Program.cs` — section rapport final (lignes 2159-2202)

- [ ] **Step 1: Ajouter les infos TC/TT et scalers dans le rapport détaillé**

Remplacer le bloc de détail des profils par compteur (lignes 2193-2202) par :

```csharp
        // Détail des profils par compteur
        foreach (var r in allReports.Where(r => r.ConnectionSuccess))
        {
            Console.WriteLine($"  --- {r.Serial} ---");

            // TC/TT
            if (r.TcTtValues.Count > 0)
            {
                var tcStr = r.TcTtValues.Select(kv => $"{kv.Key}={kv.Value}");
                Console.WriteLine($"    TC/TT: {string.Join(", ", tcStr)}");
            }

            // Scalers clés
            var keyScalers = r.Scalers
                .Where(s => s.Key.StartsWith("1.0.1.8") || s.Key.StartsWith("1.0.2.8") ||
                            s.Key.StartsWith("1.0.3.8") || s.Key.StartsWith("1.0.4.8") ||
                            s.Key.StartsWith("1.0.1.7") || s.Key.StartsWith("1.0.2.7") ||
                            s.Key.StartsWith("1.0.31.7") || s.Key.StartsWith("1.0.32.7") ||
                            s.Key.StartsWith("1.0.14.7"))
                .Take(10);
            foreach (var s in keyScalers)
            {
                Console.WriteLine($"    {s.Key,-20} scaler={s.Value.Scaler,-8} unit={s.Value.Unit}");
            }

            // Profils avec comparaison première ligne
            foreach (var p in r.Profiles)
            {
                if (!p.Success)
                {
                    Console.WriteLine($"    {p.ProfileName,-30} ECHEC: {p.Error}");
                    continue;
                }

                Console.WriteLine($"    {p.ProfileName,-30} {p.RowCount} lignes en {p.DurationMs}ms");

                // Afficher les 2 premières lignes en comparaison
                if (p.Columns != null && p.RowsRaw != null && p.RowsRaw.Count > 0)
                {
                    // En-têtes (colonnes abrégées)
                    var colHeaders = p.Columns.Select(c =>
                    {
                        var parts = c.Split('.');
                        return parts.Length >= 4 ? $"{parts[2]}.{parts[3]}" : c;
                    }).ToArray();

                    Console.WriteLine($"      {"Col:",-12} {string.Join(" | ", colHeaders.Take(6).Select(h => $"{h,14}"))}");

                    for (int ri = 0; ri < Math.Min(2, p.RowsRaw.Count); ri++)
                    {
                        var rawVals = p.RowsRaw[ri].Take(6).Select(v => $"{v,14}");
                        Console.WriteLine($"      {"brut:",-12} {string.Join(" | ", rawVals)}");

                        if (p.RowsScaled != null)
                        {
                            var scaledVals = p.RowsScaled[ri].Take(6).Select(v =>
                                v is double d ? $"{d,14:F1}" : $"{v,14}");
                            Console.WriteLine($"      {"scalé:",-12} {string.Join(" | ", scaledVals)}");
                        }

                        if (p.RowsWithTcTt != null)
                        {
                            var tcttVals = p.RowsWithTcTt[ri].Take(6).Select(v =>
                                v is double d ? $"{d,14:F1}" : $"{v,14}");
                            Console.WriteLine($"      {"TC×TT:",-12} {string.Join(" | ", tcttVals)}");
                        }

                        Console.WriteLine();
                    }
                }
            }
        }
```

- [ ] **Step 2: Commit**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat(profiles): rapport console enrichi avec comparaison brut/scalé/TC_TT"
```

---

### Task 6: Vérification compilation et test local

**Files:**
- Test: `DLMS_IntegrationTest/Program.cs`

- [ ] **Step 1: Vérifier la compilation**

```bash
cd /Users/arnaudatse/Documents/dev/dlmsAsc/DLMS
dotnet build DLMS_IntegrationTest/
```

Expected: `Build succeeded.`

- [ ] **Step 2: Corriger les erreurs éventuelles et recompiler**

Si erreurs, les corriger et relancer `dotnet build`.

- [ ] **Step 3: Commit final**

```bash
git add DLMS_IntegrationTest/Program.cs
git commit -m "feat(profiles): mode --profiles enrichi avec scalers et TC/TT — prêt pour test terrain"
```
