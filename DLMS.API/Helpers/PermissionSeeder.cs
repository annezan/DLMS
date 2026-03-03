using DLMS_DAL.Datas;
using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS.API.Helpers
{
    /// <summary>
    /// Seeder pour créer automatiquement les permissions par concept au démarrage de l'application
    /// Utilise le système par CONCEPT (VIEW_*, CREATE_*, etc.) au lieu des URLs
    /// </summary>
    public static class PermissionSeeder
    {
        public static async Task SeedPermissions(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();
                var existingActions = context.Permissions.Select(p => p.Action).ToHashSet();

                // 📋 Liste complète des 49 permissions par concept
                var conceptPermissions = GetConceptPermissions();
                var newPermissions = new List<Permission>();

                foreach (var (action, libelle, description) in conceptPermissions)
                {
                    if (!existingActions.Contains(action))
                    {
                        newPermissions.Add(new Permission
                        {
                            Action = action,
                            Libelle = libelle,
                            Description = description
                        });
                    }
                }

                if (newPermissions.Any())
                {
                    Console.WriteLine($"🔥 Création de {newPermissions.Count} nouvelles permissions...");
                    await context.Permissions.AddRangeAsync(newPermissions);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"✅ {newPermissions.Count} permissions créées avec succès !");
                }
                else
                {
                    Console.WriteLine("✅ Toutes les permissions existent déjà.");
                }
            }
        }

        /// <summary>
        /// Retourne la liste complète des 49 permissions par concept
        /// </summary>
        private static List<(string Action, string Libelle, string Description)> GetConceptPermissions()
        {
            return new List<(string, string, string)>
            {
                // ================================================================
                // 1. INFRASTRUCTURE & SÉCURITÉ (11 permissions)
                // ================================================================
                
                // Utilisateurs (6 permissions)
                ("VIEW_USER", "Voir les utilisateurs", "Permet de visualiser la liste des utilisateurs et leurs détails"),
                ("CREATE_USER", "Créer un utilisateur", "Permet de créer un nouvel utilisateur dans le système"),
                ("EDIT_USER", "Modifier un utilisateur", "Permet de modifier les informations d'un utilisateur existant"),
                ("DELETE_USER", "Supprimer un utilisateur", "Permet de supprimer un utilisateur du système"),
                ("UNLOCK_USER", "Débloquer un compte utilisateur", "Permet de débloquer le compte d'un utilisateur verrouillé"),
                ("ASSIGN_POSTE", "Assigner un poste à un utilisateur", "Permet d'assigner ou retirer un poste à un utilisateur pour limiter son accès"),
                
                // Rôles (4 permissions)
                ("VIEW_ROLE", "Voir les rôles", "Permet de visualiser la liste des rôles et leurs détails"),
                ("CREATE_ROLE", "Créer un rôle", "Permet de créer un nouveau rôle"),
                ("EDIT_ROLE", "Modifier un rôle", "Permet de modifier un rôle existant"),
                ("DELETE_ROLE", "Supprimer un rôle", "Permet de supprimer un rôle"),
                
                // Permissions (1 permission)
                ("VIEW_PERMISSION", "Voir les permissions", "Permet de visualiser la liste des permissions et leurs détails"),

                // ================================================================
                // 2. GESTION DE POSTES (12 permissions)
                // ================================================================
                
                // Postes (4 permissions)
                ("VIEW_POSTE", "Voir les postes", "Permet de visualiser les postes électriques. Les utilisateurs avec un poste assigné ne voient que leur poste."),
                ("CREATE_POSTE", "Créer un poste", "Permet de créer un nouveau poste électrique"),
                ("EDIT_POSTE", "Modifier un poste", "Permet de modifier un poste existant. Les utilisateurs avec un poste assigné ne peuvent modifier que leur poste."),
                ("DELETE_POSTE", "Supprimer un poste", "Permet de supprimer un poste. Les utilisateurs avec un poste assigné ne peuvent supprimer que leur poste."),
                
                // Cellules (4 permissions)
                ("VIEW_CELLULE", "Voir les cellules", "Permet de visualiser les cellules électriques. Les utilisateurs avec un poste assigné ne voient que les cellules de leur poste."),
                ("CREATE_CELLULE", "Créer une cellule", "Permet de créer une nouvelle cellule électrique. Les utilisateurs avec un poste assigné ne peuvent créer que dans leur poste."),
                ("EDIT_CELLULE", "Modifier une cellule", "Permet de modifier une cellule existante. Les utilisateurs avec un poste assigné ne peuvent modifier que les cellules de leur poste."),
                ("DELETE_CELLULE", "Supprimer une cellule", "Permet de supprimer une cellule. Les utilisateurs avec un poste assigné ne peuvent supprimer que les cellules de leur poste."),
                
                // Équipements (4 permissions)
                ("VIEW_EQUIPEMENT", "Voir les équipements", "Permet de visualiser les équipements électriques. Les utilisateurs avec un poste assigné ne voient que les équipements de leur poste."),
                ("CREATE_EQUIPEMENT", "Créer un équipement", "Permet de créer un nouvel équipement électrique. Les utilisateurs avec un poste assigné ne peuvent créer que dans leur poste."),
                ("EDIT_EQUIPEMENT", "Modifier un équipement", "Permet de modifier un équipement existant. Les utilisateurs avec un poste assigné ne peuvent modifier que les équipements de leur poste."),
                ("DELETE_EQUIPEMENT", "Supprimer un équipement", "Permet de supprimer un équipement. Les utilisateurs avec un poste assigné ne peuvent supprimer que les équipements de leur poste."),

                // ================================================================
                // 3. GESTION DE COMPTEURS (14 permissions)
                // ================================================================
                
                // Compteurs (5 permissions)
                ("VIEW_COMPTEUR", "Voir les compteurs", "Permet de visualiser les compteurs intelligents DLMS. Les utilisateurs avec un poste assigné ne voient que les compteurs associés aux équipements de leur poste."),
                ("CREATE_COMPTEUR", "Créer un compteur", "Permet de créer un nouveau compteur intelligent DLMS"),
                ("EDIT_COMPTEUR", "Modifier un compteur", "Permet de modifier un compteur existant. Les utilisateurs avec un poste assigné ne peuvent modifier que les compteurs de leur poste."),
                ("DELETE_COMPTEUR", "Supprimer un compteur", "Permet de supprimer un compteur. Les utilisateurs avec un poste assigné ne peuvent supprimer que les compteurs de leur poste."),
                ("SYNC_COMPTEUR", "Synchroniser un compteur", "Permet de mettre à jour les données d'un compteur DLMS (opération de synchronisation)"),
                
                // Associations Compteur-Équipement (3 permissions)
                ("VIEW_COMPTEUR_EQUIPEMENT", "Voir les associations compteur-équipement", "Permet de visualiser les associations entre compteurs et équipements"),
                ("CREATE_COMPTEUR_EQUIPEMENT", "Créer une association compteur-équipement", "Permet d'associer un compteur à un équipement. Nécessite l'accès au compteur ET à l'équipement."),
                ("DELETE_COMPTEUR_EQUIPEMENT", "Supprimer une association compteur-équipement", "Permet de dissocier un compteur d'un équipement. Nécessite l'accès au compteur ET à l'équipement."),
                
                // Associations Compteur-Cellule (3 permissions)
                ("VIEW_COMPTEUR_CELLULE", "Voir les associations compteur-cellule", "Permet de visualiser les associations entre compteurs et cellules"),
                ("CREATE_COMPTEUR_CELLULE", "Créer une association compteur-cellule", "Permet d'associer un compteur à une cellule. Nécessite l'accès au compteur ET à la cellule."),
                ("DELETE_COMPTEUR_CELLULE", "Supprimer une association compteur-cellule", "Permet de dissocier un compteur d'une cellule. Nécessite l'accès au compteur ET à la cellule."),
                
                // Commandes DLMS (4 permissions)
                ("VIEW_COMMANDE", "Voir les commandes", "Permet de visualiser les commandes DLMS. Les utilisateurs avec un poste assigné ne voient que les commandes dont TOUS les compteurs associés sont dans leur poste."),
                ("CREATE_COMMANDE", "Créer une commande", "Permet de créer une nouvelle commande DLMS à envoyer aux compteurs"),
                ("EDIT_COMMANDE", "Modifier une commande", "Permet de modifier une commande existante. L'utilisateur doit avoir accès à tous les compteurs de la commande."),
                ("DELETE_COMMANDE", "Supprimer une commande", "Permet de supprimer une commande. L'utilisateur doit avoir accès à tous les compteurs de la commande."),
                
                // Associations Commande-Compteur (2 permissions)
                ("VIEW_COMMANDE_COMPTEUR", "Voir les associations commande-compteur", "Permet de visualiser les associations entre commandes et compteurs"),
                ("DELETE_COMMANDE_COMPTEUR", "Supprimer une association commande-compteur", "Permet de retirer un compteur d'une commande. Nécessite l'accès à la commande ET au compteur."),
                
                // ⚠️ Clés d'Association DLMS - PERMISSIONS DÉSACTIVÉES
                // Les endpoints AssociationKey ne sont plus accessibles via l'API
                // ("VIEW_ASSOCIATIONKEY", "Voir les clés d'association", "Permet de visualiser les clés de chiffrement pour les communications DLMS"),
                // ("CREATE_ASSOCIATIONKEY", "Créer une clé d'association", "Permet de créer une nouvelle clé de chiffrement pour les communications DLMS"),

                // ================================================================
                // 4. TABLES DE RÉFÉRENCE (9 permissions)
                // ================================================================
                
                // Codes OBIS (1 permission)
                ("VIEW_CODEOBIS", "Voir les codes OBIS", "Permet de visualiser les codes OBIS standard du protocole DLMS (table de référence globale)"),
                
                // Fabricants (4 permissions)
                ("VIEW_FABRICANT", "Voir les fabricants", "Permet de visualiser la liste des fabricants de compteurs et équipements électriques (table de référence globale)"),
                ("CREATE_FABRICANT", "Créer un fabricant", "Permet d'ajouter un nouveau fabricant dans la liste de référence"),
                ("EDIT_FABRICANT", "Modifier un fabricant", "Permet de modifier les informations d'un fabricant existant"),
                ("DELETE_FABRICANT", "Supprimer un fabricant", "Permet de supprimer un fabricant de la liste de référence"),
                
                // Types de Commandes (1 permission)
                ("VIEW_TYPECOMMANDE", "Voir les types de commandes", "Permet de visualiser les types de commandes DLMS disponibles (table de référence globale)"),
                
                // Alarmes (1 permission)
                ("VIEW_ALARM", "Voir les alarmes", "Permet de visualiser les codes d'alarmes standard DLMS (table de référence globale)"),
                
                // Événements (1 permission)
                ("VIEW_EVENT", "Voir les événements", "Permet de visualiser les codes d'événements standard DLMS (table de référence globale)"),
                
                // Erreurs (1 permission)
                ("VIEW_ERROR", "Voir les erreurs", "Permet de visualiser les codes d'erreurs DLMS (table de référence globale)"),

                // ================================================================
                // 5. PROFILS DLMS (3 permissions)
                // ================================================================
                
                // Profils Génériques DLMS (3 permissions)
                ("VIEW_DLMS_PROFILE", "Voir les profils DLMS", "Permet de visualiser les profils génériques DLMS et leurs données historiques"),
                ("CREATE_DLMS_PROFILE", "Créer un profil DLMS", "Permet d'ajouter des détails ou événements dans les profils génériques DLMS"),
                ("DELETE_DLMS_PROFILE", "Supprimer un profil DLMS", "Permet de supprimer des détails ou événements dans les profils génériques DLMS")
            };
        }
    }
}
