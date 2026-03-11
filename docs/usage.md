---
title: Utilisation
nav_order: 3
---

# Utilisation

## 1. Préparer un template PDF

Créez un fichier PDF contenant des **champs de formulaire AcroForm** (nommés). Ces noms seront utilisés comme clés dans le dictionnaire de données.

> Vous pouvez créer des templates avec des logiciels comme **LibreOffice**, **Adobe Acrobat**, ou **PDF-XChange Editor**.

---

## 2. Générer un rapport PDF

```csharp
using MATTAR.Reporting;

IReport report = new PdfReport();

string templatePath = "templates/FACTURE.pdf";
string outputPath = $"Output/{DateTime.Now:yyyy-MM-dd}_FACTURE.pdf";

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

## 3. Générer un rapport depuis un template HTML

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

### Exemple de template HTML (`FACTURE.html`)

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
