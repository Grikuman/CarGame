using UnityEngine;

public abstract class VehicleModuleFactoryBase : ScriptableObject, IVehicleModuleFactory
{
    // ���W���[�����쐬����
    public abstract IVehicleModule Create(VehicleController vehicleController);

    public abstract void ResetSettings(IVehicleModule module);
}

