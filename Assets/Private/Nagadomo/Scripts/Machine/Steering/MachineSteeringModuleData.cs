using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Steering Module Data")]
public class MachineSteeringModuleData : VehicleModuleFactoryBase
{


    /// <summary> モジュールを作成する </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var machineEngineModule = new MachineSteeringModule();

        // 初期設定

        // 初期化処理
        machineEngineModule.Initialize(vehicleController);

        return machineEngineModule;
    }

    /// <summary> モジュールの設定値を初期化する </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineSteeringModuleData> machineSteeringModule)
        {
            machineSteeringModule.ResetModule(this);
        }
    }
}
