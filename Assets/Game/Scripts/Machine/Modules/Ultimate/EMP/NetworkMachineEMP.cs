using Fusion;
using UnityEngine;

public class NetworkMachineEMP : NetworkBehaviour
{
    private MachineBoostModule _boost;

    public override void Spawned()
    {
        _boost = GetComponent<VehicleController>()
            ?.Find<MachineBoostModule>();
    }

    public override void FixedUpdateNetwork()
    {
        if (_boost == null)
        {
            var vc = GetComponent<VehicleController>();
            if (vc != null)
                _boost = vc.Find<MachineBoostModule>();

            if (_boost == null)
                return;
        }
    }

    // 敵へのEMP
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestEMP(float damage)
    {
        if (!HasStateAuthority) return;
        _boost?.DecreaseGauge(damage);
        Debug.Log("リクエスト完了");
    }
}
