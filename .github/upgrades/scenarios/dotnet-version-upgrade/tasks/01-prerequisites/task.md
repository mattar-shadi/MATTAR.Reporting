# 01-prerequisites: Vérification des prérequis SDK

Vérifier que le SDK .NET 11 est installé et disponible, et que les fichiers `global.json` éventuels sont compatibles avec la cible net11.0.

La solution utilise .NET 10 comme base. Avant toute modification des projets, il faut s'assurer que l'environnement de développement dispose du SDK .NET 11 Preview et que son utilisation n'est pas bloquée par un `global.json` restrictif.

**Done when**: Le SDK net11.0 est confirmé installé et aucun `global.json` ne bloque son utilisation.
