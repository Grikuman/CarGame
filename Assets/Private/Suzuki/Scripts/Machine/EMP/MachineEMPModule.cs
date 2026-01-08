using UnityEngine;

public class MachineEMPModule :
    IVehicleModule,
    IResettableVehicleModule<MachineEMPModuleData>
{
    private bool _isActive = true;
    private VehicleController _vehicleController;

    // EMP設定
    private float _range;
    private float _selfGaugeRecoverAmount;
    private float _enemyGaugeDecrease;

    // 参照
    private MachineBoostModule _boostModule;

    /* =========================
     * IVehicleModule
     * ========================= */

    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;
    }

    public void Start()
    {
        _vehicleController.ResetSettings<MachineEMPModuleData>();
        _boostModule = _vehicleController.Find<MachineBoostModule>();
    }

    public void UpdateModule() { }
    public void FixedUpdateModule() { }

    public void SetActive(bool value) => _isActive = value;
    public bool GetIsActive() => _isActive;

    /* =========================
     * Reset
     * ========================= */

    public void ResetModule(MachineEMPModuleData data)
    {
        _range = data.Range;
        _selfGaugeRecoverAmount = data.SelfGaugeRecover;
        _enemyGaugeDecrease = data.EnemyGaugeDecrease;
    }

    /* =========================
     * EMP 発動
     * ========================= */

    public void ActivateEMP()
    {
        if (!_isActive) return;

        bool hitEnemy = false;

        Collider[] hits = Physics.OverlapSphere(
            _vehicleController.transform.position,
            _range
        );

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Player")) continue;

            if (!hit.TryGetComponent(out VehicleController targetVC)) continue;
            if (targetVC == _vehicleController) continue;

            var netEMP = targetVC.GetComponent<NetworkMachineEMP>();
            if (netEMP == null) continue;

            // ★ EMP通知（StateAuthorityへ）
            netEMP.RPC_RequestEMP(_enemyGaugeDecrease);

            hitEnemy = true;
        }

        // 敵がいた場合のみ自己回復
        if (hitEnemy && _boostModule != null)
        {
            _boostModule.IncreaseGauge(_selfGaugeRecoverAmount);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_vehicleController == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(
            _vehicleController.transform.position,
            _range
        );
    }
#endif
}
