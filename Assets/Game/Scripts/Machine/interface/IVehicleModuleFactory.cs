
public interface IVehicleModuleFactory
{
    /// <summary> モジュールの作成 </summary>
    public IVehicleModule Create(VehicleController vehicleController);

    /// <summary> 設定値を初期値にリセットする </summary>
    public void ResetSettings(IVehicleModule module);
}
