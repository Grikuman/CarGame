using UnityEngine;

public class UltimateController : MonoBehaviour
{
    // アルティメットの種類
    public enum UltimateType
    {
        Blitzray,
        Parasite,
        Empress
    }

    [Header("アルティメットの種類を設定")]
    [SerializeField] private UltimateType m_ultType = UltimateType.Blitzray;

    [Header("アルティメットゲージ関連")]
    [SerializeField] private float m_ultGauge = 0.0f; 　　　// 現在のゲージ
    [SerializeField] private float m_maxGauge = 100.0f;     // ゲージ最大値
    [SerializeField] private float m_gaugeGainRate = 10.0f; // ブースト中のゲージ貯まりやすさ

    [Header("状態")]
    [SerializeField] private bool m_canActivateUltimate = false; // アルティメットを発動可能か
    [SerializeField] private bool m_activateUltimate = false;  // アルティメット使用状況
    // ブーストスクリプトへの参照
    private KartBoostController m_boost;

    void Start()
    {
        // Boostクラスを取得
        m_boost = GetComponent<KartBoostController>();
    }

    void Update()
    {
        // BoostのGetUseBoost()がtrueのときゲージをためる
        if (m_boost != null && m_boost.GetIsBoost() && m_ultGauge < m_maxGauge)
        {
            // アルティメットゲージに加算する
            m_ultGauge += m_gaugeGainRate * Time.deltaTime;

            // アルティメットゲージが100を超えないように制限する
            // 超えた段階でアルティメット発動可能状態にする
            if (m_ultGauge >= m_maxGauge)
            {
                m_ultGauge = m_maxGauge;
                m_canActivateUltimate = true;
            }
        }

        // アルティメット発動可能状態のときに発動入力があれば
        if (m_canActivateUltimate && Input.GetKeyDown(KeyCode.E))
        {
            // アルティメット発動
            ActivateUltimate();
        }
        // アルティメット発動中ならば各処理を行う
        if(m_activateUltimate)
        {
            // アルティメットの処理を行う
            Ultimate();
        }
    }

    // アルティメットを発動する
    void ActivateUltimate()
    {
        // アルティメット使用状況
        m_activateUltimate = true;
    }

    // アルティメットの処理を行う
    void Ultimate()
    {
        // 設定されている種類のアルティメットを処理する
        switch (m_ultType)
        {
            case UltimateType.Blitzray:
                Debug.Log("Blitzray 発動：超加速");
                // TODO: 加速処理

                break;

            case UltimateType.Parasite:
                Debug.Log("Parasite 発動：ブースト吸収");
                // TODO: 周囲の敵のブーストゲージを減らして自分に加算

                break;

            case UltimateType.Empress:
                Debug.Log("EMPRESS 発動：スタン");
                // TODO: 一定範囲内の敵をスタン状態にする

                break;
        }

        // アルティメット使用後はリセットする
        //m_ultGauge = 0.0f;
        //m_canUseUltimate = false;
    }


    // 現在のアルティメットゲージ割合を取得する
    public float GetGaugePercent()
    {
        return m_ultGauge / m_maxGauge;
    }
}
