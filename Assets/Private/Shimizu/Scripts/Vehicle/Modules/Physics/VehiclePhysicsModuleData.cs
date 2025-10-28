using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Vehicle Physics Module Data")]
public class VehiclePhysicsModuleData : VehicleModuleFactoryBase
{
    [Header("�d�͐ݒ�")]

    [SerializeField] private float _recoverPower;
    [SerializeField] private float _rayLength;

    [Space]
    [Space]

    [Header("�z�o�[�ݒ�")]
    [SerializeField] private bool _isHover;
    [SerializeField] private float _hoverHeight;
    [SerializeField] private float _hoverForce;
    [SerializeField] private float _damping;


    [Header("�p������ݒ�")]
    [SerializeField] private float _rotationSpeed;

    [Header("�����萧��ݒ�")]
    [SerializeField] private float _lateralGrip;

    [Header("���ʐݒ�")]
    [SerializeField] private LayerMask _layerMask;

    // �ǂݎ���p�v���p�e�B

    // �d�͐ݒ�
    public float RecoverPower => _recoverPower;
    public float RayLength => _rayLength;

    // �z�o�[�ݒ�
    public bool IsHover => _isHover;
    public float HoverHeight => _hoverHeight;
    public float HoverForce => _hoverForce;
    public float Damping => _damping;

    // �p������ݒ�
    public float RotationSpeed => _rotationSpeed;

    // �����萧��ݒ�
    public float LateralGrip => _lateralGrip;

    // ���ʐݒ�
    public LayerMask LayerMask => _layerMask;


    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var vehiclePhysicsModule = new VehiclePhysicsModule();

        // �����ݒ�
        vehiclePhysicsModule.RecoverPower  = _recoverPower;
        vehiclePhysicsModule.RayLength     = _rayLength;
  
        vehiclePhysicsModule.IsHover       = _isHover;
        vehiclePhysicsModule.HoverHeight   = _hoverHeight;
        vehiclePhysicsModule.HoverForce    = _hoverForce;
        vehiclePhysicsModule.Damping       = _damping;
        vehiclePhysicsModule.RotationSpeed = _rotationSpeed;
        vehiclePhysicsModule.LateralGrip   = _lateralGrip;
        vehiclePhysicsModule.LayerMask     = _layerMask;

        // ����������
        vehiclePhysicsModule.Initialize(vehicleController);

        return vehiclePhysicsModule;
    }

    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<VehiclePhysicsModuleData> VehiclePhysicsModule)
        {
            VehiclePhysicsModule.ResetModule(this);
        }
    }
}
