using UnityEngine;

public class MachineDestructionModule : IVehicleModule, IResettableVehicleModule<MachineDestructionModuleData>
{
    public float RearHitAngle { get; set; }

    private bool _isActive = true;
    private VehicleController _vehicleController = null;

    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;
    }

    public void Start()
    {
        _vehicleController.ResetSettings<MachineDestructionModuleData>();
    }

    public void SetActive(bool value) => _isActive = value;
    public bool GetIsActive() => _isActive;

    public void UpdateModule() { }
    public void FixedUpdateModule() { }

    public void ResetModule(MachineDestructionModuleData data)
    {
        RearHitAngle = data.RearHitAngle;
    }

    public void Trigger(Collider other)
    {
        if (!_isActive) return;
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent(out VehicleController otherVC)) return;

        var combatNet = otherVC.GetComponent<NetworkMachineState>();
        if (combatNet == null) return;

        // ★ ネットワーク越しに見える状態
        bool canDestroy =
            combatNet.IsBoosting || combatNet.IsUltimateActive;

        if (!canDestroy) return;

        // 後方判定
        Vector3 dirToMe =
            (_vehicleController.transform.position - other.transform.position).normalized;

        float angle = Vector3.Angle(other.transform.forward, dirToMe);

        if (angle < RearHitAngle)
        {
            var net = _vehicleController.GetComponent<NetworkMachineDestruction>();
            net?.RPC_RequestKill();
        }
    }

}
