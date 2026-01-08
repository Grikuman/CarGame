using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine EMP Module Data")]
public class MachineEMPModuleData : VehicleModuleFactoryBase
{
    [Header("EMP Ý’è")]
    [SerializeField] private float _range = 10.0f;
    [SerializeField] private float _enemyGaugeDecrease = 30.0f;
    [SerializeField] private float _selfGaugeRecover = 20.0f;

    public float Range => _range;
    public float EnemyGaugeDecrease => _enemyGaugeDecrease;
    public float SelfGaugeRecover => _selfGaugeRecover;

    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var module = new MachineEMPModule();

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
