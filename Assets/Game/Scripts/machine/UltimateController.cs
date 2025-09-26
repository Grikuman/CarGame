
using UnityEngine;

public class UltimateController : MonoBehaviour
{
    // アルティメットの種類
    public enum UltimateType
    {
        Blitzray,
        Parasite
    }

    [Header("アルティメットの種類を設定")]
    [SerializeField] private UltimateType m_ultType = UltimateType.Blitzray;

    [Header("アルティメットゲージ関連")]
    [SerializeField] private float m_ultGauge = 0.0f; 　　　 // 現在のゲージ
    [SerializeField] private float m_maxUltGauge = 100.0f;   // ゲージ最大値
    [SerializeField] private float m_gaugeGainRate = 10.0f;  // ブースト中のゲージ貯まりやすさ

    [Header("状態")]
    [SerializeField] private bool m_canActivateUltimate = false; // アルティメットを発動可能か
    [SerializeField] private bool m_activateUltimate = false;    // アルティメット使用状況

    [Header("超加速アルティメット設定")]
    [SerializeField] private float m_boostSpeed = 70.0f;
    [SerializeField] private float m_boostAcceleration = 25.0f;

    //-----------------------------------------------------------------------------------------------

    // ブーストコントローラーコンポーネント
    private KartBoostController m_boost;
    // アーケードカートコンポーネント

    // アルティメットの加速後に戻す用
    private float originalTopSpeed;     // 元の最大速度(戻す用）
    private float originalAcceleration; // 元の加速度(戻す用）

    void Start()
    {
        // KartBoostControllerコンポーネントを取得
        m_boost = GetComponent<KartBoostController>();
        // ArcadeKartコンポーネントを取得する
     
    }

    void Update()
    {
        // アルティメットを発動していないときの処理
        if (!m_activateUltimate)
        {
            // BoostのGetUseBoost()がtrueのときゲージをためる
            if (m_boost != null && m_boost.GetIsBoost() && m_ultGauge < m_maxUltGauge)
            {
                // アルティメットゲージに加算する
                m_ultGauge += m_gaugeGainRate * Time.deltaTime;
                // アルティメットゲージが最大値を超えないように制限する
                if (m_ultGauge >= m_maxUltGauge)
                {
                    // 最大値に補正する
                    m_ultGauge = m_maxUltGauge;
                    // アルティメット発動可能状態にする
                    m_canActivateUltimate = true;
                }
            }

            // アルティメット発動可能状態のときに入力があれば
            if (m_canActivateUltimate && Input.GetKeyDown(KeyCode.E))
            {
                // アルティメット発動
                ActivateUltimate();
            }
        }
        
        // アルティメットを発動しているときの処理
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
        // ゲージを消費する
        m_ultGauge -= m_gaugeGainRate *Time.deltaTime * 5;

        // 設定されている種類のアルティメットを処理する
        switch (m_ultType)
        {
            case UltimateType.Blitzray:
                Debug.Log("発動：超加速");
                // TODO: 加速処理
                Ultimate_Blitzray();
                break;

            case UltimateType.Parasite:
                Debug.Log("発動：ブースト吸収");
                // TODO: 周囲の敵のブーストゲージを減らして自分に加算

                break;
        }
    }

    // 超加速アルティメットの処理
    private void Ultimate_Blitzray()
    {
     
        // ウルトゲージが無くなった場合
        if (m_ultGauge <= 0)
        {
            // ステータスを戻す
           
            // アルティメット数値を補正する
            m_ultGauge = 0.0f;
            // 状況をリセットする
            m_canActivateUltimate = false;
            m_activateUltimate = false;
        }
    }

    // 現在のアルティメットゲージ割合を取得する
    public float GetGaugePercent()
    {
        return m_ultGauge / m_maxUltGauge;
    }
}
