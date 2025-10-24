using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Player Input Module Data")]
public class PlayerInputModuleData : VehicleModuleFactoryBase
{
    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var playerInputModule = new PlayerInputModule();

        // �����ݒ�

        // ����������
        playerInputModule.Initialize(vehicleController);

        return playerInputModule;
    }

    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<PlayerInputModuleData> playerInputModule)
        {
            playerInputModule.ResetModule(this);
        }
    }
}
