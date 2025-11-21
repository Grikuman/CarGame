using UnityEngine;

/// <summary>
/// マシンの速度に応じてスピードラインのパーティクルエミッションを制御するスクリプト。
/// スピードラインのParticleSystemと同じオブジェクト、またはそれを管理するカメラなどにアタッチします。
/// </summary>
public class SpeedLineController : MonoBehaviour
{
    [Header("参照するコンポーネント")]
    [Tooltip("制御したいスピードラインのパーティクルシステム")]
    public ParticleSystem speedLinesParticleSystem;

    [Tooltip("速度の参照元となるマシンのRigidbody")]
    public Rigidbody machineRigidbody;

    [Header("速度とエミッションの設定")]
    [Tooltip("この速度（m/s）に達したときにエミッションレートが最大になります")]
    public float maxSpeed = 50f; // 例: 50 m/s (180 km/h)

    [Tooltip("速度がゼロ、または最低速度の時のエミッションレート")]
    public float minEmissionRate = 0f;

    [Tooltip("速度がmaxSpeedに達した時の最大エミッションレート")]
    public float maxEmissionRate = 300f;

    // パフォーマンスのためにエミッションモジュールをキャッシュする変数
    private ParticleSystem.EmissionModule emissionModule;

    void Start()
    {
        // --- 起動時の初期設定とエラーチェック ---

        // 参照がInspectorから設定されているか確認
        if (speedLinesParticleSystem == null)
        {
            Debug.LogError("SpeedLinesParticleSystemが設定されていません！", this);
            enabled = false; // このスクリプトを無効化
            return;
        }

        if (machineRigidbody == null)
        {
            Debug.LogError("MachineRigidbodyが設定されていません！", this);
            enabled = false; // このスクリプトを無効化
            return;
        }

        // パーティクルシステムのエミッションモジュールを取得してキャッシュ
        // (Update内で毎回取得するとパフォーマンスが低下するため)
        emissionModule = speedLinesParticleSystem.emission;

        // 念のため、起動時は最小レートに設定
        emissionModule.rateOverTime = minEmissionRate;
    }

    void Update()
    {
        // --- 毎フレームの速度監視とエミッション制御 ---

        // 1. マシンの現在の速度（大きさ）を取得 (m/s)
        float currentSpeed = machineRigidbody.linearVelocity.magnitude;

        // 2. 速度を0.0～1.0の割合に正規化
        // (現在の速度 / 最大速度)。Clamp01で 0.0 ～ 1.0 の範囲に収める
        float speedRatio = Mathf.Clamp01(currentSpeed / maxSpeed);

        // 3. 速度の割合に応じて、エミッションレートを線形補間 (Lerp) で計算
        // (例: 割合が0.5なら、minとmaxの中間の値が設定される)
        float newEmissionRate = Mathf.Lerp(minEmissionRate, maxEmissionRate, speedRatio);

        // 4. 計算したレートをエミッションモジュールに適用
        emissionModule.rateOverTime = newEmissionRate;
    }
}