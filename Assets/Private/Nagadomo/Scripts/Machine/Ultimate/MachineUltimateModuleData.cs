using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Ultimate Module Data")]
public class MachineUltimateModuleData : VehicleModuleFactoryBase
{
    [Header("ゲージ設定")]
    [SerializeField] private float _currentGauge = 0.0f;   // 現在のアルティメットゲージ
    [SerializeField] private float _maxUltimateGauge = 100.0f;     // 最大アルティメットゲージ
    [SerializeField] private float _gaugeIncrease = 0.01f; // ゲージ増加量

    // 読み取り専用
    public float CurrentGauge => _currentGauge;
    public float MaxUltimateGauge => _maxUltimateGauge;
    public float GaugeIncrease => _gaugeIncrease;

    /// <summary> モジュールを作成する </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var machineUltimateModule = new MachineUltimateModule();

        // 初期設定
        machineUltimateModule.CurrentGauge = _currentGauge;
        machineUltimateModule.MaxUltimateGauge = _maxUltimateGauge;
        machineUltimateModule.GaugeIncrease = _gaugeIncrease;

        // 初期化処理
        machineUltimateModule.Initialize(vehicleController);

        return machineUltimateModule;
    }

    /// <summary> モジュールの設定値を初期化する </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineUltimateModuleData> machineBoostModule)
        {
            machineBoostModule.ResetModule(this);
        }
    }
}
