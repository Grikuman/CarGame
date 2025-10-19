using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Engine Settings")]
public class EngineSettings : VehicleModuleFactoryBase
{
    public float maxRPM = 7000f;
    public float torqueCurve = 300f;
    public AnimationCurve rpmTorqueCurve;



    /// <summary> モジュールを作成する </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        return new VehicleEngineModule();
    }

    /// <summary> モジュールの設定値を初期化する </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<EngineSettings> steeringModule)
        {
            steeringModule.ResetModule(this);
        }
    }
}