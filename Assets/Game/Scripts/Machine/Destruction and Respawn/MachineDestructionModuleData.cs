using System.Drawing.Printing;
using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Destruction Module Data")]
public class MachineDestructionModuleData : VehicleModuleFactoryBase
{
    [Header("Ž€–S”»’è")]
    [SerializeField] private float _rearHitAngle = 120.0f;

    public float RearHitAngle => _rearHitAngle;

    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var module = new MachineDestructionModule();

        module.RearHitAngle = _rearHitAngle;

        module.Initialize(vehicleController);
        return module;
    }

    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineDestructionModuleData> machineDestructionModule)
        {
            machineDestructionModule.ResetModule(this);
        }
    }
}
