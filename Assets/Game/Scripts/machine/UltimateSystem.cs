using UnityEngine;

public class UltimateSystem : MonoBehaviour
{
    [Header("ゲージ設定")] 
    [SerializeField] private float m_currentGauge = 0.0f; // 現在のアルティメットゲージ
    [SerializeField] private float m_maxGauge = 100.0f;   // 最大アルティメットゲージ
    [SerializeField] private float m_gaugeIncrease = 0.01f; // ゲージ増加量

    private IUltimate currentUltimate;
    private HoverCarController car;

    void Start()
    {
        // コンポーネントを取得する
        car = GetComponent<HoverCarController>();

        // とりあえずBoost Ultimateを設定しておく
        currentUltimate = new Ultimate_Boost();
    }

    void Update()
    {
        // アルティメット発動中なら更新
        if (currentUltimate.IsActive())
        {
            currentUltimate.Update();
            // アルティメットが終了したら
            if (currentUltimate.IsEnd())
            {
                // アルティメット終了処理を行う
                currentUltimate.End();
                // ゲージをリセットする
                m_currentGauge = 0.0f;
            }
        }

        // ゲージ値を補正する
        if(m_currentGauge >= m_maxGauge)
        {
            m_currentGauge = m_maxGauge;
        }
    }

    public void TryActivateUltimate()
    {
        // ゲージが貯まっている　かつ　アルティメットが発動されていない場合
        if (m_currentGauge >= m_maxGauge && !currentUltimate.IsActive())
        {
            //currentUltimate.Activate(car);
        }
    }

    public void AddUltimateGauge()
    {
        m_currentGauge += m_gaugeIncrease;
    }

    public void SetUltimate(IUltimate ultimate)
    {
        currentUltimate = ultimate;
    }

    public bool IsActiveUltimate()
    {
        if (currentUltimate.IsActive())
        {  
            return true;
        }
        else
        {
            return false;
        }
    }

    // UI用：現在のゲージを割合で返す
    public float GetUltimateGaugeNormalized()
    {
        return m_currentGauge / m_maxGauge;
    }
}
