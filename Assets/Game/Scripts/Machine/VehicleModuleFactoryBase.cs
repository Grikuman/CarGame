using UnityEngine;

public abstract class VehicleModuleFactoryBase : ScriptableObject, IVehicleModuleFactory
{
    // ƒ‚ƒWƒ…[ƒ‹‚ğì¬‚·‚é
    public abstract IVehicleModule Create(VehicleController vehicleController);

    public abstract void ResetSettings(IVehicleModule module);
}

