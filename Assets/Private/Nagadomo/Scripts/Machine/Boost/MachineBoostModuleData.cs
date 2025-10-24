using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Boost Module Data")]
public class MachineBoostModuleData : VehicleModuleFactoryBase
{
    [Header("ブースト設定")]
    [SerializeField] private float _boostMultiplier = 1.5f;       // 倍率
    [SerializeField] private float _maxBoostGauge = 100.0f;       // 最大ゲージ量
    [SerializeField] private float _gaugeConsumptionRate = 20.0f; // 1秒あたり消費量
    [SerializeField] private float _gaugeRecoveryRate = 20.0f;    // 1秒あたり回復量
    [SerializeField] private float _boostCooldown = 3.0f;         // ブースト後のクールダウン

    [SerializeField] private float _currentGauge;
    [SerializeField] private bool _isBoosting = true;

    // 読み取り専用
    public float BoostMultiplier => _boostMultiplier;
    public float MaxBoostGauge => _maxBoostGauge;
    public float GaugeConsumptionRate => _gaugeConsumptionRate;
    public float GaugeRecoveryRate => _gaugeRecoveryRate;
    public float BoostCooldown => _boostCooldown;

    /// <summary> モジュールを作成する </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var machineBoostModule = new MachineBoostModule();

        // 初期設定
        machineBoostModule.BoostMultiplier = _boostMultiplier;
        machineBoostModule.MaxBoostGauge = _maxBoostGauge;
        machineBoostModule.GaugeConsumptionRate = _gaugeConsumptionRate;
        machineBoostModule.GaugeRecoveryRate = _gaugeRecoveryRate;
        machineBoostModule.BoostCoolDown = _boostCooldown;

        // 初期化処理
        machineBoostModule.Initialize(vehicleController);

        return machineBoostModule;
    }

    /// <summary> モジュールの設定値を初期化する </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineBoostModuleData> machineBoostModule)
        {
            machineBoostModule.ResetModule(this);
        }
    }
}
