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

    // 読み取り専用
    public float MaxThrust => _maxThrust;
    public float MaxSpeed => _maxSpeed;
    public AnimationCurve ThrustCurve => _thrustCurve;
    public float DragCoeff => _dragCoeff;
    public float BrakingDrag => _brakingDrag;
    public float Mass => _mass;


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
