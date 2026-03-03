# Exemples d'utilisation - API ResultatCommandeCompteur

## 📌 Scénarios d'utilisation

### Scénario 1 : Enregistrer un résultat de commande

Lorsqu'une commande est exécutée sur un compteur, vous pouvez enregistrer le résultat :

```http
POST /api/ResultatCommandeCompteur/add
Content-Type: application/json
Authorization: Bearer {token}

{
  "commandeCompteurId": 123,
  "codeObisId": 5,
  "gxdlmsprofilgenericId": 1,
  "value": "230.5",
  "numeroCompteur": "MTR-2024-001",
  "dateEnr": "2026-01-13T14:30:00",
  "isArchive": false,
  "numeroTentative": 1
}
```

**Réponse :**
```json
{
  "isSuccess": true,
  "message": "Résultat ajouté avec succès",
  "data": {
    "id": 456,
    "commandeCompteurId": 123,
    "codeObisId": 5,
    "gxdlmsprofilgenericId": 1,
    "value": "230.5",
    "numeroCompteur": "MTR-2024-001",
    "dateEnr": "2026-01-13T14:30:00",
    "isArchive": false,
    "numeroTentative": 1,
    "codeObis": {
      "id": 5,
      "nom": "Tension Phase 1"
    }
  }
}
```

### Scénario 2 : Enregistrer plusieurs tentatives

Si une commande nécessite plusieurs tentatives, vous pouvez les enregistrer séparément :

**Tentative 1 (échouée) :**
```json
{
  "commandeCompteurId": 123,
  "codeObisId": 5,
  "gxdlmsprofilgenericId": 1,
  "value": "ERREUR_TIMEOUT",
  "numeroCompteur": "MTR-2024-001",
  "dateEnr": "2026-01-13T14:30:00",
  "isArchive": false,
  "numeroTentative": 1
}
```

**Tentative 2 (réussie) :**
```json
{
  "commandeCompteurId": 123,
  "codeObisId": 5,
  "value": "230.5",
  "numeroCompteur": "MTR-2024-001",
  "dateEnr": "2026-01-13T14:35:00",
  "isArchive": false,
  "numeroTentative": 2
}
```

### Scénario 3 : Consulter l'historique des résultats

Pour voir tous les résultats d'une commande compteur :

```http
GET /api/ResultatCommandeCompteur/getResultatsByCommandeCompteurId?commandeCompteurId=123
Authorization: Bearer {token}
```

**Réponse :**
```json
{
  "isSuccess": true,
  "data": [
    {
      "id": 457,
      "value": "230.5",
      "dateEnr": "2026-01-13T14:35:00",
      "numeroTentative": 2,
      "codeObis": { "nom": "Tension Phase 1" }
    },
    {
      "id": 456,
      "value": "ERREUR_TIMEOUT",
      "dateEnr": "2026-01-13T14:30:00",
      "numeroTentative": 1,
      "codeObis": { "nom": "Tension Phase 1" }
    }
  ]
}
```

### Scénario 4 : Modifier un résultat

Si vous devez corriger un résultat :

```http
PUT /api/ResultatCommandeCompteur/edit
Content-Type: application/json
Authorization: Bearer {token}

{
  "id": 456,
  "commandeCompteurId": 123,
  "codeObisId": 5,
  "gxdlmsprofilgenericId": 1,
  "value": "230.8",
  "numeroCompteur": "MTR-2024-001",
  "dateEnr": "2026-01-13T14:35:00",
  "isArchive": false,
  "numeroTentative": 2
}
```

### Scénario 5 : Archiver un résultat

Pour supprimer (soft delete) un résultat :

```http
DELETE /api/ResultatCommandeCompteur/delete
Content-Type: application/json
Authorization: Bearer {token}

{
  "id": 456
}
```

## 🔄 Intégration avec le code existant

### Avant (ancien système)

```csharp
// Ancien code qui mettait à jour directement CommandeCompteur
var commandeCompteur = await _context.CommandeCompteur.FindAsync(id);
commandeCompteur.Resultats = "230.5";
commandeCompteur.Dateenrresultat = DateTime.Now;
await _context.SaveChangesAsync();
```

### Après (nouveau système)

```csharp
// Nouveau code qui crée un ResultatCommandeCompteur
var resultat = new ResultatCommandeCompteur
{
    CommandeCompteurId = id,
    CodeObisId = codeObisId,
    GxdlmsprofilgenericId = gxdlmsprofilgenericId,
    Value = "230.5",
    NumeroCompteur = compteur.NumeroCompteur,
    DateEnr = DateTime.Now,
    IsArchive = false,
    NumeroTentative = tentative
};

await _resultatRepository.AddResultatCommandeCompteur(resultat);
```

## 📊 Cas d'usage métier

### 1. Lecture de compteur avec retry

```csharp
public async Task<bool> LireCompteurAvecRetry(int commandeCompteurId, int maxTentatives)
{
    for (int tentative = 1; tentative <= maxTentatives; tentative++)
    {
        try
        {
            var valeur = await LireCompteur(commandeCompteurId);
            
            // Enregistrer le résultat réussi
            var resultat = new ResultatCommandeCompteurAddCommand
            {
                CommandeCompteurId = commandeCompteurId,
                CodeObisId = GetCodeObisId(),
                GxdlmsprofilgenericId = GetGxdlmsprofilgenericId(),
                Value = valeur.ToString(),
                NumeroCompteur = GetNumeroCompteur(commandeCompteurId),
                DateEnr = DateTime.Now,
                IsArchive = false,
                NumeroTentative = tentative
            };
            
            await _mediator.Send(resultat);
            return true;
        }
        catch (Exception ex)
        {
            // Enregistrer l'échec
            var resultatEchec = new ResultatCommandeCompteurAddCommand
            {
                CommandeCompteurId = commandeCompteurId,
                CodeObisId = GetCodeObisId(),
                GxdlmsprofilgenericId = GetGxdlmsprofilgenericId(),
                Value = $"ERREUR: {ex.Message}",
                NumeroCompteur = GetNumeroCompteur(commandeCompteurId),
                DateEnr = DateTime.Now,
                IsArchive = false,
                NumeroTentative = tentative
            };
            
            await _mediator.Send(resultatEchec);
            
            if (tentative < maxTentatives)
                await Task.Delay(TimeSpan.FromSeconds(5)); // Attendre avant retry
        }
    }
    
    return false;
}
```

### 2. Affichage de l'historique dans l'interface

```typescript
// Exemple frontend TypeScript/Angular
async afficherHistoriqueResultats(commandeCompteurId: number) {
  const response = await this.http.get(
    `/api/ResultatCommandeCompteur/getResultatsByCommandeCompteurId?commandeCompteurId=${commandeCompteurId}`
  ).toPromise();
  
  if (response.isSuccess) {
    this.resultats = response.data.map(r => ({
      tentative: r.numeroTentative,
      valeur: r.value,
      date: new Date(r.dateEnr),
      statut: r.value.includes('ERREUR') ? 'Échec' : 'Succès',
      codeObis: r.codeObis?.nom
    }));
  }
}
```

### 3. Rapport de statistiques

```csharp
public async Task<StatistiquesResultats> GetStatistiquesResultats(int commandeCompteurId)
{
    var query = new GetResultatCommandeCompteursByCommandeCompteurIdQuery(commandeCompteurId);
    var response = await _mediator.Send(query);
    
    if (!response.IsSuccess || !response.Data.Any())
        return null;
    
    var resultats = response.Data;
    
    return new StatistiquesResultats
    {
        NombreTotalTentatives = resultats.Count,
        NombreTentativesReussies = resultats.Count(r => !r.Value.Contains("ERREUR")),
        TauxReussite = (double)resultats.Count(r => !r.Value.Contains("ERREUR")) / resultats.Count * 100,
        DernierResultat = resultats.OrderByDescending(r => r.DateEnr).First(),
        TempsMoyenEntreRessais = CalculerTempsMoyen(resultats)
    };
}
```

## 🎯 Bonnes pratiques

### 1. Toujours spécifier le CodeObis

Chaque résultat doit être typé avec un CodeObis approprié pour identifier ce qui a été mesuré.

### 2. Enregistrer aussi les échecs

N'enregistrez pas seulement les résultats réussis, mais aussi les tentatives échouées pour l'analyse.

### 3. Utiliser NumeroTentative

Incrémentez le numéro de tentative pour suivre les retries.

### 4. Ne pas supprimer définitivement

Utilisez toujours le soft delete (IsArchive) pour conserver l'historique.

### 5. Utiliser les filtres

Profitez du fait que les résultats sont dans une table séparée pour faire des requêtes optimisées :

```csharp
// Exemple : récupérer seulement les résultats réussis des 7 derniers jours
var resultatsRecents = await _context.ResultatCommandeCompteurs
    .Where(r => r.CommandeCompteurId == id 
             && r.DateEnr >= DateTime.Now.AddDays(-7)
             && !r.Value.Contains("ERREUR")
             && r.IsArchive == false)
    .OrderByDescending(r => r.DateEnr)
    .ToListAsync();
```

## 📈 Analyse et reporting

Avec la nouvelle structure, vous pouvez facilement :

- Calculer le taux de succès par compteur
- Identifier les compteurs problématiques (nombreuses tentatives)
- Analyser les types d'erreurs les plus fréquents
- Suivre l'évolution des valeurs dans le temps
- Générer des rapports d'audit

## 🔍 Débogage

Pour déboguer, vous pouvez facilement voir tous les résultats :

```sql
-- Voir tous les résultats d'une commande compteur avec détails
SELECT 
    rcc.Id,
    rcc.NumeroTentative,
    rcc.Value,
    rcc.DateEnr,
    co.Nom AS CodeObis,
    rcc.CreatedAt,
    rcc.CreatedBy
FROM ResultatCommandeCompteur rcc
INNER JOIN CodeObis co ON rcc.CodeObisId = co.Id
WHERE rcc.CommandeCompteurId = 123
  AND rcc.IsArchive = 0
ORDER BY rcc.DateEnr DESC;
```

## 💡 Astuce

Pour faciliter la transition, vous pouvez créer une méthode helper qui convertit automatiquement les anciens appels :

```csharp
public async Task<ResultatCommandeCompteurResponse> EnregistrerResultatLegacy(
    int commandeCompteurId, 
    string resultat, 
    DateTime? dateResultat = null)
{
    var commandeCompteur = await _context.CommandeCompteur
        .Include(cc => cc.Compteur)
        .FirstOrDefaultAsync(cc => cc.Id == commandeCompteurId);
    
    var command = new ResultatCommandeCompteurAddCommand
    {
        CommandeCompteurId = commandeCompteurId,
        CodeObisId = DeterminerCodeObisId(resultat), // À implémenter selon votre logique
        GxdlmsprofilgenericId = DeterminerGxdlmsprofilgenericId(resultat), // À implémenter selon votre logique
        Value = resultat,
        NumeroCompteur = commandeCompteur.Compteur.NumeroCompteur,
        DateEnr = dateResultat ?? DateTime.Now,
        IsArchive = false,
        NumeroTentative = await GetProchainNumeroTentative(commandeCompteurId)
    };
    
    var response = await _mediator.Send(command);
    return response.Data;
}
```
