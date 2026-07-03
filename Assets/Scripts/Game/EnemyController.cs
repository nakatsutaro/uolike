using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public float basePower = 20f; // 敵の基礎強さ（PlayerPowerMonitor の自動閾値に使う）
    public float callRadius = 12f;     // 近傍の敵を呼ぶ距離
    public float attackRange = 1.6f;
    public float damagePerSecond = 10f;
    public float pursueSpeed = 3.5f;
    public float patrolSpeed = 1.8f;
    public float calmDuration = 10f; // プレイヤー過強解除後に落ち着くまでの猶予

    NavMeshAgent agent;
    Transform player;
    bool isAggressive = false;
    float calmTimer = 0f;

    static List<EnemyController> allEnemies = new List<EnemyController>();

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        allEnemies.Add(this);
    }
    void OnDestroy() => allEnemies.Remove(this);

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        PlayerPowerMonitor.OnOverpoweredChanged += OnPlayerPowerChanged;
        // 初期パトロール速度
        if (agent != null) agent.speed = patrolSpeed;
    }

    void OnPlayerPowerChanged(bool overpowered)
    {
        if (overpowered) BecomeAggressive();
        else StartCalmDown();
    }

    void BecomeAggressive()
    {
        if (isAggressive) return;
        isAggressive = true;
        if (agent != null) agent.speed = pursueSpeed;

        // 近傍の味方に参加を通知（包囲や群攻を誘発）
        foreach (var e in allEnemies)
        {
            if (e == this) continue;
            if ((e.transform.position - transform.position).sqrMagnitude <= callRadius * callRadius)
            {
                e.ReceiveGroupAttack(player);
            }
        }

        if (player != null)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        StopAllCoroutines();
        StartCoroutine(AggressiveBehavior());
    }

    // 直接呼び出される（leader の Transform を渡す）
    public void ReceiveGroupAttack(Transform leader)
    {
        if (isAggressive) return;
        isAggressive = true;
        player = leader;
        if (agent != null) agent.speed = pursueSpeed;
        StopAllCoroutines();
        StartCoroutine(AggressiveBehavior());
    }

    IEnumerator AggressiveBehavior()
    {
        while (isAggressive)
        {
            if (player != null)
            {
                agent.SetDestination(player.position);
                float dist = Vector3.Distance(transform.position, player.position);
                if (dist <= attackRange)
                {
                    var hp = player.GetComponent<Health>();
                    if (hp != null)
                    {
                        hp.TakeDamage(damagePerSecond * Time.deltaTime, gameObject);
                    }
                }
            }
            yield return null;
        }
    }

    void StartCalmDown()
    {
        // すぐに即時沈静せず、少し時間を置いて減衰させる
        calmTimer = calmDuration;
        if (isAggressive)
        {
            StartCoroutine(CalmDownCoroutine());
        }
    }

    IEnumerator CalmDownCoroutine()
    {
        while (calmTimer > 0f)
        {
            calmTimer -= Time.deltaTime;
            yield return null;
        }
        CalmDown();
    }

    void CalmDown()
    {
        isAggressive = false;
        if (agent != null)
        {
            agent.ResetPath();
            agent.speed = patrolSpeed;
        }
        StopAllCoroutines();
        // ここで元のパトロール等に戻す処理を入れてください。
    }

    // ユーティリティ: 敵の平均 basePower を取れるようにして PlayerPowerMonitor が利用可能にする
    public static float GetAverageEnemyBasePower()
    {
        if (allEnemies.Count == 0) return 0f;
        float sum = 0f;
        foreach (var e in allEnemies) sum += e.basePower;
        return sum / allEnemies.Count;
    }
}
