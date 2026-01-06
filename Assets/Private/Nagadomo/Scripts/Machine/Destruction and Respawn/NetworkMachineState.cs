using Fusion;
using UnityEngine;

public class NetworkMachineState : NetworkBehaviour
{
    [Networked] public bool IsBoosting { get; set; }
    [Networked] public bool IsUltimateActive { get; set; }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetBoosting(bool value)
    {
        IsBoosting = value;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetUltimate(bool value)
    {
        IsUltimateActive = value;
    }
}
