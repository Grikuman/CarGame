using UnityEngine;

public class BoostSystem : MonoBehaviour
{
    [Header("ブースト設定")]
    public float boostMultiplier = 2.0f;   // 倍率
    public float maxBoostGauge = 100f;     // 最大ゲージ量
    public float gaugeConsumptionRate = 30f; // 1秒あたり消費量
    public float gaugeRecoveryRate = 20f;    // 1秒あたり回復量
    public float boostCooldown = 3.0f;       // ブースト後のクールダウン

    [SerializeField ]private float currentGauge;            // 現在のゲージ
    private float cooldownTimer = 0.0f;    // クールダウン残り時間
    private bool isBoosting = false;

    // コンポーネント
    private HoverCarController car;
    private UltimateSystem m_ultimateSystem;

    void Start()
    {
        car = GetComponent<HoverCarController>();
        m_ultimateSystem = GetComponent<UltimateSystem>();
        currentGauge = maxBoostGauge; // 初期は満タン
    }

    void Update()
    {
        if (isBoosting)
        {
            // ゲージを消費
            currentGauge -= gaugeConsumptionRate * Time.deltaTime;
            // アルティメットゲージを貯める
            m_ultimateSystem.AddUltimateGauge();

            if (currentGauge <= 0)
            {
                currentGauge = 0;
                EndBoost();
            }
        }
        else
        {
            // クールダウン中なら回復しない
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                // ゲージを回復
                if (currentGauge < maxBoostGauge)
                {
                    currentGauge += gaugeRecoveryRate * Time.deltaTime;
                    currentGauge = Mathf.Clamp(currentGauge, 0, maxBoostGauge);
                }
            }
        }
    }

    // 外部から呼ぶ（入力など）
    public void TryStartBoost()
    {
        if (!isBoosting && currentGauge > 0 && cooldownTimer <= 0)
        {
            StartBoost();
        }
    }

    private void StartBoost()
    {
        car.boostMultiplier = boostMultiplier;
        isBoosting = true;
    }

    private void EndBoost()
    {
        car.boostMultiplier = 1.0f;
        isBoosting = false;
        cooldownTimer = boostCooldown; // 終了時にクールダウン
    }

    // UI用：現在のゲージを割合で返す
    public float GetBoostGaugeNormalized()
    {
        return currentGauge / maxBoostGauge;
    }
}
