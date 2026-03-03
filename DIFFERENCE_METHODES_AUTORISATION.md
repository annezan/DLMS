# 🔍 Différence : UserHasPosteAssignedAsync vs UserHasAccessToPosteAsync

## 📋 Vue d'Ensemble

| Méthode | Question Posée | Paramètres | Retour |
|---------|---------------|------------|--------|
| **UserHasPosteAssignedAsync** | "L'utilisateur a-t-il UN poste assigné ?" | `userId` | `bool` (oui/non) |
| **UserHasAccessToPosteAsync** | "L'utilisateur peut-il accéder à CE poste spécifique ?" | `userId`, `posteId` | `bool` (oui/non) |

---

## 1️⃣ UserHasPosteAssignedAsync

### 🎯 Objectif

Vérifier **SI** l'utilisateur a un poste assigné (sans savoir lequel).

### 📝 Signature

```csharp
Task<bool> UserHasPosteAssignedAsync(Guid userId)
```

### 💻 Implémentation

```csharp
public async Task<bool> UserHasPosteAssignedAsync(Guid userId)
{
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    
    // Retourne true si PosteId n'est pas null
    return user?.PosteId.HasValue ?? false;
}
```

### 🎯 Utilisation

**Question** : "Est-ce que cet utilisateur a un poste assigné ?"

```csharp
var userId = GetCurrentUserId();
var hasPoste = await AuthService.UserHasPosteAssignedAsync(userId);

if (hasPoste)
{
    // L'utilisateur a un poste assigné → Filtrer les données
    Console.WriteLine("Filtrage par poste activé");
}
else
{
    // L'utilisateur n'a pas de poste → Accès global
    Console.WriteLine("Accès global (pas de filtrage)");
}
```

### 📊 Exemples

| Utilisateur | PosteId | Résultat |
|------------|---------|----------|
| Admin | `null` | `false` ❌ (pas de poste) |
| Gestionnaire A | `1` | `true` ✅ (a un poste) |
| Gestionnaire B | `2` | `true` ✅ (a un poste) |
| Technicien | `null` | `false` ❌ (pas de poste) |

**Note** : Cette méthode ne dit PAS **quel** poste, juste **si** l'utilisateur en a un.

---

## 2️⃣ UserHasAccessToPosteAsync

### 🎯 Objectif

Vérifier si l'utilisateur peut accéder à **un poste SPÉCIFIQUE**.

### 📝 Signature

```csharp
Task<bool> UserHasAccessToPosteAsync(Guid userId, int posteId)
```

### 💻 Implémentation

```csharp
public async Task<bool> UserHasAccessToPosteAsync(Guid userId, int posteId)
{
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    if (user == null) return false;

    // Si l'utilisateur a un poste assigné
    if (user.PosteId.HasValue)
    {
        // Il ne peut accéder QUE à SON poste
        return user.PosteId.Value == posteId;
    }
    else
    {
        // Pas de poste assigné → Vérifier la permission générale
        return await UserHasPermissionAsync(userId, "VIEW_POSTE");
    }
}
```

### 🎯 Utilisation

**Question** : "Est-ce que cet utilisateur peut accéder au Poste #5 ?"

```csharp
var userId = GetCurrentUserId();
var posteId = 5;

var canAccess = await AuthService.UserHasAccessToPosteAsync(userId, posteId);

if (canAccess)
{
    // L'utilisateur peut voir le Poste #5
    var poste = await GetPosteById(posteId);
    return Ok(poste);
}
else
{
    // Accès refusé
    return Forbid();
}
```

### 📊 Exemples

**Scénario** : Vérifier l'accès au **Poste #1**

| Utilisateur | PosteId | Paramètre `posteId` | Résultat | Raison |
|------------|---------|---------------------|----------|---------|
| Admin | `null` | `1` | `true` ✅ | Pas de poste → Vérifie permission VIEW_POSTE |
| Gestionnaire A | `1` | `1` | `true` ✅ | Son poste (1 == 1) |
| Gestionnaire A | `1` | `2` | `false` ❌ | Pas son poste (1 ≠ 2) |
| Gestionnaire B | `2` | `1` | `false` ❌ | Pas son poste (2 ≠ 1) |

---

## 🔄 Différence Clé

### UserHasPosteAssignedAsync

```
Question : "A-t-il un poste ?"
Réponse  : Oui/Non (simple vérification d'existence)
```

### UserHasAccessToPosteAsync

```
Question : "Peut-il accéder à CE poste spécifique ?"
Réponse  : Oui/Non (vérification de correspondance ou permission)
```

---

## 🎯 Cas d'Usage Pratiques

### Cas 1 : Filtrer une Liste

**Besoin** : Savoir s'il faut filtrer les résultats ou non

```csharp
[HttpGet]
public async Task<ActionResult<List<CommandeResponse>>> Get()
{
    var userId = GetCurrentUserId();
    
    // ✅ Utiliser UserHasPosteAssignedAsync
    var hasPosteAssigne = await AuthService.UserHasPosteAssignedAsync(userId);
    
    if (hasPosteAssigne)
    {
        // Filtrer les résultats par poste
        var posteId = await AuthService.GetUserPosteIdAsync(userId);
        var commandes = await GetCommandesByPoste(posteId);
        return Ok(commandes);
    }
    else
    {
        // Retourner toutes les commandes (pas de filtrage)
        var commandes = await GetAllCommandes();
        return Ok(commandes);
    }
}
```

**Pourquoi pas UserHasAccessToPosteAsync ?**  
→ On ne vérifie pas l'accès à UN poste spécifique, on veut juste savoir s'il faut filtrer.

---

### Cas 2 : Vérifier l'Accès à une Ressource Spécifique

**Besoin** : Vérifier si l'utilisateur peut voir le Poste #5

```csharp
[HttpGet("{id}")]
public async Task<ActionResult<PosteResponse>> GetById(int id)
{
    var userId = GetCurrentUserId();
    
    // ✅ Utiliser UserHasAccessToPosteAsync
    var hasAccess = await AuthService.UserHasAccessToPosteAsync(userId, id);
    
    if (!hasAccess)
    {
        return Forbid("Vous ne pouvez pas accéder à ce poste");
    }
    
    var poste = await GetPosteById(id);
    return Ok(poste);
}
```

**Pourquoi pas UserHasPosteAssignedAsync ?**  
→ On vérifie l'accès à un poste SPÉCIFIQUE (#5), pas juste si l'utilisateur a un poste.

---

### Cas 3 : Créer une Ressource

**Besoin** : Vérifier si l'utilisateur peut créer une cellule dans un poste donné

```csharp
[HttpPost("add")]
public async Task<ActionResult> AddCellule(CelluleAddCommand command)
{
    var userId = GetCurrentUserId();
    
    // ✅ Utiliser UserHasAccessToPosteAsync
    var hasAccess = await AuthService.UserHasAccessToPosteAsync(userId, command.PosteId);
    
    if (!hasAccess)
    {
        return Forbid("Vous ne pouvez pas créer de cellule dans ce poste");
    }
    
    var result = await Mediator.Send(command);
    return Ok(result);
}
```

---

## 🎨 Diagramme de Décision

```
┌─────────────────────────────────────────┐
│  Quel type de vérification ?            │
└─────────────────────────────────────────┘
                 │
      ┌──────────┴──────────┐
      │                     │
      ↓                     ↓
┌─────────────┐      ┌─────────────┐
│ Je veux     │      │ Je veux     │
│ savoir SI   │      │ vérifier    │
│ l'utilisateur│     │ l'accès à   │
│ a un poste  │      │ UN poste    │
│ (oui/non)   │      │ SPÉCIFIQUE  │
└─────────────┘      └─────────────┘
      │                     │
      ↓                     ↓
┌─────────────┐      ┌─────────────┐
│ UserHas     │      │ UserHas     │
│ Poste       │      │ AccessTo    │
│ Assigned    │      │ Poste       │
│ Async       │      │ Async       │
└─────────────┘      └─────────────┘
```

---

## 💡 Exemple Complet dans le Code

### Dans GetCommandesQueryHandler

```csharp
public async Task<ResponseBase<List<CommandeResponse>>> Handle(...)
{
    var commandes = await _repository.GetCommandes();
    var commandesResponse = Mapper.Map<List<CommandeResponse>>(commandes);

    // 1️⃣ PREMIÈRE QUESTION : "A-t-il un poste assigné ?"
    var hasPosteAssigne = await _authService.UserHasPosteAssignedAsync(request.UserId);

    if (hasPosteAssigne)
    {
        // OUI → Il faut filtrer les commandes
        
        var commandesAccessibles = new List<CommandeResponse>();

        foreach (var commande in commandesResponse)
        {
            // 2️⃣ DEUXIÈME QUESTION : "Peut-il accéder à CETTE commande spécifique ?"
            var hasAccess = await _authService.UserHasAccessToCommandeAsync(request.UserId, commande.Id);
            
            if (hasAccess)
            {
                commandesAccessibles.Add(commande);
            }
        }

        responseBase.Data = commandesAccessibles;
    }
    else
    {
        // NON → Pas de filtrage, retourner toutes les commandes
        responseBase.Data = commandesResponse;
    }

    return responseBase;
}
```

**Logique** :
1. **Première question** (`UserHasPosteAssignedAsync`) : "Dois-je filtrer ?"
2. **Si oui**, pour chaque commande : "A-t-il accès à CELLE-CI ?" (`UserHasAccessToCommandeAsync`)

---

## 📊 Comparaison Directe

### Scénario : Admin (sans poste)

```csharp
// Admin avec PosteId = null

// Question 1 : A-t-il un poste ?
var hasPoste = await UserHasPosteAssignedAsync(adminId);
// → false ❌ (pas de poste assigné)

// Question 2 : Peut-il accéder au Poste #1 ?
var canAccessPoste1 = await UserHasAccessToPosteAsync(adminId, 1);
// → true ✅ (a la permission VIEW_POSTE)

// Question 3 : Peut-il accéder au Poste #2 ?
var canAccessPoste2 = await UserHasAccessToPosteAsync(adminId, 2);
// → true ✅ (a la permission VIEW_POSTE)
```

**Conclusion** : 
- N'a PAS de poste assigné → `UserHasPosteAssignedAsync` = false
- MAIS peut accéder à TOUS les postes → `UserHasAccessToPosteAsync` = true pour tous

---

### Scénario : Gestionnaire (avec Poste #1)

```csharp
// Gestionnaire avec PosteId = 1

// Question 1 : A-t-il un poste ?
var hasPoste = await UserHasPosteAssignedAsync(gestionnaireId);
// → true ✅ (PosteId = 1)

// Question 2 : Peut-il accéder au Poste #1 ?
var canAccessPoste1 = await UserHasAccessToPosteAsync(gestionnaireId, 1);
// → true ✅ (son poste : 1 == 1)

// Question 3 : Peut-il accéder au Poste #2 ?
var canAccessPoste2 = await UserHasAccessToPosteAsync(gestionnaireId, 2);
// → false ❌ (pas son poste : 1 ≠ 2)
```

**Conclusion** :
- A un poste assigné → `UserHasPosteAssignedAsync` = true
- Peut accéder SEULEMENT à son poste → `UserHasAccessToPosteAsync` dépend du posteId

---

## ✅ Résumé en Une Phrase

| Méthode | En une phrase |
|---------|--------------|
| **UserHasPosteAssignedAsync** | "Est-ce que `user.PosteId` n'est pas null ?" |
| **UserHasAccessToPosteAsync** | "Est-ce que `user.PosteId == posteId` OU est-ce qu'il a la permission VIEW_POSTE ?" |

---

## 🎯 Quand Utiliser Laquelle ?

### Utiliser UserHasPosteAssignedAsync quand :
- ✅ Vous voulez savoir **si** il faut filtrer les données
- ✅ Vous voulez adapter le comportement (filtrage oui/non)
- ✅ Vous ne vérifiez pas l'accès à une ressource spécifique

### Utiliser UserHasAccessToPosteAsync quand :
- ✅ Vous vérifiez l'accès à **un poste spécifique** (par son ID)
- ✅ Vous voulez autoriser/refuser l'accès à une ressource
- ✅ Vous avez un `posteId` en paramètre

---

**Est-ce que la différence est claire maintenant ?** 🎯

