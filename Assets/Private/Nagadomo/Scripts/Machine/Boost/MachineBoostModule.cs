using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MachineBoostModule : IVehicleModule, IResettableVehicleModule<MachineBoostModuleData>
{
    public float BoostMultiplier { get; set; }
    public float MaxBoostGauge { get; set; }
    public float GaugeConsumptionRate { get; set; }
    public float GaugeRecoveryRate { get; set; }
    public float BoostCoolDown { get; set; }
    public float CurrentGauge { get; set; }
    public float CoolDownTimer { get; private set; }
    public bool IsBoosting { get; set; }

    // エンジンモジュール
    private MachineEngineModule _machineEngineModule;
    // アルティメットモジュール
    //private MachineUltimateModule _machineUltimateModule;

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
    }

    /// <summary> 開始処理 </summary>
    public void Start()
    {
        Debug.Log("Start MachineBoostModule");
        // モジュールデータリセット処理
        _vehicleController.ResetSettings<MachineBoostModuleData>();
    }

    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    {
        Debug.Log("Update MachineBoostModule");
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        Debug.Log("FixedUpdate MachineBoostModule");
    }

    // リセット時の処理
    public void ResetModule(MachineBoostModuleData data)
    {
        Debug.Log("Reset MachineBoostData");
    }
}
