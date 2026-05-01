# .NET Version Upgrade Plan

## Overview

**Target**: Mise à niveau de net10.0 vers net11.0 (Preview)
**Scope**: 5 projets — 1 bibliothèque principale, 2 applications Blazor, 1 console, 1 projet de tests

### Selected Strategy
**All-At-Once** — Tous les projets mis à niveau simultanément en une seule opération atomique.
**Rationale**: 5 projets, tous sur net10.0, graphe de dépendances à 2 niveaux, sans contrainte CI.

## Tasks

### 01-prerequisites: Vérification des prérequis SDK

Vérifier que le SDK .NET 11 est installé et disponible, et que les fichiers `global.json` éventuels sont compatibles avec la cible net11.0.

La solution utilise .NET 10 comme base. Avant toute modification des projets, il faut s'assurer que l'environnement de développement dispose du SDK .NET 11 Preview et que son utilisation n'est pas bloquée par un `global.json` restrictif.

**Done when**: Le SDK net11.0 est confirmé installé et aucun `global.json` ne bloque son utilisation.

---

### 02-upgrade-all-projects: Mise à niveau de tous les projets vers net11.0

Mettre à niveau les 5 projets de la solution de `net10.0` vers `net11.0`, en mettant à jour les frameworks cibles, les packages NuGet, et en corrigeant les changements de comportement d'API identifiés.

Projets concernés :
- `src\MATTAR.Reporting.csproj` — bibliothèque principale (cibles `net10.0;net10.0-browser` → `net11.0;net11.0-browser`)
- `samples\Blazor-Wasm\MattarReportBlazor.csproj` — application Blazor WebAssembly
- `samples\MATTAR.Reporting.BlazorSample\MATTAR.Reporting.BlazorSample.csproj` — application Blazor
- `sample\MATTAR.Reporting.Console.csproj` — application console
- `tests\MATTAR.Reporting.Tests\MATTAR.Reporting.Tests.csproj` — tests unitaires

Signaux de l'évaluation à surveiller pendant l'exécution :
- **Api.0003** (potentiel) dans les 2 projets Blazor : `System.Uri` et `Uri(string)` dans `Program.cs` ligne 8-9 — changement de comportement dans net11. Vérifier si la construction `new Uri(builder.HostEnvironment.BaseAddress)` est toujours valide ou requiert un ajustement.
- **NuGet.0005** (optionnel) dans le projet de tests : `xunit 2.9.3` est déprécié. Migrer vers xunit v3 ou la dernière version v2 stable selon les recommandations de la migration xunit.net v2→v3.
- La bibliothèque `MATTAR.Reporting.csproj` cible deux frameworks (`net10.0;net10.0-browser`) — mettre à jour les deux cibles vers `net11.0;net11.0-browser`.

**Done when**: Tous les fichiers `.csproj` ciblent net11.0, tous les packages sont mis à jour, la solution compile sans erreur, les tests passent.

---

### 03-final-validation: Validation finale de la solution

Effectuer une validation complète de la solution après la mise à niveau : build complet, exécution de la suite de tests, et vérification qu'il n'y a ni erreur ni avertissement non résolu.

**Done when**: `dotnet build` réussit sans erreur sur toute la solution, tous les tests passent, aucun avertissement résiduel non traité.
