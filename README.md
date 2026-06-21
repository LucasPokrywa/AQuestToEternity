# AQuestToEternity

## Présentation

**A Quest to Eternity** est un jeu d’action et d’exploration spatiale en 3D réalisé avec Unity. Le joueur explore le système solaire à bord d’un vaisseau, collecte et détruit des astéroïdes, puis atterrit sur Mercure et Vénus pour accomplir des missions de colonisation et de sécurisation.

## Release Notes

Voir les [Release Notes](RELEASE_NOTES.md).

## Fonctionnalités principales

- Pilotage d’un vaisseau spatial avec accélération et déplacements verticaux.
- Ciblage des planètes et des astéroïdes.
- Collecte d’astéroïdes et combat spatial au laser.
- Exploration au sol avec vues FPS et TPS.
- Combat contre des lézards et des golems.
- Ennemis contrôlés par une IA utilisant la navigation NavMesh.
- Système de quêtes avec objectifs et prérequis.
- Activation de balises pour coloniser les planètes.
- Barre de vie, animations, effets sonores et écran de défaite.
- Système de désynchronisation et de réapparition dans l’espace.

## Contrôles

### À pied

| Commande | Action |
| --- | --- |
| `ZQSD` | Se déplacer |
| Souris | Orienter la caméra |
| `Maj gauche` | Sprinter |
| `Espace` | Sauter |
| `1` | Sortir ou ranger l’arme |
| `2` | Alterner entre les vues FPS et TPS |
| Clic gauche | Tirer lorsque l’arme est équipée |
| `E` | Interagir avec une balise ou une sortie |

### Dans le vaisseau

| Commande | Action |
| --- | --- |
| `Z/S` | Avancer ou reculer |
| `Q/D` | Tourner |
| `Espace` / `Ctrl gauche` | Monter ou descendre |
| `Maj gauche` | Accélérer |
| `F` | Entrer dans le vaisseau, en sortir ou ouvrir son interface |
| `T` | Cibler une planète |
| `R` | Cibler un astéroïde |
| Flèches gauche/droite | Changer de cible |
| Clic gauche | Tirer |
| `E` | Collecter un astéroïde proche ou atterrir |

## Objectifs du jeu

Pour débloquer Mercure, le joueur doit collecter 10 astéroïdes et en détruire 20. Pour débloquer Vénus, il doit en collecter 15 et en détruire 20.

Une fois au sol, chaque planète doit être sécurisée en éliminant 20 lézards et 5 golems. Le joueur doit ensuite activer une balise afin de terminer la colonisation de la planète.

## Installation et lancement

Le projet utilise **Unity 6000.3.11f1**.

1. Cloner ou télécharger le dépôt.
2. Ajouter le dossier du projet dans Unity Hub.
3. Ouvrir le projet avec Unity 6000.3.11f1.
4. Ouvrir la scène `Assets/Scenes/menu.unity`.
5. Lancer le jeu avec le bouton **Play** de l’éditeur.

## Équipe et répartition

- **Lucas Pokrywa** : système de quêtes, interface des missions, scènes de Mercure et Vénus, terrains, balises, transitions, menu principal, équilibrage et intégration générale.
- **Kadir Ersoy** : système solaire, pilotage du vaisseau, astéroïdes, ciblage et combat spatial, collisions, désynchronisation, interface des commandes et intégration musicale.
- **Valentin Hodonou** : personnage jouable, combat au sol, points de vie, ennemis, IA, navigation NavMesh, animations et effets sonores des monstres.

## Assets

### Assets réalisés en propre

- Scripts et systèmes de gameplay spécifiques au projet.
- Quêtes, scènes, terrains, niveaux et interfaces propres au jeu.
- Modèles `base`, `balise` et `asteroids`.
- Logo, matériaux et prefabs créés pour le projet.

### Assets audio réalisés en propre

- `piano train` et `dessin animé` : musiques composées et produites par Lucas Pokrywa.
- `roars`, `growl` et `Gun2_1` : effets sonores créés par Valentin Hodonou.
- `targetLockSound.wav` : effet sonore créé par Kadir Ersoy.

© 2026 Lucas Pokrywa, Valentin Hodonou et Kadir Ersoy. Tous droits réservés.

Toute extraction, copie, modification, diffusion, redistribution ou réutilisation de ces musiques et effets sonores, séparément du jeu ou dans un autre projet, est interdite sans l’autorisation écrite préalable de leur auteur respectif, sauf exceptions prévues par la loi.

### Assets externes

Le projet emploie notamment des modèles, textures, animations et outils provenant de packs externes : Dark Astronaut, F3 Corvette, Free Sci-Fi Drone, Free Skyboxes – Space, Hatogame Lizard, Kevin Iglesias Humanoid Giant, Siuniaev Characters – Golem, Sci-Fi Trooper Man, Sci-Fi Modular Pack, Planets of the Solar System 3D, Parallel Cascades, All In One – Heightmaps, MicroVerse Extras, TextMesh Pro et Orbitron.

Ces éléments restent soumis aux licences et conditions d’utilisation de leurs auteurs respectifs.

