using UnityEngine;
public class Ultimate_Shield : UltimateBase
{
    private VehicleController _selfVehicle;

    private bool _shieldActive = false;

    public Ultimate_Shield(float duration, float ultimateTime, VehicleController vc)
    {
        _ultimateTime = ultimateTime;
        _selfVehicle = vc;
    }

    public override void Activate(MachineEngineModule engine)
    {
        base.Activate(engine);

        _shieldActive = true;
        Debug.Log($"[ShieldState] ON : {_selfVehicle.name}");
    }

    public override void End()
    {
        base.End();

        _shieldActive = false;
        Debug.Log($"[ShieldState] OFF : {_selfVehicle.name}");
    }

    public bool IsShieldActive()
    {
        return _shieldActive;
    }
}
