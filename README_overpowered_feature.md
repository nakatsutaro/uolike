# uolike: overpowered enemies and map spawner

This change adds a system where if the player becomes "overpowered", nearby enemies will coordinate and gang up on the player. It also adds a map spawner that places buildings (indestructible), trees (destructible), and chickens (destructible, moving) randomly in the scene.

Files added (Assets/Scripts/Game/)
- IDamageable.cs
- Health.cs
- PlayerStats.cs
- PlayerPowerMonitor.cs
- EnemyController.cs
- MapSpawner.cs
- Tree.cs
- Chicken.cs
- Indestructible.cs

Setup / Usage
1. NavMesh: Bake a NavMesh for your scene (Window > AI > Navigation) so NavMeshAgent-based enemies and chickens can navigate.
2. Player:
   - Tag the player GameObject with "Player".
   - Attach PlayerStats and Health components.
3. Enemy prefab:
   - Add NavMeshAgent, Collider, EnemyController, and Health.
   - Configure EnemyController.basePower and other parameters as desired.
4. Building prefabs:
   - Add a Collider and attach the Indestructible component. Tag as "Building" (optional).
5. Tree prefabs:
   - Add Collider and Tree component (or Health implementing IDamageable). Tag as "Tree".
6. Chicken prefab:
   - Add NavMeshAgent, Collider and Chicken component. Tag as "Chicken".
7. Scene:
   - Add a MapSpawner GameObject and assign prefab arrays and area bounds. Set obstacleMask if you want to prevent placement on specific layers.
   - Add a PlayerPowerMonitor and link the playerStats (or leave threshold=0 to auto-calc from enemies).

Tuning parameters
- PlayerPowerMonitor.autoMultiplier: multiplies average enemy basePower to determine the auto threshold.
- EnemyController.callRadius: how far enemies will call nearby allies.
- MapSpawner.placementRadius: minimum spacing between spawned objects.
- Counts for each type in MapSpawner.

Notes
- This commit creates scripts and a README. It does not add binary assets (models/textures) — please assign your existing prefabs to the MapSpawner fields in the Inspector.
- After pulling, run the scene and test by adjusting PlayerStats to trigger the overpowered state.

If you want, I can open a Pull Request description with testing steps and screenshots (if you provide them).