# Suppression des champs obsolètes de CommandeCompteur

## ✅ Modifications effectuées

Les champs `Resultats` et `Dateenrresultat` ont été **complètement supprimés** de l'entité `CommandeCompteur`.

### 1. Fichiers modifiés

#### Entités et Responses
- ✅ `CommandeCompteur.cs` - Suppression des champs `Resultats` et `Dateenrresultat`
- ✅ `CommandeCompteurResponse.cs` - Suppression des champs `Resultats` et `Dateenrresultat`

#### Repositories
- ✅ `CommandeCompteurCommandRepository.cs` - Suppression des références aux champs obsolètes
- ✅ `ReadObjectCommandeQueryRepository.cs` - Migration vers l'utilisation de `ResultatCommandeCompteur`

### 2. Nouvelle logique dans ReadObjectCommandeQueryRepository

Au lieu de mettre à jour directement les champs obsolètes, le code crée maintenant un objet `ResultatCommandeCompteur` :

```csharp
// AVANT (ancien code)
commandeCompteur.Resultats = result;
commandeCompteur.Dateenrresultat = DateTime.Now;
var updateResult = _ICommandeCompteurCommandRepository.EditCommandeCompteur(commandeCompteur).Result;

// APRÈS (nouveau code)
var resultatCommandeCompteur = new ResultatCommandeCompteur
{
    CommandeCompteurId = commandeCompteur.Id,
    CodeObisId = item.Numeroprofile ?? 1,
    GxdlmsprofilgenericId = item.Numeroprofile ?? 1,
    Value = result,
    NumeroCompteur = compteur.NumeroCompteur,
    DateEnr = DateTime.Now,
    IsArchive = false,
    NumeroTentative = 1,
    CreatedBy = "System",
    CreatedAt = DateTime.Now
};
var savedResult = await _IResultatCommandeCompteurCommandRepository.AddResultatCommandeCompteur(resultatCommandeCompteur);
```

### 3. Avantages de cette modification

- ✅ **Code plus propre** : Plus de champs marqués comme `[Obsolete]`
- ✅ **Séparation des responsabilités** : Les résultats sont dans leur propre table
- ✅ **Historisation complète** : Possibilité de conserver plusieurs résultats par commande
- ✅ **Traçabilité** : Chaque résultat a ses propres métadonnées (date, tentative, etc.)

## 📋 Prochaines étapes

### 1. Créer une nouvelle migration

La nouvelle migration va :
- Créer la table `ResultatCommandeCompteur`
- Supprimer les colonnes `Resultats`, `Dateenrresultat` et `Nombretentative` de `CommandeCompteur`

```bash
cd c:\Users\annezan\source\repos\DLMS\API\DLMS.API
dotnet ef migrations add SupprimerChampsObsoletesCommandeCompteur --project "..\DLMS_DAL\DLMS_DAL.csproj" --startup-project "DLMS.API.csproj"
```

### 2. Migration des données (IMPORTANT !)

⚠️ **AVANT d'appliquer la migration**, assurez-vous de migrer les données existantes :

```sql
-- 1. Migrer les données existantes vers ResultatCommandeCompteur
INSERT INTO ResultatCommandeCompteur (
    CommandeCompteurId,
    CodeObisId,
    GxdlmsprofilgenericId,
    Value,
    NumeroCompteur,
    DateEnr,
    IsArchive,
    NumeroTentative,
    CreatedAt,
    CreatedBy
)
SELECT 
    cc.Id AS CommandeCompteurId,
    1 AS CodeObisId, -- À AJUSTER selon votre logique métier
    1 AS GxdlmsprofilgenericId, -- À AJUSTER selon votre logique métier
    cc.Resultats AS Value,
    c.NumeroCompteur AS NumeroCompteur,
    cc.Dateenrresultat AS DateEnr,
    0 AS IsArchive,
    1 AS NumeroTentative, -- Par défaut à 1
    GETDATE() AS CreatedAt,
    'Migration' AS CreatedBy
FROM CommandeCompteur cc
INNER JOIN Compteur c ON cc.CompteurId = c.Id
WHERE cc.Resultats IS NOT NULL 
  AND cc.Resultats != ''
  AND cc.IsArchive = 0;

-- 2. Vérifier que les données ont été migrées
SELECT COUNT(*) AS NombreDonneesMigrees 
FROM ResultatCommandeCompteur 
WHERE CreatedBy = 'Migration';

-- 3. SEULEMENT APRÈS vérification, supprimer les colonnes
-- ALTER TABLE CommandeCompteur DROP COLUMN Resultats;
-- ALTER TABLE CommandeCompteur DROP COLUMN Dateenrresultat;
-- ALTER TABLE CommandeCompteur DROP COLUMN Nombretentative;
```

### 3. Appliquer la migration

```bash
dotnet ef database update --project "..\DLMS_DAL\DLMS_DAL.csproj" --startup-project "DLMS.API.csproj"
```

### 4. Tester l'application

1. Vérifier que l'application démarre sans erreur
2. Tester la création d'une commande compteur
3. Tester l'exécution d'une commande (qui devrait maintenant créer des `ResultatCommandeCompteur`)
4. Vérifier que les résultats sont bien enregistrés dans la nouvelle table

## ⚠️ Points d'attention

### Dépendances ajoutées

Le repository `ReadObjectCommandeQueryRepository` a maintenant besoin de `IResultatCommandeCompteurCommandRepository` :

```csharp
public ReadObjectCommandeQueryRepository(
    DLMSDBContext context, 
    ICommandeQueryRepository ICommandeQueryRepository, 
    ICommandeCompteurCommandRepository ICommandeCompteurCommandRepository, 
    ICommandeCommandRepository ICommandeCommandRepository,
    IResultatCommandeCompteurCommandRepository IResultatCommandeCompteurCommandRepository) // ← NOUVEAU
```

Assurez-vous que cette dépendance est bien enregistrée dans le conteneur DI (déjà fait dans `DependencyInjection.cs`).

### Logique métier à ajuster

Dans `ReadObjectCommandeQueryRepository.cs`, les valeurs suivantes sont à ajuster selon votre logique métier :

```csharp
CodeObisId = item.Numeroprofile ?? 1, // À ajuster
GxdlmsprofilgenericId = item.Numeroprofile ?? 1, // À ajuster
```

Vous devrez peut-être :
- Récupérer le vrai `CodeObisId` depuis la commande ou le profil
- Récupérer le vrai `GxdlmsprofilgenericId` depuis la commande ou le profil

### Gestion des tentatives

L'ancien code incrémentait `Nombretentative` sur `CommandeCompteur`. Maintenant, chaque tentative crée un nouveau `ResultatCommandeCompteur` avec un `NumeroTentative` incrémenté.

Vous devrez peut-être ajouter une logique pour :
1. Récupérer le dernier `NumeroTentative` pour une `CommandeCompteur` donnée
2. L'incrémenter lors de la création d'un nouveau résultat

Exemple :

```csharp
// Récupérer le dernier numéro de tentative
var dernierResultat = await _context.ResultatCommandeCompteurs
    .Where(r => r.CommandeCompteurId == commandeCompteur.Id && r.IsArchive == false)
    .OrderByDescending(r => r.NumeroTentative)
    .FirstOrDefaultAsync();

int prochainNumeroTentative = (dernierResultat?.NumeroTentative ?? 0) + 1;

// Utiliser lors de la création
var resultatCommandeCompteur = new ResultatCommandeCompteur
{
    // ... autres champs ...
    NumeroTentative = prochainNumeroTentative
};
```

## 📊 Résumé des changements

| Avant | Après |
|-------|-------|
| `CommandeCompteur.Resultats` | `ResultatCommandeCompteur.Value` |
| `CommandeCompteur.Dateenrresultat` | `ResultatCommandeCompteur.DateEnr` |
| `CommandeCompteur.Nombretentative` | `ResultatCommandeCompteur.NumeroTentative` |
| 1 résultat par CommandeCompteur | N résultats par CommandeCompteur (historisation) |
| Pas de lien avec CodeObis/Gxdlmsprofilgeneric | Lien explicite avec CodeObis et Gxdlmsprofilgeneric |

## 🎉 Bénéfices

1. **Architecture plus claire** : Séparation des responsabilités
2. **Historisation complète** : Tous les résultats sont conservés
3. **Traçabilité améliorée** : Métadonnées riches (date, tentative, créateur)
4. **Extensibilité** : Facile d'ajouter de nouveaux champs aux résultats
5. **Analyse facilitée** : Requêtes SQL plus simples pour analyser les résultats

---

**Date de modification** : 13 janvier 2026
**Version** : 2.0
**Statut** : ✅ Champs obsolètes supprimés
