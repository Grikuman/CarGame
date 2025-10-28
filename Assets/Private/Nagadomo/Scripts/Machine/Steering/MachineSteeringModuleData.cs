using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Steering Module Data")]
public class MachineSteeringModuleData : VehicleModuleFactoryBase
{
    [Header("�����ڂ̉�]�ݒ�")]
    [SerializeField] private float _visualYawAngle;    // ��]���̃��f���̍ő�X���p�x(Yaw)
    [SerializeField] private float _visualRollAngle;   // ��]���̃��f���̍ő�X���p�x(Roll)
    [SerializeField] private float _visualRotateSpeed; // ��]��ԑ��x

    // �ǂݎ���p
    public float VisualYawAngle => _visualYawAngle;
    public float VisualRollAngle => _visualRollAngle;
    public float VisualRotateSpeed => _visualRotateSpeed;

    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var machineSteeringModule = new MachineSteeringModule();

        // �����ݒ�
        machineSteeringModule.VisualYawAngle = _visualYawAngle;
        machineSteeringModule.VisualRollAngle = _visualRollAngle;
        machineSteeringModule.VisualRotateSpeed = _visualRotateSpeed;

        // ����������
        machineSteeringModule.Initialize(vehicleController);

        return machineSteeringModule;
    }

    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineSteeringModuleData> machineSteeringModule)
        {
            machineSteeringModule.ResetModule(this);
        }
    }
}
