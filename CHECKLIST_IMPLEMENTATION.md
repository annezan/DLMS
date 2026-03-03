# ✅ Checklist d'implémentation - ResultatCommandeCompteur

## 📋 Vérifications avant de commencer

- [ ] **Backup de la base de données** effectué
- [ ] **Branch Git créée** pour cette fonctionnalité
- [ ] **Tous les fichiers modifiés** sont bien sauvegardés

## 🔨 Étape 1 : Compilation et vérification

### 1.1 Compiler le projet

```bash
cd c:\Users\annezan\source\repos\DLMS\API
dotnet build
```

- [ ] Le projet compile sans erreur
- [ ] Aucun warning critique

### 1.2 Vérifier les références

- [ ] Tous les namespaces sont correctement importés
- [ ] Aucune référence circulaire
- [ ] Les dépendances NuGet sont à jour

## 🗄️ Étape 2 : Migration de la base de données

### 2.1 Créer la migration

```bash
cd c:\Users\annezan\source\repos\DLMS\API\DLMS.API
dotnet ef migrations add AjouterTableResultatCommandeCompteur --project "..\DLMS_DAL\DLMS_DAL.csproj" --startup-project "DLMS.API.csproj"
```

- [ ] Migration créée sans erreur
- [ ] Fichier de migration généré dans le dossier Migrations

### 2.2 Vérifier le script de migration

Ouvrez le fichier de migration généré et vérifiez :

- [ ] La table `ResultatCommandeCompteur` est créée
- [ ] Tous les champs sont présents (Id, CommandeCompteurId, CodeObisId, Value, etc.)
- [ ] Les clés étrangères sont correctement définies
- [ ] Les index sont créés

### 2.3 Appliquer la migration

```bash
dotnet ef database update --project "..\DLMS_DAL\DLMS_DAL.csproj" --startup-project "DLMS.API.csproj"
```

- [ ] Migration appliquée sans erreur
- [ ] Table `ResultatCommandeCompteur` créée dans la base de données

### 2.4 Vérifier la table dans la base de données

Exécutez cette requête SQL :

```sql
SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'ResultatCommandeCompteur'
ORDER BY ORDINAL_POSITION;
```

- [ ] Tous les champs sont présents
- [ ] Les types de données sont corrects
- [ ] Les clés étrangères sont en place

## 🔐 Étape 3 : Configuration des permissions

### 3.1 Ajouter les permissions dans la base de données

```sql
-- Vérifier si les permissions existent déjà
SELECT * FROM Permissions 
WHERE Name IN (
    'VIEW_RESULTAT_COMMANDE_COMPTEUR',
    'ADD_RESULTAT_COMMANDE_COMPTEUR',
    'EDIT_RESULTAT_COMMANDE_COMPTEUR',
    'DELETE_RESULTAT_COMMANDE_COMPTEUR'
);

-- Si elles n'existent pas, les ajouter
INSERT INTO Permissions (Name, Description, CreatedAt, IsArchive)
VALUES 
    ('VIEW_RESULTAT_COMMANDE_COMPTEUR', 'Voir les résultats des commandes compteur', GETDATE(), 0),
    ('ADD_RESULTAT_COMMANDE_COMPTEUR', 'Ajouter des résultats de commande compteur', GETDATE(), 0),
    ('EDIT_RESULTAT_COMMANDE_COMPTEUR', 'Modifier des résultats de commande compteur', GETDATE(), 0),
    ('DELETE_RESULTAT_COMMANDE_COMPTEUR', 'Supprimer des résultats de commande compteur', GETDATE(), 0);
```

- [ ] 4 permissions créées
- [ ] Permissions visibles dans la table Permissions

### 3.2 Attribuer les permissions aux rôles

```sql
-- Exemple : attribuer toutes les permissions au rôle Admin (RoleId = 1)
DECLARE @AdminRoleId INT = 1; -- À ajuster selon votre configuration

INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT @AdminRoleId, Id
FROM Permissions
WHERE Name IN (
    'VIEW_RESULTAT_COMMANDE_COMPTEUR',
    'ADD_RESULTAT_COMMANDE_COMPTEUR',
    'EDIT_RESULTAT_COMMANDE_COMPTEUR',
    'DELETE_RESULTAT_COMMANDE_COMPTEUR'
);
```

- [ ] Permissions attribuées au(x) rôle(s) approprié(s)

## 🧪 Étape 4 : Tests unitaires (optionnel mais recommandé)

### 4.1 Tester les repositories

- [ ] Test ajout d'un résultat
- [ ] Test récupération par ID
- [ ] Test récupération par CommandeCompteurId
- [ ] Test modification d'un résultat
- [ ] Test suppression (soft delete)

### 4.2 Tester les handlers

- [ ] Test AddCommandHandler
- [ ] Test EditCommandHandler
- [ ] Test DeleteCommandHandler
- [ ] Test GetByIdQueryHandler
- [ ] Test GetAllQueryHandler
- [ ] Test GetByCommandeCompteurIdQueryHandler

## 🚀 Étape 5 : Tests d'intégration

### 5.1 Démarrer l'application

```bash
cd c:\Users\annezan\source\repos\DLMS\API\DLMS.API
dotnet run
```

- [ ] L'application démarre sans erreur
- [ ] Swagger est accessible (https://localhost:XXXX/swagger)

### 5.2 Tester via Swagger

#### Test 1 : Ajouter un résultat

```json
POST /api/ResultatCommandeCompteur/add
{
  "commandeCompteurId": 1,
  "codeObisId": 1,
  "gxdlmsprofilgenericId": 1,
  "value": "TEST_123",
  "numeroCompteur": "TEST-001",
  "dateEnr": "2026-01-13T15:00:00",
  "isArchive": false,
  "numeroTentative": 1
}
```

- [ ] Statut 200 OK
- [ ] Résultat créé avec un ID
- [ ] Message de succès retourné

#### Test 2 : Récupérer le résultat créé

```
GET /api/ResultatCommandeCompteur/getResultatById?id={id}
```

- [ ] Statut 200 OK
- [ ] Données correctes retournées

#### Test 3 : Récupérer par CommandeCompteurId

```
GET /api/ResultatCommandeCompteur/getResultatsByCommandeCompteurId?commandeCompteurId=1
```

- [ ] Statut 200 OK
- [ ] Liste contenant le résultat créé

#### Test 4 : Modifier le résultat

```json
PUT /api/ResultatCommandeCompteur/edit
{
  "id": {id},
  "commandeCompteurId": 1,
  "codeObisId": 1,
  "gxdlmsprofilgenericId": 1,
  "value": "TEST_MODIFIE",
  "numeroCompteur": "TEST-001",
  "dateEnr": "2026-01-13T15:00:00",
  "isArchive": false,
  "numeroTentative": 1
}
```

- [ ] Statut 200 OK
- [ ] Valeur modifiée correctement

#### Test 5 : Supprimer le résultat

```json
DELETE /api/ResultatCommandeCompteur/delete
{
  "id": {id}
}
```

- [ ] Statut 200 OK
- [ ] Résultat marqué comme archivé (IsArchive = true)

#### Test 6 : Vérifier que le résultat n'apparaît plus

```
GET /api/ResultatCommandeCompteur
```

- [ ] Le résultat supprimé n'apparaît pas dans la liste

### 5.3 Tester les permissions

- [ ] Utilisateur sans permission ne peut pas accéder aux endpoints
- [ ] Message d'erreur approprié retourné (401 ou 403)

## 📊 Étape 6 : Migration des données existantes (si nécessaire)

### 6.1 Vérifier s'il y a des données à migrer

```sql
SELECT COUNT(*) 
FROM CommandeCompteur 
WHERE Resultats IS NOT NULL 
  AND Resultats != ''
  AND IsArchive = 0;
```

- [ ] Nombre de lignes à migrer identifié

### 6.2 Exécuter le script de migration

Voir le fichier `MIGRATION_RESULTAT_COMMANDE_COMPTEUR.md` pour le script SQL complet.

- [ ] Script de migration exécuté
- [ ] Données migrées avec succès
- [ ] Vérification que les données sont correctes

### 6.3 Vérifier la migration

```sql
SELECT COUNT(*) FROM ResultatCommandeCompteur WHERE CreatedBy = 'Migration';
```

- [ ] Nombre correct de lignes migrées

## 📝 Étape 7 : Documentation et communication

### 7.1 Documenter les changements

- [ ] Mettre à jour la documentation API
- [ ] Ajouter des exemples d'utilisation
- [ ] Documenter les nouveaux endpoints dans Swagger

### 7.2 Communiquer avec l'équipe

- [ ] Informer l'équipe frontend des nouveaux endpoints
- [ ] Partager les exemples d'utilisation
- [ ] Planifier la mise à jour du code existant

## 🔄 Étape 8 : Intégration dans le code existant

### 8.1 Identifier les endroits où CommandeCompteur.Resultats est utilisé

```bash
# Rechercher dans le code
grep -r "\.Resultats" --include="*.cs"
grep -r "Dateenrresultat" --include="*.cs"
```

- [ ] Liste des fichiers à modifier créée

### 8.2 Mettre à jour le code

Pour chaque utilisation trouvée :

- [ ] Remplacer l'accès direct à `Resultats` par la création d'un `ResultatCommandeCompteur`
- [ ] Tester le changement
- [ ] Commit le changement

## ✅ Étape 9 : Validation finale

### 9.1 Tests fonctionnels

- [ ] Créer une commande compteur
- [ ] Exécuter la commande et enregistrer le résultat
- [ ] Consulter l'historique des résultats
- [ ] Modifier un résultat si nécessaire
- [ ] Archiver un résultat

### 9.2 Tests de performance

- [ ] Mesurer le temps de réponse des nouveaux endpoints
- [ ] Vérifier qu'il n'y a pas de régression de performance

### 9.3 Tests de sécurité

- [ ] Vérifier que les permissions fonctionnent correctement
- [ ] Tester les cas limites (IDs invalides, etc.)
- [ ] Vérifier qu'on ne peut pas accéder aux résultats sans autorisation

## 🎉 Étape 10 : Déploiement

### 10.1 Préparer le déploiement

- [ ] Créer un script de migration pour la production
- [ ] Préparer la documentation de déploiement
- [ ] Créer un plan de rollback si nécessaire

### 10.2 Déployer en environnement de test

- [ ] Déployer en environnement de test
- [ ] Exécuter tous les tests
- [ ] Valider avec l'équipe

### 10.3 Déployer en production

- [ ] Backup de la base de données production
- [ ] Déployer l'application
- [ ] Exécuter les migrations
- [ ] Vérifier que tout fonctionne
- [ ] Monitorer les logs

## 📚 Ressources

- `MIGRATION_RESULTAT_COMMANDE_COMPTEUR.md` - Guide de migration détaillé
- `RESUME_MODIFICATIONS.md` - Résumé des changements
- `EXEMPLES_UTILISATION_RESULTAT.md` - Exemples d'utilisation

## ⚠️ En cas de problème

### Si la compilation échoue

1. Vérifier les références de projet
2. Nettoyer et rebuilder : `dotnet clean && dotnet build`
3. Vérifier les namespaces

### Si la migration échoue

1. Vérifier la chaîne de connexion
2. Vérifier les permissions sur la base de données
3. Consulter les logs d'erreur

### Si les tests échouent

1. Vérifier que les permissions sont bien créées
2. Vérifier que les repositories sont bien enregistrés dans DI
3. Vérifier les logs de l'application

## 📞 Support

Pour toute question ou problème :
- Consulter les fichiers de documentation
- Vérifier les logs de l'application
- Contacter l'équipe de développement

---

**Date de création** : 13 janvier 2026
**Version** : 1.0
**Statut** : ✅ Prêt pour l'implémentation
