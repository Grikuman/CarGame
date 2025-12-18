using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineUltimateModule : IVehicleModule, IResettableVehicleModule<MachineUltimateModuleData>
{
    public float CurrentGauge { get; set; }
    public float MaxUltimateGauge { get;set; }
    public float GaugeIncrease { get; set; }
    public bool InputUltimate {  get; set; }

    // 現在のアルティメット
    private IUltimate _currentUltimate = null;

    // エンジンモジュール
    private MachineEngineModule _machineEngineModule;

    private bool _isActive = true;
    private VehicleController _vehicleController = null;

    /// <summary> アクティブ状態を設定 </summary>
    public void SetActive(bool value) => _isActive = value;
    /// <summary> アクティブ状態を取得 </summary>
    public bool GetIsActive() => _isActive;

    /// <summary> 初期化処理 </summary>
    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;

        // 初期のゲージを設定する
        CurrentGauge = MaxUltimateGauge;
        // とりあえずBoost Ultimateを設定しておく
        //SetUltimate(new Ultimate_Boost(2.5f, 3.0f));
        SetUltimate(new Ultimate_EMP());


    }

    /// <summary> 開始処理 </summary>
    public void Start()
    {
        // モジュールデータリセット処理
        _vehicleController.ResetSettings<MachineBoostModuleData>();

        // エンジンモジュールを取得する
        _machineEngineModule = _vehicleController.Find<MachineEngineModule>();
    }

    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    {
        // 入力取得
        InputUltimate = _vehicleController.Ultimate;

        if(InputUltimate)
        {
            // アルティメットを発動できるか確認する
            this.TryActivateUltimate();
        }

        // アルティメット発動中なら更新
        if (_currentUltimate.IsActive())
        {
            _currentUltimate.Update();
            // アルティメットが終了したら
            if (_currentUltimate.IsEnd())
            {
                Debug.Log("アルティメットを終了");
                // アルティメット終了処理を行う
                _currentUltimate.End();
                // ゲージをリセットする
                CurrentGauge = 0.0f;
            }
        }

        // ゲージ値を補正する
        if (CurrentGauge >= MaxUltimateGauge)
        {
            CurrentGauge = MaxUltimateGauge;
        }

        // 入力の初期化
        InputUltimate = false;
        _vehicleController.Ultimate = InputUltimate;
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        
    }

    // リセット時の処理
    public void ResetModule(MachineUltimateModuleData data)
    {
        CurrentGauge = data.CurrentGauge;
        MaxUltimateGauge = data.MaxUltimateGauge;
        GaugeIncrease = data.GaugeIncrease;

        // 初期のゲージを設定する
        CurrentGauge = MaxUltimateGauge;
    }

    /// <summary>
    /// アルティメットを発動できるか確認する
    /// </summary>
    public void TryActivateUltimate()
    {
        // ゲージが貯まっている　かつ　アルティメットが発動されていない場合
        if (CurrentGauge >= MaxUltimateGauge && !_currentUltimate.IsActive())
        {
            _currentUltimate.Activate(_machineEngineModule);
            Debug.Log("アルティメットを発動");
        }
    }

    /// <summary>
    /// アルティメットゲージを増加させる
    /// </summary>
    public void AddUltimateGauge()
    {
        CurrentGauge += GaugeIncrease;
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
        return CurrentGauge / MaxUltimateGauge;
    }
}
