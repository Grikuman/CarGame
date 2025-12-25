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

        // ===========================================================
        // 攻撃側（otherVC）が Boost or Ultimate 中でなければ破壊不可
        // ===========================================================

        var boost = otherVC.Find<MachineBoostModule>();
        var ultimate = otherVC.Find<MachineUltimateModule>();

        bool isBoosting = boost != null && boost.IsActiveBoost();
        bool isUltimate = ultimate != null && ultimate.IsActiveUltimate();

        // 攻撃可能かチェック（どちらも false なら破壊できない）
        if (!isBoosting && !isUltimate)
        {
            return;
        }

        // シールド中は破壊不可
        var shield = _vehicleController.GetComponent<Ultimate_Shield>();
        if (shield != null && shield.IsShieldActive())
        {
            return; 
        }


        // ===========================================================
        // 後方衝突判定
        // ===========================================================

        Vector3 dirToMe = (_vehicleController.transform.position - other.transform.position).normalized;
        float angle = Vector3.Angle(other.transform.forward, dirToMe);

        if (angle < RearHitAngle)
        {
            var net = _vehicleController.GetComponent<VehicleDestructionNetwork>();
            if (net != null)
            {
                net.RPC_RequestKill();
            }

        }
    }
}
