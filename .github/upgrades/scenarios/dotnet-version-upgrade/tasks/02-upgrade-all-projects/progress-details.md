# progress-details.md — 02-upgrade-all-projects

## Résumé

Mise à niveau de 5 projets de net10.0 vers net11.0. Build propre, 36/36 tests passés.

## Fichiers modifiés

### Frameworks cibles (TFM)
- `src/MATTAR.Reporting.csproj` — `net10.0;net10.0-browser` → `net11.0;net11.0-browser` + conditions MSBuild mises à jour
- `samples/Blazor-Wasm/MattarReportBlazor.csproj` — `net10.0` → `net11.0`
- `samples/MATTAR.Reporting.BlazorSample/MATTAR.Reporting.BlazorSample.csproj` — `net10.0` → `net11.0`
- `sample/MATTAR.Reporting.Console.csproj` — `net10.0` → `net11.0`
- `tests/MATTAR.Reporting.Tests/MATTAR.Reporting.Tests.csproj` — `net10.0` → `net11.0`

### Packages mis à jour
- `Microsoft.AspNetCore.Components.WebAssembly` : `10.0.7` → `11.0.0-preview.3.26207.106` (2 projets Blazor)
- `Microsoft.AspNetCore.Components.WebAssembly.DevServer` : `10.0.7` → `11.0.0-preview.3.26207.106` (2 projets Blazor)
- `SixLabors.ImageSharp` ajouté en référence explicite `3.1.12` dans `MattarReportBlazor.csproj` pour neutraliser les vulnérabilités NU1902/NU1903 de la version transitive 1.0.4

### Signaux Api.0003 (System.Uri)
- Vérifiés dans `samples/Blazor-Wasm/Program.cs` ligne 9 et `samples/MATTAR.Reporting.BlazorSample/Program.cs` ligne 8 — `new Uri(builder.HostEnvironment.BaseAddress)` compile et fonctionne correctement sous net11.0, aucune modification requise.

### Package xunit (NuGet.0005)
- xunit 2.9.3 reste à cette version — c'est la version supportée pour net11.0 selon get_supported_package_version. La dépréciation concerne la roadmap v3, pas une incompatibilité avec net11.0.

## Résultats de validation

- ✅ `dotnet build` — 0 erreur, 0 avertissement
- ✅ `dotnet test` — 36/36 tests passés (net11.0)
