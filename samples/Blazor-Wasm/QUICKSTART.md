# Guide de Démarrage Rapide - MATTAR.Report Blazor WASM

## 🎯 En 5 minutes

### 1. Prérequis
- .NET 10.0 SDK ([Télécharger](https://dotnet.microsoft.com/download))
- Navigateur web moderne

### 2. Cloner et Démarrer

```bash
# Cloner le dépôt MATTAR.Reporting (si nécessaire)
gh repo clone mattar-shadi/MATTAR.Reporting

# Naviguer vers le projet
cd MattarReportBlazor

# Lancer le serveur de développement
dotnet watch run
```

L'application s'ouvrira automatiquement à `https://localhost:5001`

### 3. Utiliser l'Application

1. **Sélectionner un Template**
   - Cliquez sur "Facture", "Rapport Mensuel" ou "Certificat"

2. **Éditer les Données**
   - Modifiez les champs dans le formulaire
   - L'aperçu se met à jour en temps réel

3. **Exporter**
   - Cliquez "Télécharger PDF" ou "Télécharger HTML"
   - Le fichier se télécharge automatiquement

## 📁 Structure du Projet

```
MattarReportBlazor/
├── Pages/
│   ├── Home.razor              ← Interface principale
│   └── Home.razor.css          ← Styles
├── Services/
│   ├── ReportGeneratorService.cs    ← Génération
│   └── TemplateService.cs           ← Templates
├── Models/
│   └── TemplateModel.cs        ← Modèles
├── Program.cs                  ← Configuration
└── README.md                   ← Documentation complète
```

## 🔧 Commandes Utiles

```bash
# Lancer en développement avec rechargement automatique
dotnet watch run

# Compiler uniquement
dotnet build

# Publier en Release
dotnet publish -c Release -o dist

# Nettoyer les fichiers générés
dotnet clean
```

## 📝 Ajouter un Template Personnalisé

Modifiez `Services/TemplateService.cs` :

```csharp
private TemplateModel GetMyTemplate()
{
    return new TemplateModel
    {
        Id = "my-template",
        Name = "Mon Template",
        Description = "Description",
        Type = TemplateType.DDL,
        Content = @"\document { \section { \heading1 { {{ Title }} } } }",
        SampleData = new Dictionary<string, string>
        {
            { "Title", "Exemple" }
        }
    };
}
```

Puis ajoutez à `GetAvailableTemplates()` :

```csharp
return new List<TemplateModel>
{
    // ... templates existants
    GetMyTemplate()  // ← Ajouter ici
};
```

## 🎨 Personnaliser l'Apparence

Modifiez `Pages/Home.razor.css` pour changer :
- Couleurs (`.hero-section`, `.btn-primary`)
- Polices et tailles
- Espacements et marges
- Animations

## 🚀 Déployer

### Sur Netlify
```bash
# Publier
dotnet publish -c Release -o dist

# Déployer le contenu de dist/wwwroot
```

### Sur GitHub Pages
```bash
# Publier avec base path
dotnet publish -c Release -o dist /p:GithubPages=true
```

## ⚠️ Dépannage

| Problème | Solution |
|----------|----------|
| Erreur de compilation | `dotnet clean && dotnet restore && dotnet build` |
| Port 5001 occupé | Modifier dans `launchSettings.json` |
| Aperçu vide | Vérifier la syntaxe du template DDL/HTML |
| PDF ne se télécharge pas | Vérifier les logs du navigateur (F12) |

## 📚 Ressources

- [Documentation Blazor](https://learn.microsoft.com/aspnet/core/blazor)
- [Syntaxe MigraDoc DDL](https://www.pdfsharp.net/wiki/MigraDoc/Overview.ashx)
- [Scriban Templating](https://github.com/scriban/scriban)
- [MATTAR.Reporting](https://github.com/mattar-shadi/MATTAR.Reporting)

## 💡 Conseils

- **Commencer simple** : Testez d'abord avec le template Facture
- **Aperçu en temps réel** : Utilisez-le pour déboguer les templates
- **Réutiliser les données** : Les données sont conservées entre les templates
- **Exporter en HTML d'abord** : Plus rapide pour tester

## 🎓 Prochaines Étapes

1. Explorez les templates existants
2. Créez votre propre template
3. Ajoutez des images aux templates
4. Personnalisez les styles
5. Déployez votre application

---

**Besoin d'aide ?** Consultez le [README complet](README.md)
