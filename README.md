# MATTAR.Reporting

[![CI - Build & Test](https://github.com/mattar-shadi/MATTAR.Reporting/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/mattar-shadi/MATTAR.Reporting/actions/workflows/ci.yml?query=branch%3Amain)
[![NuGet](https://img.shields.io/nuget/v/MATTAR.Reporting.svg)](https://www.nuget.org/packages/MATTAR.Reporting/)

**MATTAR.Reporting** est une bibliothèque C# .NET simple et légère permettant de générer des rapports en remplissant des gabarits de documents (templates). Elle supporte actuellement la génération de **PDF** via des formulaires AcroForm, avec un support **HTML** prévu dans les prochaines versions.

---

## 🎯 Démo Interactive

**Essayez la démo Blazor WebAssembly en ligne :**

👉 **[Démo MATTAR.Reporting sur GitHub Pages](https://mattar-shadi.github.io/MATTAR.Reporting/samples/Blazor-Wasm/)**

Cette démo interactive vous permet de :
- 📄 Générer des rapports PDF directement dans le navigateur
- 🌐 Générer du HTML à partir de templates
- 📋 Tester les 3 templates prédéfinis (Facture, Rapport Mensuel, Certificat)
- ⚡ Voir les résultats en temps réel
- 💾 Télécharger les documents générés

**100% côté client** - Aucune donnée n'est envoyée à un serveur.

Voir le code source : [`samples/Blazor-Wasm/`](https://github.com/mattar-shadi/MATTAR.Reporting/tree/main/samples/Blazor-Wasm)

---

## 📦 Fonctionnalités

- ✅ Génération de rapports PDF à partir d'un template PDF contenant des champs de formulaire (AcroForm)
- ✅ Remplissage dynamique des champs via un dictionnaire `clé → valeur`
- ✅ Protection du document généré par mot de passe propriétaire
- ✅ Création automatique du dossier de sortie si inexistant
- ✅ Permissions de sécurité configurables (impression, annotations, extraction...)
- ✅ Génération de rapports depuis un template HTML avec remplacement de placeholders (via Scriban)
- ✅ Export PDF depuis un template HTML (via HtmlRenderer.PdfSharp)
- ✅ Support des images (injection base64 dans les rapports HTML, dessin sur les champs PDF)
- ✅ **Compatible Blazor WebAssembly** - Génération 100% côté client (v1.1.0+)

---

## 🏗️ Structure du projet

```
MATTAR.Reporting/
├── src/
│   ├── Interfaces/
│   │   └── IReport.cs          # Interface commune à tous les générateurs de rapports
│   ├── PdfReport.cs            # Implémentation PDF (via PdfSharpCore)
│   ├── HtmlReport.cs           # Implémentation HTML (via Scriban + HtmlRenderer.PdfSharp)
│   ├── MigraDocReport.cs       # Implémentation MigraDoc (compatible WASM)
│   └── MATTAR.Reporting.csproj # Projet de la bibliothèque
├── samples/
│   ├── Blazor-Wasm/            # 🆕 Démo interactive Blazor WebAssembly
│   │   ├── Pages/
│   │   ├── Services/
│   │   └── MattarReportBlazor.csproj
│   └── Console/
│       ├── Program.cs          # Exemple d'utilisation console
│       ├── FACTURE.pdf         # Template PDF exemple
│       └── MATTAR.Reporting.Console.csproj
├── MATTAR.Reporting.sln
└── README.md
```

---

## 🚀 Installation

### Via NuGet

Disponible sur NuGet : [https://www.nuget.org/packages/MATTAR.Reporting/](https://www.nuget.org/packages/MATTAR.Reporting/)

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

- [.NET 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) ou supérieur
- Dépendances NuGet :
  - [`PdfSharpCore`](https://www.nuget.org/packages/PdfSharpCore/) `1.3.56`
  - [`Scriban`](https://www.nuget.org/packages/Scriban/) `7.0.3`
  - [`HtmlRenderer.PdfSharp.NetStandard2`](https://www.nuget.org/packages/HtmlRenderer.PdfSharp.NetStandard2/) `1.5.1.3`
  - [`MigraDocCore.DocumentObjectModel`](https://www.nuget.org/packages/MigraDocCore.DocumentObjectModel/) `1.3.56` (pour WASM)

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

### 3. Générer un rapport depuis un template HTML

Créez un fichier HTML contenant des **placeholders** au format `{{ NomDuChamp }}` (syntaxe Scriban). Ces noms correspondent aux clés du dictionnaire de données.

```csharp
using MATTAR.Reporting;

IReport htmlReport = new HtmlReport();

string templatePath = "templates/FACTURE.html";

// Générer un PDF depuis un template HTML
string outputPdfPath = $"Output/{DateTime.Now:yyyy-MM-dd}_FACTURE.pdf";
string generatedPdfPath = htmlReport.GenerateReport(
    templatePath,
    outputPdfPath,
    new Dictionary<string, string?>
    {
        { "Number", "F2231323" },
        { "Name",   "MATTAR SASU" },
        { "Date",   DateTime.Now.ToShortDateString() },
        { "Total",  "107 100" }
    }
);

// Générer un fichier HTML depuis un template HTML
string outputHtmlPath = $"Output/{DateTime.Now:yyyy-MM-dd}_FACTURE.html";
string generatedHtmlPath = htmlReport.GenerateReport(
    templatePath,
    outputHtmlPath,
    new Dictionary<string, string?>
    {
        { "Number", "F2231323" },
        { "Name",   "MATTAR SASU" },
        { "Date",   DateTime.Now.ToShortDateString() },
        { "Total",  "107 100" }
    }
);

Console.WriteLine("PDF généré : " + Path.GetFullPath(generatedPdfPath));
Console.WriteLine("HTML généré : " + Path.GetFullPath(generatedHtmlPath));
```

Exemple de template HTML (`FACTURE.html`) :

```html
<!DOCTYPE html>
<html>
<body>
    <h1>Facture N° {{ Number }}</h1>
    <p>Client : {{ Name }}</p>
    <p>Date : {{ Date }}</p>
    <p>Total : {{ Total }} €</p>
</body>
</html>
```

> Les placeholders suivent la syntaxe **Scriban** (`{{ clé }}`). Toutes les clés du dictionnaire `datas` sont disponibles dans le template.

---

## 🖼️ Support des images

Le paramètre optionnel `images` permet d'injecter des images dans les rapports générés.

### Images dans un template HTML

Les images sont converties en **URI base64** et injectées comme des variables Scriban. Utilisez-les directement dans l'attribut `src` d'une balise `<img>` :

```csharp
IReport htmlReport = new HtmlReport();

string generatedPath = htmlReport.GenerateReport(
    "templates/FACTURE.html",
    "Output/FACTURE.html",
    new Dictionary<string, string?>
    {
        { "Number", "F2231323" },
        { "Name",   "MATTAR SASU" }
    },
    ownerPassword: null,
    images: new Dictionary<string, string?>
    {
        { "Logo", "assets/logo.png" }
    }
);
```

Template HTML correspondant (`FACTURE.html`) :

```html
<!DOCTYPE html>
<html>
<body>
    <img src="{{ Logo }}" style="width:200px;" />
    <h1>Facture N° {{ Number }}</h1>
    <p>Client : {{ Name }}</p>
</body>
</html>
```

Les types MIME reconnus automatiquement : `.png`, `.jpg`/`.jpeg`, `.gif`, `.webp`, `.bmp`.

### Images dans un template PDF (AcroForm)

Pour les templates PDF, les images sont dessinées directement sur la page aux coordonnées du champ AcroForm de type **bouton poussoir** (`PdfPushButtonField`) :

```csharp
IReport pdfReport = new PdfReport();

string generatedPath = pdfReport.GenerateReport(
    "templates/FACTURE.pdf",
    "Output/FACTURE.pdf",
    new Dictionary<string, string?>
    {
        { "Number", "F2231323" }
    },
    ownerPassword: null,
    images: new Dictionary<string, string?>
    {
        { "Logo", "assets/logo.png" }  // "Logo" doit être le nom d'un champ bouton dans le PDF
    }
);
```

> **Remarque :** Si le champ n'existe pas ou n'est pas de type bouton poussoir, l'image est ignorée silencieusement. Si le fichier image est introuvable, une `FileNotFoundException` est levée.

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
        string? ownerPassword = "MATTAR.Reporting",
        Dictionary<string, string?>? images = null
    );
}
```

| Paramètre        | Type                          | Description                                              |
|------------------|-------------------------------|----------------------------------------------------------|
| `templateDocPath`| `string`                      | Chemin vers le fichier template                          |
| `outputPathFile` | `string`                      | Chemin de destination du fichier généré                  |
| `datas`          | `Dictionary<string, string?>` | Dictionnaire des champs texte à remplir (`nom` → `valeur`) |
| `ownerPassword`  | `string?`                     | Mot de passe propriétaire du PDF (optionnel)             |
| `images`         | `Dictionary<string, string?>?`| Dictionnaire des images à insérer (`nom` → `chemin`)    |

**Retour :** le chemin du fichier généré (`outputPathFile`).

---

## 📌 Feuille de route

- [x] Génération PDF via AcroForm
- [x] Génération HTML (via Scriban + HtmlRenderer.PdfSharp)
- [x] Support MigraDoc (DDL templates) compatible WASM
- [x] Démo interactive Blazor WebAssembly
- [x] Support des images dans les champs
- [x] Publication NuGet officielle
- [ ] Export PDF depuis template HTML avancé
- [ ] Support des tableaux dynamiques

## 🚀 Exemples

### Utilisation en Console

Voir le dossier [`samples/Console/`](https://github.com/mattar-shadi/MATTAR.Reporting/tree/main/samples/Console) pour des exemples complets.

### Utilisation en Blazor WebAssembly

Voir le dossier [`samples/Blazor-Wasm/`](https://github.com/mattar-shadi/MATTAR.Reporting/tree/main/samples/Blazor-Wasm) pour une implémentation complète avec :
- Interface utilisateur interactive
- Génération de PDF côté client
- Génération de HTML côté client
- Téléchargement des documents générés

**Démo en ligne :** https://mattar-shadi.github.io/MATTAR.Reporting/samples/Blazor-Wasm/

---

## 📄 Licence

Ce projet est distribué sous licence **MIT**. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

---

## 🤝 Contribution

Les contributions sont les bienvenues ! N'hésitez pas à ouvrir une **issue** ou une **pull request**.

---

---

## 🎓 Ressources

- 📖 [Documentation complète](https://github.com/mattar-shadi/MATTAR.Reporting/wiki) (Wiki)
- 🔗 [Package NuGet](https://www.nuget.org/packages/MATTAR.Reporting/)
- 🐛 [Issues & Discussions](https://github.com/mattar-shadi/MATTAR.Reporting/issues)
- 🎯 [Démo interactive](https://mattar-shadi.github.io/MATTAR.Reporting/samples/Blazor-Wasm/)

---

*Développé par [mattar-shadi](https://github.com/mattar-shadi)*