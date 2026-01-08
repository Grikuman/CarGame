using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Steering Module Data")]
public class MachineSteeringModuleData : VehicleModuleFactoryBase
{
    [Header("見た目の回転設定")]
    [SerializeField] private float _visualYawAngle;    // 回転時のモデルの最大傾き角度(Yaw)
    [SerializeField] private float _visualRollAngle;   // 回転時のモデルの最大傾き角度(Roll)
    [SerializeField] private float _visualRotateSpeed; // 回転補間速度

    // 読み取り専用
    public float VisualYawAngle => _visualYawAngle;
    public float VisualRollAngle => _visualRollAngle;
    public float VisualRotateSpeed => _visualRotateSpeed;

    /// <summary> モジュールを作成する </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var machineSteeringModule = new MachineSteeringModule();

        // 初期設定
        machineSteeringModule.VisualYawAngle = _visualYawAngle;
        machineSteeringModule.VisualRollAngle = _visualRollAngle;
        machineSteeringModule.VisualRotateSpeed = _visualRotateSpeed;

        // 初期化処理
        machineSteeringModule.Initialize(vehicleController);

        return machineSteeringModule;
    }

    /// <summary> モジュールの設定値を初期化する </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineSteeringModuleData> machineSteeringModule)
        {
            machineSteeringModule.ResetModule(this);
        }
    }
}
