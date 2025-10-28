using UnityEngine;

public class MachineUltimateController : MonoBehaviour
{
    [Header("ゲージ設定")]
    [SerializeField] private float _currentGauge = 0.0f;   // 現在のアルティメットゲージ
    [SerializeField] private float _maxUltimateGauge = 100.0f;     // 最大アルティメットゲージ
    [SerializeField] private float _gaugeIncrease = 0.01f; // ゲージ増加量

    // 現在のアルティメット
    private IUltimate _currentUltimate;

    private MachineEngineModule _machineEngineModule;

    void Start()
    {
        // コンポーネントを取得する
        _machineEngineModule = GetComponent<MachineEngineModule>();

        // とりあえずBoost Ultimateを設定しておく
        _currentUltimate = new Ultimate_Boost();
    }

    void Update()
    {
        // アルティメット発動中なら更新
        if (_currentUltimate.IsActive())
        {
            _currentUltimate.Update();
            // アルティメットが終了したら
            if (_currentUltimate.IsEnd())
            {
                // アルティメット終了処理を行う
                _currentUltimate.End();
                // ゲージをリセットする
                _currentGauge = 0.0f;
            }
        }

        // ゲージ値を補正する
        if (_currentGauge >= _maxUltimateGauge)
        {
            _currentGauge = _maxUltimateGauge;
        }
    }

    /// <summary>
    /// アルティメットを発動できるか確認する
    /// </summary>
    public void TryActivateUltimate()
    {
        // ゲージが貯まっている　かつ　アルティメットが発動されていない場合
        if (_currentGauge >= _maxUltimateGauge && !_currentUltimate.IsActive())
        {
            _currentUltimate.Activate(_machineEngineModule);
        }
    }

    /// <summary>
    /// アルティメットゲージを増加させる
    /// </summary>
    public void AddUltimateGauge()
    {
        _currentGauge += _gaugeIncrease;
    }

    /// <summary>
    /// アルティメットの種類を設定する
    /// </summary>
    /// <param name="ultimate">マシンに設定するアルティメットの種類</param>
    public void SetUltimate(IUltimate ultimate)
    {
        _currentUltimate = ultimate;
    }

    /// <summary>
    /// アルティメットが発動しているかどうか
    /// </summary>
    /// <returns>アルティメットの発動状態</returns>
    public bool IsActiveUltimate()
    {
        if (_currentUltimate.IsActive())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 現在のゲージ割合を取得する(0～1)
    /// </summary>
    /// <returns>正規化された現在のゲージ割合を返す</returns>
    public float GetUltimateGaugeNormalized()
    {
        return _currentGauge / _maxUltimateGauge;
    }
}
