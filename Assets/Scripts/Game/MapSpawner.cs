using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] buildingPrefabs; // 破壊不可（タグ: Building）
    public GameObject[] treePrefabs;     // 破壊可（タグ: Tree）
    public GameObject chickenPrefab;     // 移動する破壊可（タグ: Chicken）

    [Header("Counts")]
    public int buildingCount = 12;
    public int treeCount = 40;
    public int chickenCount = 10;

    [Header("Area (XZ plane)")]
    public Vector2 areaMin = new Vector2(-50, -50);
    public Vector2 areaMax = new Vector2(50, 50);

    [Header("Placement")]
    public float placementRadius = 2f; // オブジェクト間最小距離
    public LayerMask obstacleMask; // 既存オブジェクトを避けるためのレイヤーマスク

    void Start()
    {
        SpawnMany(buildingPrefabs, buildingCount, "Building");
        SpawnMany(treePrefabs, treeCount, "Tree");
        SpawnMany(new GameObject[] { chickenPrefab }, chickenCount, "Chicken", true);
    }

    void SpawnMany(GameObject[] prefabs, int count, string tag, bool isMoving=false)
    {
        if (prefabs == null || prefabs.Length == 0 || count <= 0) return;
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = count * 30;
        while (spawned < count && attempts < maxAttempts)
        {
            attempts++;
            Vector3 pos = new Vector3(
                Random.Range(areaMin.x, areaMax.x),
                0f,
                Random.Range(areaMin.y, areaMax.y)
            );

            // Raycast down to terrain or adjust Y here if needed. For flat plane we keep y=0
            if (!IsClear(pos, placementRadius)) continue;

            var prefab = prefabs[Random.Range(0, prefabs.Length)];
            var rot = Quaternion.Euler(0f, Random.Range(0,360), 0f);
            var go = Instantiate(prefab, pos, rot, transform);
            go.tag = tag;

            // Tag-based conventions: trees and chickens should have Health component attached
            spawned++;
        }
    }

    bool IsClear(Vector3 pos, float radius)
    {
        Collider[] cols = Physics.OverlapSphere(pos, radius, ~0, QueryTriggerInteraction.Ignore);
        // Check if any collider belongs to existing spawn area; allow placement on terrain collider only if desired.
        foreach (var c in cols)
        {
            // Ignore Terrain or ground if needed: you can filter by layer
            if (((1 << c.gameObject.layer) & obstacleMask) != 0)
            {
                return false;
            }
        }
        return true;
    }
}
