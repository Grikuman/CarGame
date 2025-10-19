using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Steering Settings")]
public class SteeringSettings : VehicleModuleFactoryBase
{
    [Header("Steering Parameters")]
    public float steeringSensitivity = 1.0f;
    public float maxSteeringAngle = 30f;
    public float steeringSpeed = 5f;

    /// <summary> ���W���[�����쐬���� </summary>
    public override IVehicleModule Create(VehicleController vehicleController)
    {
        return new VehicleSteeringModule();
    }
    
    /// <summary> ���W���[���̐ݒ�l������������ </summary>
    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<SteeringSettings> steeringModule)
        {
            steeringModule.ResetModule(this);
        }
    }
}
