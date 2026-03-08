# MATTAR.Reporting

**MATTAR.Reporting** est une bibliothèque C# .NET simple et légère permettant de générer des rapports en remplissant des gabarits de documents (templates). Elle supporte actuellement la génération de **PDF** via des formulaires AcroForm, avec un support **HTML** prévu dans les prochaines versions.

---

## 📦 Fonctionnalités

- ✅ Génération de rapports PDF à partir d'un template PDF contenant des champs de formulaire (AcroForm)
- ✅ Remplissage dynamique des champs via un dictionnaire `clé → valeur`
- ✅ Protection du document généré par mot de passe propriétaire
- ✅ Création automatique du dossier de sortie si inexistant
- ✅ Permissions de sécurité configurables (impression, annotations, extraction...)
- 🔜 Support HTML (en cours de développement)

---

## 🏗️ Structure du projet

```
MATTAR.Reporting/
├── src/
│   ├── Interfaces/
│   │   └── IReport.cs          # Interface commune à tous les générateurs de rapports
│   ├── PdfReport.cs            # Implémentation PDF (via PdfSharpCore)
│   ├── HtmlReport.cs           # Implémentation HTML (TODO)
│   └── MATTAR.Reporting.csproj # Projet de la bibliothèque
├── sample/
│   ├── Program.cs              # Exemple d'utilisation de la bibliothèque
│   ├── FACTURE.pdf             # Template PDF exemple
│   └── MATTAR.Reporting.Console.csproj
├── MATTAR.Reporting.sln
└── README.md
```

---

## 🚀 Installation

### Via NuGet *(si publié)*

```bash
dotnet add package MATTAR.Reporting
```

### Depuis les sources

```bash
git clone https://github.com/mattar-shadi/MATTAR.Reporting.git
cd MATTAR.Reporting
dotnet build
```

---

## 📋 Prérequis

- [.NET 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) ou supérieur
- Dépendance NuGet : [`PdfSharpCore`](https://www.nuget.org/packages/PdfSharpCore/) `1.3.56`

---

## 🛠️ Utilisation

### 1. Préparer un template PDF

Créez un fichier PDF contenant des **champs de formulaire AcroForm** (nommés). Ces noms seront utilisés comme clés dans le dictionnaire de données.

> Vous pouvez créer des templates avec des logiciels comme **LibreOffice**, **Adobe Acrobat**, ou **PDF-XChange Editor**.

---

### 2. Générer un rapport PDF

```csharp
using MATTAR.Reporting;

IReport report = new PdfReport();

string templatePath = "templates/FACTURE.pdf";
string outputPath = $"Output/{{DateTime.Now:yyyy-MM-dd}}_FACTURE.pdf";

string generatedFilePath = report.GenerateReport(
    templatePath,
    outputPath,
    new Dictionary<string, string?>
    {
        { "Number",          "F2231323" },
        { "CustomerNumber",  "C12354" },
        { "JourneyNumber",   "J2231323" },
        { "EntryNumber",     "E31FF323" },
        { "Date",            DateTime.Now.ToShortDateString() },
        { "Name",            "MATTAR SASU" },
        { "Line1",           "50 Avenue Pierre-George Latécoère" },
        { "ZipCode",         "31520" },
        { "City",            "RAMONVILLE-SAINT-AGNE (FRANCE)" },
        { "InvoiceLine1Description", "Stockage des marchandises (jour)" },
        { "InvoiceLine1UnitPrice",   "45 000" },
        { "InvoiceLine1Qty",         "2" },
        { "InvoiceLine1Total",       "90 000" },
        { "TotalBeforeTax",          "90 000" },
        { "TaxAmount",               "17 100" },
        { "Total",                   "107 100" }
    }
);

Console.WriteLine("Fichier généré : " + Path.GetFullPath(generatedFilePath));
```

---

## 🔐 Sécurité du document généré

Par défaut, le document PDF généré est protégé par un mot de passe propriétaire (`"MATTAR.Reporting"`). Les permissions appliquées sont :

| Permission                  | Valeur   |
|-----------------------------|----------|
| Impression normale          | ✅ Autorisé |
| Impression haute qualité    | ✅ Autorisé |
| Annotations                 | ❌ Refusé  |
| Remplissage de formulaires  | ❌ Refusé  |
| Extraction du contenu       | ❌ Refusé  |
| Modification du document    | ❌ Refusé  |
| Assemblage du document      | ❌ Refusé  |

Vous pouvez personnaliser le mot de passe en passant le paramètre `ownerPassword` :

```csharp
report.GenerateReport(templatePath, outputPath, datas, ownerPassword: "MonMotDePasse");
```

---

## 🔌 Interface `IReport`

Tous les générateurs implémentent l'interface commune suivante :

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

| Paramètre        | Type                          | Description                                              |
|------------------|-------------------------------|----------------------------------------------------------|
| `templateDocPath`| `string`                      | Chemin vers le fichier template                          |
| `outputPathFile` | `string`                      | Chemin de destination du fichier généré                  |
| `datas`          | `Dictionary<string, string?>` | Dictionnaire des champs à remplir (`nom du champ` → `valeur`) |
| `ownerPassword`  | `string`                      | Mot de passe propriétaire du PDF (optionnel)             |

**Retour :** le chemin du fichier généré (`outputPathFile`).

---

## 📌 Feuille de route

- [x] Génération PDF via AcroForm
- [ ] Génération HTML
- [ ] Export PDF depuis template HTML
- [ ] Support des images dans les champs
- [ ] Publication NuGet officielle

---

## 📄 Licence

Ce projet est distribué sous licence **MIT**. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

---

## 🤝 Contribution

Les contributions sont les bienvenues ! N'hésitez pas à ouvrir une **issue** ou une **pull request**.

---

*Développé par [mattar-shadi](https://github.com/mattar-shadi)*