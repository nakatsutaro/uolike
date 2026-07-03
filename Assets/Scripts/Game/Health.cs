using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class Health : MonoBehaviour, IDamageable
{
    public float maxHP = 100f;
    public bool destroyOnDeath = true;
    public event Action OnDie;

    float hp;

    void Awake() => hp = maxHP;

    public void TakeDamage(float amount, GameObject source = null)
    {
        if (hp <= 0f) return;
        hp -= amount;
        if (hp <= 0f)
        {
            hp = 0f;
            OnDie?.Invoke();
            if (destroyOnDeath) Destroy(gameObject);
        }
    }

    public void Heal(float amount)
    {
        hp = Mathf.Min(maxHP, hp + amount);
    }

    public float GetHP() => hp;
    public float GetMaxHP() => maxHP;
}
