# A Quest to Eternity — Notes de version 1.0.0

## Présentation

*A Quest to Eternity* est un jeu d’action et d’exploration spatiale en 3D. Le joueur pilote un vaisseau dans le système solaire, collecte et détruit des astéroïdes, puis atterrit sur Mercure et Vénus afin d’accomplir des missions de colonisation et de sécurisation.

## Mécaniques principales

- Pilotage libre du vaisseau avec déplacements verticaux.
- Verrouillage et sélection des planètes ou des astéroïdes.
- Collecte d’astéroïdes et destruction au laser.
- Exploration au sol en vue FPS ou TPS.
- Combat contre des lézards et des golems disposant d’une IA, d’animations et de points de vie.
- Système de quêtes avec objectifs, progression et prérequis.
- Activation de balises pour coloniser les planètes.
- Barre de vie, mort du personnage et écran de défaite.
- Effet de désynchronisation et réapparition lors de la sortie de la zone de jeu dans l'espace (trop proche du soleil ou assez éloigné des planètes).

## Contrôles

### À pied

- `ZQSD` : se déplacer.
- Souris : orienter la caméra.
- `Maj gauche` : sprinter.
- `Espace` : sauter.
- `1` : sortir ou ranger l’arme.
- `2` : alterner entre les vues FPS et TPS.
- Clic gauche : tirer lorsque l’arme est équipée.
- `E` : interagir avec une balise ou une sortie.

### Dans le vaisseau

- `Z/S` : avancer ou reculer.
- `Q/D` : tourner.
- `Espace` / `Ctrl gauche` : monter ou descendre.
- `F` : Quitter le siège du pilote pour se balader dans le vaisseau ou reprendre les commandes du vaisseau
- `T` : cibler une planète.
- `R` : cibler un astéroïde.
- Flèches gauche/droite : changer de cible.
- Clic gauche : tirer.
- `E` : collecter un astéroïde proche ou atterrir.

## Règles et objectifs

Pour débloquer Vénus, il faut collecter 15 astéroïdes et en détruire 20. Pour débloquer Mercure, il faut collecter 10 astéroïdes et en détruire 20. 

Une fois au sol, chaque planète doit être sécurisée en éliminant 20 lézards et 5 golems. Le joueur doit ensuite activer la balise de la planète pour terminer sa colonisation. 
Les missions sont débloquées progressivement selon leurs prérequis.

La partie est perdue lorsque les points de vie du personnage atteignent zéro.

## Limitations, bugs connus et améliorations envisagées

- Seules Mercure et Vénus proposent actuellement des missions complètes.
- Le jeu est principalement prévu pour clavier et souris.
- La progression n’est pas sauvegardée entre deux lancements.
- L’IA peut encore rencontrer des difficultés de navigation sur certains reliefs.
- L’équilibrage des combats, du nombre d’ennemis et des objectifs peut être affiné.
- Des tutoriels, davantage de planètes, de missions, d’ennemis et de variantes de vaisseaux pourraient être ajoutés.
- Les paramètres Unity indiquent encore le nom technique « My project » et la version `0.1.0`. Ils devront être alignés sur *A Quest to Eternity — 1.0.0* avant la compilation finale.

## Répartition de la production

- **Lucas Pokrywa** : système de quêtes et de prérequis, interface des missions, conception et intégration des scènes de Mercure et Vénus, terrains, balises, transitions entre les scènes, menu principal, équilibrage et intégration générale.
- **Kadir Ersoy** : système solaire, pilotage du vaisseau, ceinture d’astéroïdes, collecte et destruction des astéroïdes, ciblage et tir spatial, collisions avec les planètes, désynchronisation et réapparition, interface des commandes et intégration musicale.
- **Valentin Hodonou** : personnage jouable, points de vie et combat au sol, ennemis, IA et navigation NavMesh, animations des personnages et intégration des effets sonores des monstres, intérieur du vaissseau et cinématique de victoire.

## Assets réalisés en propre

D’après l’organisation du projet, les éléments suivants sont des productions internes :

- Tous les scripts de gameplay propres au projet.
- Les systèmes de quêtes, de combat, de ciblage, de pilotage et d’interaction.
- La conception des scènes, des niveaux, des terrains et des missions.
- Les modèles `base`, `balise` et `asteroids`.
- Le logo du jeu et les éléments d’interface spécifiques.
- Les matériaux et prefabs créés pour assembler le jeu.
- Les morceaux nommés `Unity_Projet`, `Unity_Projet2`, `Unity_Projet3` et `Unity_Projet4`, sous réserve de confirmation par l’équipe.

## Assets récupérés sur Internet

Le projet contient notamment les packs externes suivants :

- Dark Astronaut.
- F3 Corvette.
- Free Sci-Fi Drone.
- Free Skyboxes – Space.
- Hatogame Lizard.
- Siuniaev Characters – Golem.
- Sci-Fi Modular Pack / 3D Sci-Fi Kit.
- Planets of the Solar System 3D.
- Procedural Planet Generation de Parallel Cascades.
- All In One – Heightmaps.
- MicroVerse Extras.
- TextMesh Pro.
- Modèles de blaster et de vaisseaux additionnels.
- Police Orbitron.

## Assets audio récupérés sur Internet

- `Gun2_1.wav` — « Laser Gun Sound », par Mikael Vanninem.
  Source : Unity Asset Store.
  Licence : Unity Asset Store EULA.
  Modifications : Volume réduit.

- `25. Warning Growl.wav` - « Warning Growl », par *VoiceBosch*.
  Source : Unity Asset Store.
  Licence : Unity Asset Store EULA.
  Modifications : aucune.

- `02. Ferocious Roar.wav` - « Ferocious Roar », par *VoiceBosch*.
  Source : Unity Asset Store.
  Licence : Unity Asset Store EULA.
  Modifications : aucune.
  
  
## Assets audio fournis par un tiers

- `piano train` et `dessin animé` : créé et fourni par Dylan Marquses, utilisé dans *A Quest to Eternity* avec son autorisation. Tous les droits restent réservés à son auteur.

## Assets audio créé par nous même

- `Unity_Projet` : Créé et fourni par Valentin Hodonou, utilisé dans *A Quest to Eternity*. Tous les droits restent réservés à son auteur.
  
## Assets audio réalisés en propre

- `roars`, `growl` et `Gun2_1` : effets sonores créés par Valentin Hodonou.
- `targetLockSound.wav` : effet sonore créé par Kadir Ersoy.

© 2026 Lucas Pokrywa, Valentin Hodonou et Kadir Ersoy. Tous droits réservés.

Toute extraction, copie, modification, diffusion, redistribution ou réutilisation de ces musiques et effets sonores, séparément du jeu ou dans un autre projet, est interdite sans l’autorisation écrite préalable de leur auteur respectif, sauf exceptions prévues par la loi.