---
title: Sécurité
nav_order: 5
---

# 🔐 Sécurité du document généré

Par défaut, le document PDF généré est protégé par un mot de passe propriétaire (`"MATTAR.Reporting"`). Les permissions appliquées sont :

| Permission                      | Valeur        |
|---------------------------------|---------------|
| Impression normale              | ✅ Autorisé   |
| Impression haute qualité        | ✅ Autorisé   |
| Annotations                     | ❌ Refusé     |
| Remplissage de formulaires      | ❌ Refusé     |
| Extraction du contenu           | ❌ Refusé     |
| Extraction pour accessibilité   | ❌ Refusé     |
| Modification du document        | ❌ Refusé     |
| Assemblage du document          | ❌ Refusé     |

---

## Personnaliser le mot de passe

Vous pouvez personnaliser le mot de passe en passant le paramètre `ownerPassword` :

```csharp
report.GenerateReport(templatePath, outputPath, datas, ownerPassword: "MonMotDePasse");
```

Pour désactiver la protection, passez une chaîne vide ou `null` :

```csharp
report.GenerateReport(templatePath, outputPath, datas, ownerPassword: "");
```
