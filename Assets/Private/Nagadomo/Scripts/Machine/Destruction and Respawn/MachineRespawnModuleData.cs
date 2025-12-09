using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Machine Respawn Module Data")]
public class MachineRespawnModuleData : VehicleModuleFactoryBase
{
    [Header("リスポーン設定")]
    [SerializeField] private float _respawnDelay = 0.1f;

    [Header("キルされた瞬間の爆発エフェクト")]
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _explosionScale = 3.0f;

    public float RespawnDelay => _respawnDelay;
    public GameObject ExplosionPrefab => _explosionPrefab;
    public float ExplosionScale => _explosionScale;

    public override IVehicleModule Create(VehicleController vehicleController)
    {
        var module = new MachineRespawnModule();

        module.RespawnDelay = _respawnDelay;
        module.ExplosionPrefab = _explosionPrefab;

        module.Initialize(vehicleController);
        return module;
    }

    public override void ResetSettings(IVehicleModule module)
    {
        if (module is IResettableVehicleModule<MachineRespawnModuleData> machineRespawnModule)
        {
            machineRespawnModule.ResetModule(this);
        }
    }
}
