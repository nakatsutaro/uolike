using System;
using UnityEngine;

// プレイヤーの強さを定期監視し、閾値を超えたらイベント発火する。
// overpoweredThreshold <= 0 のときは敵の平均 basePower から自動算出（EnemyController が登録されていることが前提）。
public class PlayerPowerMonitor : MonoBehaviour
{
    public PlayerStats playerStats;
    public float checkInterval = 1f;
    [Tooltip("プレイヤーがこの数値以上の power を持ったら過強化と見なす。0以下の場合は自動計算（既存敵の平均 * autoMultiplier）")]
    public float overpoweredThreshold = 0f;
    public float autoMultiplier = 1.5f;

    float timer = 0f;
    bool isOverpowered = false;

    public static event Action<bool> OnOverpoweredChanged;

    void Start()
    {
        if (playerStats == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerStats = p.GetComponent<PlayerStats>();
        }
        if (overpoweredThreshold <= 0f)
        {
            float avg = EnemyController.GetAverageEnemyBasePower();
            if (avg > 0f) overpoweredThreshold = avg * autoMultiplier;
            else overpoweredThreshold = 100f; // フォールバック
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = checkInterval;
            float power = playerStats != null ? playerStats.GetPower() : 0f;
            bool next = power >= overpoweredThreshold;
            if (next != isOverpowered)
            {
                isOverpowered = next;
                OnOverpoweredChanged?.Invoke(isOverpowered);
                Debug.Log($"PlayerPowerMonitor: overpowered={isOverpowered} power={power} threshold={overpoweredThreshold}");
            }
        }
    }
}
