using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Engine Module Data")]
public class MachineEngineModuleData : VehicleModuleFactoryBase
{
    [Header("�G���W���̊�{�ݒ�")]
    [SerializeField] private float _maxThrust;            // �ő各�i��
    [SerializeField] private float _maxSpeed;             // �ō����x
    [SerializeField] private AnimationCurve _thrustCurve; // ���x�ɉ��������i��

    [Header("��R�̐ݒ�")]
    [SerializeField] private float _dragCoeff;   // ��C��R�W��
    [SerializeField] private float _brakingDrag; // �u���[�L�̋���

    [Header("���ʂ̐ݒ�")]
    [SerializeField] private float _mass; // �}�V���̎���

    [Header("�������̐ݒ�")]
    [SerializeField] private float _lateralGrip; // ������̗}�����鋭��

    [Header("�����ڗp�ݒ�")]
    [SerializeField] private float _visualYawAngle;    // ��]���̃��f���̍ő�X���p�x(Yaw)
    [SerializeField] private float _visualRollAngle;   // ��]���̃��f���̍ő�X���p�x(Roll)
    [SerializeField] private float _visualRotateSpeed; // ��]��ԑ��x

    // �ǂݎ���p
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


    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var machineEngineModule = new MachineEngineModule();

        // �����ݒ�
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

        // ����������
        machineEngineModule.Initialize(vehicleController);

        return machineEngineModule;
    }

    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineEngineModuleData> machineEngineModule)
        {
            machineEngineModule.ResetModule(this);
        }
    }
}
