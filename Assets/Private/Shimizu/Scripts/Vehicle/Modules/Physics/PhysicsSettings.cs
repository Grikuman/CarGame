using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Physics Settings")]
public class PhysicsSettings : VehicleModuleFactoryBase
{
    [Header("�d�͐ݒ�")]

    [SerializeField] private float _recoverPower;
    [SerializeField] private float _rayLength;

    [Space]
    [Space]

    [SerializeField] private bool _isGrounded;
    [SerializeField] public Vector3 _groundNormal;

    [Header("�z�o�[�ݒ�")]
    [SerializeField] private bool _isHover;
    [SerializeField] private float _hoverHeight;
    [SerializeField] private float _hoverForce;
    [SerializeField] private float _damping;


    [Header("�p������ݒ�")]
    [SerializeField] private float _rotationSpeed;


    [Header("���ʐݒ�")]
    [SerializeField] private LayerMask _layerMask;


    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var vehiclePhysicsModule = new VehiclePhysicsModule();

        // �����ݒ�
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

        // ����������
        vehiclePhysicsModule.Initialize(vehicleController);

        return new VehiclePhysicsModule();
    }

    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<PhysicsSettings> steeringModule)
        {
            steeringModule.ResetModule(this);
        }
    }
}
