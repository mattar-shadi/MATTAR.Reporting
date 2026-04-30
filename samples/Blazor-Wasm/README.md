# MATTAR.Report Blazor WebAssembly Demo

Une application **Blazor WebAssembly (WASM) 100% côté client** qui démontre la génération de documents professionnels en PDF et HTML à partir de templates, en utilisant la bibliothèque **MATTAR.Reporting** avec sa nouvelle implémentation compatible WASM.

## 🚀 Caractéristiques

- ✅ **Blazor WASM pur** - Exécution entièrement côté client, pas de serveur requis
- 📄 **Génération PDF** - Création de PDF professionnels à partir de templates MigraDoc DDL
- 🌐 **Génération HTML** - Export en HTML avec templating Scriban
- 🎨 **Interface moderne** - Design professionnel et intuitif
- 📋 **Templates prédéfinis** - Factures, rapports, certificats
- ⚡ **Aperçu en temps réel** - Visualisation instantanée des modifications
- 💾 **Téléchargement direct** - Exporte les documents sans serveur

## 📋 Templates Disponibles

### 1. Facture (Invoice)
- Numéro de facture
- Informations client
- Tableau d'articles avec calculs
- Total et montants

### 2. Rapport Mensuel (Monthly Report)
- En-tête avec informations de département
- Indicateurs clés de performance
- Tableau des projets avec statuts et budgets
- Notes et observations

### 3. Certificat (Certificate)
- Nom du destinataire
- Nom du cours/formation
- Date de complétion
- Numéro de certificat

## 🛠️ Architecture Technique

### Stack Technologique
- **Framework** : Blazor WebAssembly (.NET 10.0)
- **Génération de Documents** : MATTAR.Reporting (MigraDocReport + HtmlReport)
- **Templating** : Scriban
- **Rendu PDF** : MigraDocCore + PdfSharpCore
- **Styling** : CSS personnalisé

### Structure du Projet

```
MattarReportBlazor/
├── Pages/
│   ├── Home.razor              # Page principale avec interface
│   ├── Home.razor.css          # Styles de la page
│   └── ...
├── Services/
│   ├── ReportGeneratorService.cs    # Service de génération
│   └── TemplateService.cs           # Service de gestion des templates
├── Models/
│   └── TemplateModel.cs        # Modèles de données
├── Layout/
│   └── MainLayout.razor        # Layout principal
├── wwwroot/
│   ├── index.html              # Point d'entrée HTML
│   └── css/
└── MattarReportBlazor.csproj   # Configuration du projet
```

## 🚀 Installation et Démarrage

### Prérequis
- .NET 10.0 SDK ou supérieur
- Navigateur web moderne (Chrome, Firefox, Edge, Safari)

### Installation

1. **Cloner le dépôt MATTAR.Reporting** (si pas déjà fait)
```bash
gh repo clone mattar-shadi/MATTAR.Reporting
```

2. **Naviguer vers le répertoire du projet**
```bash
cd MattarReportBlazor
```

3. **Restaurer les dépendances**
```bash
dotnet restore
```

4. **Lancer le serveur de développement**
```bash
dotnet watch run
```

L'application sera accessible à `https://localhost:5001`

### Publication

Pour publier l'application en mode Release :

```bash
dotnet publish -c Release -o dist
```

Les fichiers publiés seront dans le répertoire `dist/`.

## 📖 Utilisation

### Workflow Typique

1. **Sélectionner un Template**
   - Cliquez sur l'une des cartes de template disponibles
   - Le formulaire de données se charge automatiquement

2. **Éditer les Données**
   - Modifiez les champs du formulaire
   - L'aperçu se met à jour en temps réel

3. **Exporter le Document**
   - Cliquez sur "Télécharger PDF" ou "Télécharger HTML"
   - Le document est généré et téléchargé automatiquement

### Ajouter un Nouveau Template

Pour ajouter un nouveau template, modifiez `Services/TemplateService.cs` :

```csharp
private TemplateModel GetMyCustomTemplate()
{
    return new TemplateModel
    {
        Id = "my-template",
        Name = "Mon Template",
        Description = "Description du template",
        Type = TemplateType.DDL,
        Content = @"\document { ... }",  // Contenu DDL ou HTML
        SampleData = new Dictionary<string, string>
        {
            { "Field1", "Value1" },
            { "Field2", "Value2" }
        }
    };
}
```

Puis ajoutez-le à la liste dans `GetAvailableTemplates()`.

## 🔧 Développement

### Structure du Service de Génération

Le `ReportGeneratorService` fournit deux méthodes principales :

```csharp
// Générer un PDF
byte[] pdfBytes = await reportGeneratorService.GeneratePdfReportAsync(
    templateContent,
    data,
    tables);

// Générer du HTML
string html = await reportGeneratorService.GenerateHtmlReportAsync(
    templateContent,
    data,
    tables);
```

### Format des Données

Les données sont passées sous forme de dictionnaire :

```csharp
var data = new Dictionary<string, string?>
{
    { "InvoiceNumber", "INV-2024-001" },
    { "CustomerName", "Acme Corp" },
    { "Total", "1,500.00" }
};
```

Les tableaux sont passés comme `Dictionary<string, IEnumerable<Dictionary<string, string?>>>` :

```csharp
var tables = new Dictionary<string, IEnumerable<Dictionary<string, string?>>>
{
    {
        "Items",
        new List<Dictionary<string, string?>>
        {
            new Dictionary<string, string?> { { "Description", "Service" }, { "Amount", "100" } }
        }
    }
};
```

## 🎨 Personnalisation

### Modifier les Couleurs

Les couleurs principales sont définies dans `Pages/Home.razor.css` :

```css
.hero-section {
    background: linear-gradient(135deg, #2563EB 0%, #1E40AF 100%);
}

.btn-primary {
    background-color: #2563EB;
}
```

### Modifier les Styles

Tous les styles sont dans `Pages/Home.razor.css`. Modifiez les classes CSS pour personnaliser l'apparence.

## 📝 Format des Templates

### MigraDoc DDL (pour PDF)

```ddl
\document
{
  \section
  {
    \heading1 { Titre: {{ Title }} }
    \paragraph { Contenu: {{ Content }} }
    \table
    {
      \row { \cell { Header1 } \cell { Header2 } }
      {{ for item in Items }}
      \row { \cell { {{ item.Field1 }} } \cell { {{ item.Field2 }} } }
      {{ end }}
    }
  }
}
```

### HTML (pour HTML/PDF)

```html
<h1>{{ Title }}</h1>
<p>{{ Content }}</p>
<table>
  <tr><th>Header1</th><th>Header2</th></tr>
  {{ for item in Items }}
  <tr><td>{{ item.Field1 }}</td><td>{{ item.Field2 }}</td></tr>
  {{ end }}
</table>
```

## ⚠️ Limitations et Considérations

- **Taille des fichiers** : Les fichiers PDF générés sont stockés en mémoire
- **Performance** : La génération de PDF volumineux peut être lente côté client
- **Navigateur** : Nécessite un navigateur moderne avec support WebAssembly
- **Sécurité** : Aucune donnée n'est envoyée à un serveur (tout est côté client)

## 🔐 Sécurité

- ✅ Aucune donnée n'est transmise à un serveur
- ✅ Tout le traitement se fait côté client
- ✅ Les documents ne sont jamais stockés
- ✅ Exécution dans un environnement WebAssembly isolé

## 📦 Dépendances Principales

- `Microsoft.AspNetCore.Components.WebAssembly` - Framework Blazor WASM
- `MATTAR.Reporting` - Génération de documents
- `Scriban` - Templating
- `MigraDocCore` - Génération PDF
- `PdfSharpCore` - Manipulation PDF

## 🐛 Dépannage

### L'aperçu ne s'affiche pas
- Vérifiez la syntaxe du template (DDL ou HTML)
- Vérifiez que les noms de variables correspondent aux données

### Le PDF ne se télécharge pas
- Vérifiez la console du navigateur pour les erreurs
- Assurez-vous que les données requises sont présentes

### L'application est lente
- Les fichiers PDF volumineux peuvent être lents à générer côté client
- Essayez avec un template plus simple d'abord

## 📚 Ressources

- [Documentation Blazor WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly)
- [MATTAR.Reporting GitHub](https://github.com/mattar-shadi/MATTAR.Reporting)
- [Scriban Documentation](https://github.com/scriban/scriban)
- [MigraDoc Documentation](https://www.pdfsharp.net/wiki/MigraDoc/Overview.ashx)

## 📄 Licence

Ce projet utilise la bibliothèque MATTAR.Reporting qui est sous licence MIT.

## 🤝 Contribution

Pour contribuer à ce projet :

1. Forkez le dépôt
2. Créez une branche pour votre fonctionnalité
3. Commitez vos changements
4. Poussez vers la branche
5. Ouvrez une Pull Request

## 📞 Support

Pour les problèmes ou questions :
- Consultez la [documentation MATTAR.Reporting](https://github.com/mattar-shadi/MATTAR.Reporting)
- Ouvrez une issue sur GitHub
- Consultez les logs du navigateur pour les erreurs

---

**Créé avec ❤️ en utilisant Blazor WebAssembly et MATTAR.Reporting**
