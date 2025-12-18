using Fusion;
using UnityEngine;

public class VehicleEMPNetwork : NetworkBehaviour
{
    private MachineEMPModule _emp;

    public override void Spawned()
    {
        _emp = GetComponent<VehicleController>()
            ?.Find<MachineEMPModule>();
    }

    // “G‚Ö‚ÌEMP
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestEMP(float damage)
    {
        if (!HasStateAuthority) return;
        RPC_ApplyEnemyEMP(damage);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ApplyEnemyEMP(float damage)
    {
        _emp?.ApplyEnemyEMP(damage);
    }

    // Ž©•ª‰ñ•œ
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestSelfHeal(float heal)
    {
        if (!HasStateAuthority) return;
        RPC_ApplySelfHeal(heal);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ApplySelfHeal(float heal)
    {
        _emp?.ApplySelfHeal(heal);
    }
}
