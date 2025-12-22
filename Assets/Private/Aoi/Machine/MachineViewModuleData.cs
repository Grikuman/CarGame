using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine View Module Data")]
public class MachineViewModuleData : VehicleModuleFactoryBase
{
    [SerializeField] GameObject m_vehicleViewModel;
    public GameObject VehcleModel { get { return m_vehicleViewModel; }  }

    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var module = new MachineViewModule();

        module.VehcleModel = m_vehicleViewModel;

        module.Initialize(vehicleController);
        return module;
    }

    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineViewModuleData> machineViewModule)
        {
            machineViewModule.ResetModule(this);
        }
    }

}
