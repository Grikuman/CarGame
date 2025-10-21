using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Engine Settings")]
public class EngineSettings : VehicleModuleFactoryBase
{
    public float maxRPM = 7000f;
    public float torqueCurve = 300f;
    public AnimationCurve rpmTorqueCurve;



    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        return new VehicleEngineModule();
    }

    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<EngineSettings> steeringModule)
        {
            steeringModule.ResetModule(this);
        }
    }
}