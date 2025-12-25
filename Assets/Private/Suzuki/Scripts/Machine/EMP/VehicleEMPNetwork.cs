using Fusion;
using UnityEngine;

public class VehicleEMPNetwork : NetworkBehaviour
{
    private MachineEMPModule _emp;

    public override void Spawned()
    {
        Debug.Log($"[VehicleEMPNetwork] Spawned on {gameObject.name}");

        _emp = GetComponent<VehicleController>()
            ?.Find<MachineEMPModule>();

        if (_emp == null)
            Debug.LogWarning("[VehicleEMPNetwork] MachineEMPModule not found");
    }

    // ===== “G‚Ö‚ÌEMP =====
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestEMP(float damage)
    {
        Debug.Log($"[VehicleEMPNetwork] RPC_RequestEMP called damage={damage} HasStateAuthority={HasStateAuthority}");

        if (!HasStateAuthority) return;

        RPC_ApplyEnemyEMP(damage);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ApplyEnemyEMP(float damage)
    {
        Debug.Log($"[VehicleEMPNetwork] RPC_ApplyEnemyEMP damage={damage}");

        _emp?.ApplyEnemyEMP(damage);
    }

    // ===== Ž©•ª‰ñ•œ =====
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestSelfHeal(float heal)
    {
        Debug.Log($"[VehicleEMPNetwork] RPC_RequestSelfHeal heal={heal} HasStateAuthority={HasStateAuthority}");

        if (!HasStateAuthority) return;

        RPC_ApplySelfHeal(heal);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ApplySelfHeal(float heal)
    {
        Debug.Log($"[VehicleEMPNetwork] RPC_ApplySelfHeal heal={heal}");

        _emp?.ApplySelfHeal(heal);
    }

}
