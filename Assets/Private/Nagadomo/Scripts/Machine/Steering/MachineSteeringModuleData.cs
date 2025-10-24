using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Steering Module Data")]
public class MachineSteeringModuleData : VehicleModuleFactoryBase
{


    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var machineEngineModule = new MachineSteeringModule();

        // �����ݒ�

        // ����������
        machineEngineModule.Initialize(vehicleController);

        return machineEngineModule;
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
