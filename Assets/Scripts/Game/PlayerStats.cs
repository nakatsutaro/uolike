using UnityEngine;

// 単純な「強さ」計算の例 — プロジェクトに合わせて拡張してください。
public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public float baseAttack = 10f;
    public float weaponBonus = 0f;
    public float defense = 0f;

    // プレイヤーの「強さ」を返す。ゲームデザインに応じて調整すること。
    public float GetPower()
    {
        // 例: レベルスケーリング + 攻撃力寄せ
        return level * 10f + baseAttack + weaponBonus - defense * 0.5f;
    }
}
