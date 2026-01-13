using UnityEngine;

public class Ultimate_Shield : UltimateBase
{
    private VehicleController _selfVehicle;
    private MachineShieldState _shieldState;

    public Ultimate_Shield(float ultimateTime)
    {
        _ultimateTime = ultimateTime;
    }

    public override void Activate(MachineEngineModule engine)
    {
        base.Activate(engine);

        _selfVehicle = engine.Owner.GetComponent<VehicleController>();

        if (_shieldState == null)
        {
            _shieldState = _selfVehicle.GetComponent<MachineShieldState>();
        }

        if (_shieldState != null)
        {
            _shieldState.Enable();
        }

        Debug.Log($"[Ultimate_Shield] Activate : {_selfVehicle.name}");
    }

    public override void End()
    {
        base.End();

        if (_shieldState != null)
        {
            _shieldState.Disable();
        }

        Debug.Log($"[Ultimate_Shield] End : {_selfVehicle.name}");
    }
}
