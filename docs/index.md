---
title: Accueil
nav_order: 1
---

# MATTAR.Reporting

**MATTAR.Reporting** est une bibliothèque C# .NET simple et légère permettant de générer des rapports en remplissant des gabarits de documents (templates). Elle supporte la génération de **PDF** via des formulaires AcroForm et la génération **HTML** via le moteur de templates Scriban.

---

## 📦 Fonctionnalités

- ✅ Génération de rapports PDF à partir d'un template PDF contenant des champs de formulaire (AcroForm)
- ✅ Remplissage dynamique des champs via un dictionnaire `clé → valeur`
- ✅ Protection du document généré par mot de passe propriétaire
- ✅ Création automatique du dossier de sortie si inexistant
- ✅ Permissions de sécurité configurables (impression, annotations, extraction...)
- ✅ Génération de rapports depuis un template HTML avec remplacement de placeholders (via Scriban)
- ✅ Export PDF depuis un template HTML (via HtmlRenderer.PdfSharp)

---

## 🏗️ Structure du projet

```
MATTAR.Reporting/
├── src/
│   ├── Interfaces/
│   │   └── IReport.cs          # Interface commune à tous les générateurs de rapports
│   ├── PdfReport.cs            # Implémentation PDF (via PdfSharpCore)
│   ├── HtmlReport.cs           # Implémentation HTML (via Scriban + HtmlRenderer.PdfSharp)
│   └── MATTAR.Reporting.csproj # Projet de la bibliothèque
├── sample/
│   ├── Program.cs              # Exemple d'utilisation de la bibliothèque
│   ├── FACTURE.pdf             # Template PDF exemple
│   └── MATTAR.Reporting.Console.csproj
├── MATTAR.Reporting.sln
└── README.md
```

---

## 📄 Licence

Ce projet est distribué sous licence **MIT**. Voir le fichier [LICENSE](https://github.com/mattar-shadi/MATTAR.Reporting/blob/main/LICENSE) pour plus de détails.

---

## 🤝 Contribution

Les contributions sont les bienvenues ! N'hésitez pas à ouvrir une **issue** ou une **pull request** sur [GitHub](https://github.com/mattar-shadi/MATTAR.Reporting).

---

*Développé par [mattar-shadi](https://github.com/mattar-shadi)*
