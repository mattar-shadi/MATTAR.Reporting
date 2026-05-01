# 02-upgrade-all-projects: Mise à niveau de tous les projets vers net11.0

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
