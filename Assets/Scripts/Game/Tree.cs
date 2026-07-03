using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Tree : MonoBehaviour, IDamageable
{
    public float maxHP = 30f;
    float hp;

    void Start() => hp = maxHP;

    public void TakeDamage(float amount, GameObject source = null)
    {
        hp -= amount;
        if (hp <= 0f) Destroy(gameObject);
    }
}
