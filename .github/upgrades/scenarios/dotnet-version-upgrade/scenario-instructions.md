# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatique
- **Version cible**: net11.0 (.NET 11.0 — PREVIEW)

## Strategy
**Selected**: All-at-Once
**Rationale**: 5 projets, tous sur net10.0, graphe de dépendances à 2 niveaux, sans contrainte CI.

### Execution Constraints
- Mise à niveau atomique — tous les projets mis à jour ensemble en une seule passe
- Séquence : mettre à jour les TFM → mettre à jour les packages → corriger les erreurs de compilation → valider
- Valider le build complet de la solution après la mise à niveau avant de conclure
- Commits : après chaque tâche complétée

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

## Source Control
- **Source Branch**: main
- **Working Branch**: upgrade-dotnet-11
- **Commit Strategy**: After Each Task
