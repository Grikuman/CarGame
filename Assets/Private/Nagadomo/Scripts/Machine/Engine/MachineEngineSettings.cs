using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Engine Settings")]
public class MachineEngineSettings : VehicleModuleFactoryBase
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
    [SerializeField] private GameObject _visualModel; // �}�V���̃��f���Q��(�q�I�u�W�F�N�g)
    [SerializeField] private float _visualYawAngle;   // ��]���̃��f���̍ő�X���p�x(Yaw)
    [SerializeField] private float _visualRollAngle;  // ��]���̃��f���̍ő�X���p�x(Roll)
    [SerializeField] private float _visualRotateSpeed; // ��]��ԑ��x

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

        if (_visualModel != null)
        {
            var visualInstance = GameObject.Instantiate(_visualModel, vehicleController.transform);
            machineEngineModule.VisualModel = visualInstance.transform;
        }

        // ����������
        machineEngineModule.Initialize(vehicleController);

        return machineEngineModule;
    }

    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineEngineSettings> machineEngineModule)
        {
            machineEngineModule.ResetModule(this);
        }
    }
}
