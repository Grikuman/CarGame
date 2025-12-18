using System.Drawing.Printing;
using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine EMP Module Data")]
public class MachineEMPModuleData : VehicleModuleFactoryBase
{
    [Header("EMPÝ’è")]
    [SerializeField] private float _empDuration = 2.5f;

    public float EMPDuration => _empDuration;

    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var module = new MachineEMPModule();

        module.EMPDuration = _empDuration;
        module.Initialize(vehicleController);

        return module;
    }

    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineEMPModuleData> emp)
        {
            emp.ResetModule(this);
        }
    }
}
