

public interface IResettableVehicleModule<TSettings> : IVehicleModule where TSettings : VehicleModuleFactoryBase
{
    public void ResetModule(TSettings settings);
}