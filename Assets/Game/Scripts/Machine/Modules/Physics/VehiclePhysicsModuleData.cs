using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Vehicle Physics Module Data")]
public class VehiclePhysicsModuleData : VehicleModuleFactoryBase
{
    [Header("重力設定")]

    [SerializeField] private float _recoverPower;
    [SerializeField] private float _rayLength;

    [Space]
    [Space]

    [Header("ホバー設定")]
    [SerializeField] private bool _isHover;
    [SerializeField] private float _hoverHeight;
    [SerializeField] private float _hoverForce;
    [SerializeField] private float _damping;


    [Header("姿勢制御設定")]
    [SerializeField] private float _rotationSpeed;

    [Header("横滑り制御設定")]
    [SerializeField] private float _lateralGrip;

    [Header("共通設定")]
    [SerializeField] private LayerMask _layerMask;

    // 読み取り専用プロパティ

    // 重力設定
    public float RecoverPower => _recoverPower;
    public float RayLength => _rayLength;

    // ホバー設定
    public bool IsHover => _isHover;
    public float HoverHeight => _hoverHeight;
    public float HoverForce => _hoverForce;
    public float Damping => _damping;

    // 姿勢制御設定
    public float RotationSpeed => _rotationSpeed;

    // 横滑り制御設定
    public float LateralGrip => _lateralGrip;

    // 共通設定
    public LayerMask LayerMask => _layerMask;


    /// <summary> モジュールを作成する </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var vehiclePhysicsModule = new VehiclePhysicsModule();

        // 初期設定
        vehiclePhysicsModule.RecoverPower  = _recoverPower;
        vehiclePhysicsModule.RayLength     = _rayLength;
  
        vehiclePhysicsModule.IsHover       = _isHover;
        vehiclePhysicsModule.HoverHeight   = _hoverHeight;
        vehiclePhysicsModule.HoverForce    = _hoverForce;
        vehiclePhysicsModule.Damping       = _damping;
        vehiclePhysicsModule.RotationSpeed = _rotationSpeed;
        vehiclePhysicsModule.LateralGrip   = _lateralGrip;
        vehiclePhysicsModule.LayerMask     = _layerMask;

        // 初期化処理
        vehiclePhysicsModule.Initialize(vehicleController);

        return vehiclePhysicsModule;
    }

    /// <summary> モジュールの設定値を初期化する </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<VehiclePhysicsModuleData> VehiclePhysicsModule)
        {
            VehiclePhysicsModule.ResetModule(this);
        }
    }
}
