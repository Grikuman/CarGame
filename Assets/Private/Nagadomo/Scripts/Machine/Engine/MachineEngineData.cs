using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Engine Module Data")]
public class MachineEngineModuleData : VehicleModuleFactoryBase
{
    [Header("エンジンの基本設定")]
    [SerializeField] private float _maxThrust;            // 最大推進力
    [SerializeField] private float _maxSpeed;             // 最高速度
    [SerializeField] private AnimationCurve _thrustCurve; // 速度に応じた推進力

    [Header("抵抗の設定")]
    [SerializeField] private float _dragCoeff;   // 空気抵抗係数
    [SerializeField] private float _brakingDrag; // ブレーキの強さ

    [Header("質量の設定")]
    [SerializeField] private float _mass; // マシンの質量

    [Header("横方向の設定")]
    [SerializeField] private float _lateralGrip; // 横滑りの抑制する強さ

    [Header("見た目用設定")]
    [SerializeField] private float _visualYawAngle;    // 回転時のモデルの最大傾き角度(Yaw)
    [SerializeField] private float _visualRollAngle;   // 回転時のモデルの最大傾き角度(Roll)
    [SerializeField] private float _visualRotateSpeed; // 回転補間速度

    // 読み取り専用
    public float MaxThrust => _maxThrust;
    public float MaxSpeed => _maxSpeed;
    public AnimationCurve ThrustCurve => _thrustCurve;
    public float DragCoeff => _dragCoeff;
    public float BrakingDrag => _brakingDrag;
    public float Mass => _mass;
    public float LateralGrip => _lateralGrip;
    public float VisualYawAngle => _visualYawAngle;
    public float VisualRollAngle => _visualRollAngle;
    public float VisualRotateSpeed => _visualRotateSpeed;


    /// <summary> モジュールを作成する </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var machineEngineModule = new MachineEngineModule();

        // 初期設定
        machineEngineModule.MaxThrust = _maxThrust;
        machineEngineModule.MaxSpeed = _maxSpeed;
        machineEngineModule.ThrustCurve = _thrustCurve;

        machineEngineModule.DragCoeff = _dragCoeff;
        machineEngineModule.BrakingDrag = _brakingDrag;

        machineEngineModule.Mass = _mass;

        machineEngineModule.LateralGrip = _lateralGrip;

        machineEngineModule.VisualYawAngle = _visualYawAngle;
        machineEngineModule.VisualRollAngle = _visualRollAngle;
        machineEngineModule.VisualRotateSpeed = _visualRotateSpeed;

        // 初期化処理
        machineEngineModule.Initialize(vehicleController);

        return machineEngineModule;
    }

    /// <summary> モジュールの設定値を初期化する </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineEngineModuleData> machineEngineModule)
        {
            machineEngineModule.ResetModule(this);
        }
    }
}
