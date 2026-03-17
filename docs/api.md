---
title: API Reference
nav_order: 4
---

# Interface `IReport`

Tous les générateurs de rapports implémentent l'interface commune suivante :

```csharp
public interface IReport
{
    string GenerateReport(
        string templateDocPath,
        string outputPathFile,
        Dictionary<string, string?> datas,
        string ownerPassword = "MATTAR.Reporting"
    );
}
```

---

## Paramètres

| Paramètre         | Type                          | Description                                                        |
|-------------------|-------------------------------|--------------------------------------------------------------------|
| `templateDocPath` | `string`                      | Chemin vers le fichier template                                    |
| `outputPathFile`  | `string`                      | Chemin de destination du fichier généré                            |
| `datas`           | `Dictionary<string, string?>` | Dictionnaire des champs à remplir (`nom du champ` → `valeur`)      |
| `ownerPassword`   | `string`                      | Mot de passe propriétaire du PDF (optionnel, défaut : `"MATTAR.Reporting"`) |

**Retour :** le chemin du fichier généré (`outputPathFile`).

---

## Implémentations disponibles

| Classe        | Description                                                          |
|---------------|----------------------------------------------------------------------|
| `PdfReport`   | Génère un PDF en remplissant les champs AcroForm d'un template PDF   |
| `HtmlReport`  | Génère un PDF ou HTML depuis un template HTML (moteur Scriban)       |
