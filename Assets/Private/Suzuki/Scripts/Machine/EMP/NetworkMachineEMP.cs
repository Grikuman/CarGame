using Fusion;
using UnityEngine;

public class NetworkMachineEMP : NetworkBehaviour
{
    [Networked] public bool IsEMPAffected { get; set; }
    [Networked] private float _pendingDecrease { get; set; }

    private MachineBoostModule _boostModule;
    private bool _handled = false;

    public override void Spawned()
    {
        var vc = GetComponent<VehicleController>();
        if (vc != null)
        {
            _boostModule = vc.Find<MachineBoostModule>();
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (_boostModule == null)
        {
            var vc = GetComponent<VehicleController>();
            if (vc != null)
                _boostModule = vc.Find<MachineBoostModule>();

            if (_boostModule == null)
                return;
        }

        // EMPŒø‰Ê‚ðˆê“x‚¾‚¯“K—p
        if (IsEMPAffected && !_handled)
        {
            _handled = true;
            _boostModule.DecreaseGauge(_pendingDecrease);
        }


        if (!IsEMPAffected)
        {
            _handled = false;
        }
    }

    /* =========================
     * RPC
     * ========================= */

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_RequestEMP(float decreaseAmount)
    {
        IsEMPAffected = true;
        _pendingDecrease = decreaseAmount;
    }

}
