using UnityEngine;

public class MachineBoostController : MonoBehaviour
{
    [Header("ブースト設定")]
    [SerializeField] private float _boostMultiplier = 1.5f;       // 倍率
    [SerializeField] private float _maxBoostGauge = 100.0f;       // 最大ゲージ量
    [SerializeField] private float _gaugeConsumptionRate = 20.0f; // 1秒あたり消費量
    [SerializeField] private float _gaugeRecoveryRate = 20.0f;    // 1秒あたり回復量
    [SerializeField] private float _boostCooldown = 3.0f;         // ブースト後のクールダウン

    [SerializeField] private float _currentGauge;         // 現在のゲージ
    [SerializeField] private float _cooldownTimer = 0.0f; // クールダウン残り時間
    [SerializeField] private bool _isBoosting = false;    // ブーストの発動状態管理

    // コンポーネント
    private MachineEngineController _machineEngineController;

    void Start()
    {
        _machineEngineController = GetComponent<MachineEngineController>();
        _currentGauge = _maxBoostGauge; // 初期はゲージを貯めておく
    }

    void Update()
    {
        if (_isBoosting)
        {
            // ゲージを消費
            _currentGauge -= _gaugeConsumptionRate * Time.deltaTime;

            // ゲージが無くなった場合ブーストを終了する
            if (_currentGauge <= 0)
            {
                _currentGauge = 0;
                EndBoost();
                Debug.Log("ブースト終了");
            }
        }
        else
        {
            // クールダウン中なら回復しない
            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;
            }
            else
            {
                // ゲージを回復
                if (_currentGauge < _maxBoostGauge)
                {
                    _currentGauge += _gaugeRecoveryRate * Time.deltaTime;
                    _currentGauge = Mathf.Clamp(_currentGauge, 0, _maxBoostGauge);
                }
            }
        }
    }

    /// <summary>
    /// ブースト発動できるか確認する
    /// </summary>
    public void TryStartBoost()
    {
        // 発動条件
        // ブースト発動中でない　かつ　ゲージが貯まっている　かつクールタイムが終了している場合
        if (!_isBoosting && _currentGauge > 0 && _cooldownTimer <= 0)
        {
            StartBoost();
        }
    }

    /// <summary>
    /// ブーストを発動する
    /// </summary>
    private void StartBoost()
    {
        // マシンエンジンのブースト入力を設定する
        _machineEngineController.InputBoost = _boostMultiplier;
        // ブースト発動状態
        _isBoosting = true;
        Debug.Log("ブースト発動");
    }

    /// <summary>
    /// ブーストを終了する
    /// </summary>
    private void EndBoost()
    {
        // ブーストの倍率をリセットする
        _machineEngineController.InputBoost = 1.0f;
        // ブースト発動状態を解除
        _isBoosting = false;
        // ブーストのクールタイムを設定する
        _cooldownTimer = _boostCooldown;
    }

    /// <summary>
    /// ブーストが有効かどうか
    /// </summary>
    /// <returns>ブーストの発動状態を返す</returns>
    public bool IsActiveBoost()
    {
        return _isBoosting;
    }

    /// <summary>
    /// 現在のゲージ割合を取得する(0～1)
    /// </summary>
    /// <returns>正規化された現在のゲージ割合を返す</returns>
    public float GetBoostGaugeNormalized()
    {
        return _currentGauge / _maxBoostGauge;
    }
}
