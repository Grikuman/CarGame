// ArcadeKart に追加するブーストシステム（ブーストゲージ + モード制御）
using UnityEngine;


public class KartBoostController : MonoBehaviour
{
    [Header("ブーストゲージ設定")]
    public float maxBoost = 100f;                    // 最大ゲージ量
    public float boostGauge = 0f;                  // 現在のゲージ量
    public float boostDrainRate = 10f;               // 使用中のゲージ減少量/秒
    public float boostChargeRate = 2f;               // 自然回復（順位によって変化）
    public float drivingChargeRate = 2f;             // 走行中に増加する量/秒
    public float boostItemCharge = 25f;              // ブースト玉1個取得で増える量

    [Header("ブーストモード設定")]
    public float boostSpeed = 50f;                   // ブースト中のスピード上限
    public float boostAcceleration = 20f;            // ブースト中の加速度

    [Header("システム")]
    public bool isBoosting = false;                  // ブーストモード中か？
    public KeyCode boostKey = KeyCode.Space;         // ブースト発動キー
    public GameObject boostVFX;                      // ブースト時エフェクト（任意）

   
    private float originalTopSpeed;                  // 元のスピード（戻す用）
    private float originalAcceleration;              // 元の加速度（戻す用）

    void Start()
    {

        if (boostVFX) boostVFX.SetActive(false);     // エフェクト初期状態で非表示
    }

    void Update()
    {
        HandleBoostInput();     // 入力チェック＆処理
        RecoverBoostGauge();    // ゲージ回復処理
    }

    // ブースト入力処理
    void HandleBoostInput()
    {
        if (Input.GetKey(boostKey) && boostGauge > 0f)
        {
            if (!isBoosting)
            {
                StartBoost();   // ブースト開始（ステータス書き換え）
            }
            ApplyBoost();       // ゲージを減らし続ける
        }
        else
        {
            if (isBoosting)
            {
                EndBoost();     // ブースト終了（ステータス戻す）
            }
        }
    }

    // ブースト開始（速度・加速度を上書き）
    void StartBoost()
    {
        isBoosting = true;
        
        if (boostVFX) boostVFX.SetActive(true);  // エフェクト表示
    }

    // ブースト継続中の処理（ゲージ消費）
    void ApplyBoost()
    {
        boostGauge -= boostDrainRate * Time.deltaTime;
        boostGauge = Mathf.Clamp(boostGauge, 0f, maxBoost);
    }

    // ブースト終了（ステータスを元に戻す）
    void EndBoost()
    {
        isBoosting = false;
        
        if (boostVFX) boostVFX.SetActive(false); // エフェクト非表示
    }

    // ブーストゲージの自然回復処理
    void RecoverBoostGauge()
    {
        float rankMultiplier = 1f; // 順位に応じて変化させる（後で追加）

        // 走行中（速度が一定以上）のとき、追加回復
       

        // 通常の自然回復（順位による補正をかける）
        boostGauge += boostChargeRate * rankMultiplier * Time.deltaTime;
        boostGauge = Mathf.Clamp(boostGauge, 0f, maxBoost);
    }

    // ブースト玉（タグ付きオブジェクト）取得処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item_Boost"))
        {
            boostGauge += boostItemCharge;
            boostGauge = Mathf.Clamp(boostGauge, 0f, maxBoost);
        }
    }

    // UI用：現在のゲージ割合（0〜1）
    public float GetBoostPercent() => boostGauge / maxBoost;

    public bool GetIsBoost()
    {
        return isBoosting;
    }
}
