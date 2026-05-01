# Upgrade Options — MATTAR.Reporting

Assessment: 5 projets, tous sur net10.0, dépendance 2 niveaux (bibliothèque centrale + applications)

## Strategy

### Upgrade Strategy
La solution compte 5 projets, tous sur net10.0, avec une profondeur de graphe de 2 niveaux et sans contrainte CI. Une mise à niveau atomique est la solution la plus efficace.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Mettre à niveau tous les projets simultanément en une seule passe atomique. Approche la plus rapide, sans surcharge de multi-ciblage. |
| Top-Down | Mettre à niveau les applications d'abord, puis les bibliothèques en multi-ciblage. Utile pour les grandes solutions ou quand CI doit rester vert. |
