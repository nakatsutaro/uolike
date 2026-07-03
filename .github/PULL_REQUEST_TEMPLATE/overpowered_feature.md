# Pull Request: Overpowered enemies and map spawner

This PR introduces systems for: if the player becomes overpowered, enemies will coordinate and gang up on the player; and a MapSpawner which places buildings (indestructible), trees (destructible), and chickens (destructible and moving) randomly in the scene.

What I changed
- Added new scripts in Assets/Scripts/Game/:
  - IDamageable.cs, Health.cs, PlayerStats.cs, PlayerPowerMonitor.cs, EnemyController.cs, MapSpawner.cs, Tree.cs, Chicken.cs, Indestructible.cs
- Added README_overpowered_feature.md with setup and tuning instructions

How to test
1. Pull this branch and open the project in Unity.
2. Bake NavMesh for the scene.
3. Set up the player GameObject with Tag="Player" and attach PlayerStats and Health.
4. Prepare enemy prefab with NavMeshAgent, Collider, EnemyController and Health; place a few enemies in the scene.
5. Create building/tree/chicken prefabs or use placeholders, and assign them in MapSpawner in the scene. Configure area bounds and counts.
6. Play the scene. Increase PlayerStats (e.g., level or baseAttack) until the PlayerPowerMonitor logs "overpowered=true". Enemies should call nearby allies and pursue the player.

Tuning
- PlayerPowerMonitor.overpoweredThreshold (or auto when set to 0)
- PlayerPowerMonitor.autoMultiplier (default 1.5)
- EnemyController.callRadius, damagePerSecond, pursueSpeed, calmDuration
- MapSpawner counts and placementRadius

Notes
- No binary assets included. Assign your existing prefabs to MapSpawner.
- Further refinements (formation, leader AI, balancing) can be implemented after playtesting.

