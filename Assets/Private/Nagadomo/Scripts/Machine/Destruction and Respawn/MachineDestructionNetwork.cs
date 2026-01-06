using Fusion;
using UnityEngine;

public class NetworkMachineDestruction : NetworkBehaviour
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
        if (_respawn == null)
        {
            var vc = GetComponent<VehicleController>();
            if (vc != null)
                _respawn = vc.Find<MachineRespawnModule>();

            if (_respawn == null)
                return;
        }

        if (IsDead && !_handled)
        {
            _handled = true;
            _respawn.Kill();
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
