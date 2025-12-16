using Fusion;
using UnityEngine;

public class VehicleDestructionNetwork : NetworkBehaviour
{
    [Networked] public bool IsDead { get; set; }

    private MachineRespawnModule _respawn;
    private bool _handled = false;

    public override void Spawned()
    {
        _respawn = GetComponent<VehicleController>()
            ?.Find<MachineRespawnModule>();
    }

    public override void FixedUpdateNetwork()
    {
        if (IsDead && !_handled)
        {
            _handled = true;

            // ★ ローカル演出として Kill を実行
            _respawn?.Kill();
        }

        if (!IsDead)
        {
            _handled = false;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestKill()
    {
        IsDead = true;
    }

}
