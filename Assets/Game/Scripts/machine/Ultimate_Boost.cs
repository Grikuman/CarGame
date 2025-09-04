using UnityEngine;

public class Ultimate_Boost : IUltimate
{
    private float m_ultimateTime = 3.0f;
    private float m_boostMultiplier = 3.0f;
    private float m_timer;
    private bool m_isActive;
    private bool m_end = false;
    private HoverCarController car;
    public void Activate(HoverCarController car)
    {
        // コントローラーを設定する
        this.car = car;
        // 加速倍率を設定する
        car.boostMultiplier = m_boostMultiplier;
        // アルティメットの効果時間を設定する
        m_timer = m_ultimateTime;
        // アルティメット発動状態
        m_isActive = true;
    }

    public void Update()
    {
        if (!m_isActive) return;

        m_timer -= Time.deltaTime;
        // アルティメット時間が終了したら
        if (m_timer <= 0.0f)
        {
            // 終了させる
            m_end = true;
        }
    }

    public void End()
    {
        // 加速倍率を戻す
        car.boostMultiplier = 1.0f;
        // アルティメット状態を解除する
        m_isActive = false;
        // 終了状態を解除する
        m_end = false;
    }

    public bool IsEnd() { return m_end; }

    public bool IsActive() { return m_isActive; }
}
