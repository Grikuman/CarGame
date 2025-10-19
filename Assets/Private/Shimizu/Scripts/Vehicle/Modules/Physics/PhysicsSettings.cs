using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Physics Settings")]
public class PhysicsSettings : VehicleModuleFactoryBase
{
    [Header("重力設定")]

    [SerializeField] private float _recoverPower;
    [SerializeField] private float _rayLength;

    [Space]
    [Space]

    [SerializeField] private bool _isGrounded;
    [SerializeField] public Vector3 _groundNormal;

    [Header("ホバー設定")]
    [SerializeField] private bool _isHover;
    [SerializeField] private float _hoverHeight;
    [SerializeField] private float _hoverForce;
    [SerializeField] private float _damping;


    [Header("姿勢制御設定")]
    [SerializeField] private float _rotationSpeed;


    [Header("共通設定")]
    [SerializeField] private LayerMask _layerMask;


    /// <summary> モジュールを作成する </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var vehiclePhysicsModule = new VehiclePhysicsModule();

        // 初期設定
        vehiclePhysicsModule.RecoverPower  = _recoverPower;
        vehiclePhysicsModule.RayLength     = _rayLength;
        vehiclePhysicsModule.IsGrounded    = _isGrounded;
        vehiclePhysicsModule.GroundNormal  = _groundNormal;
        vehiclePhysicsModule.IsHover       = _isHover;
        vehiclePhysicsModule.HoverHeight   = _hoverHeight;
        vehiclePhysicsModule.HoverForce    = _hoverForce;
        vehiclePhysicsModule.Damping       = _damping;
        vehiclePhysicsModule.RotationSpeed = _rotationSpeed;
        vehiclePhysicsModule.LayerMask     = _layerMask;

        // 初期化処理
        vehiclePhysicsModule.Initialize(vehicleController);

        return new VehiclePhysicsModule();
    }

    /// <summary> モジュールの設定値を初期化する </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<PhysicsSettings> steeringModule)
        {
            steeringModule.ResetModule(this);
        }
    }
}
