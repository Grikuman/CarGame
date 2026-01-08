using UnityEngine;

public abstract class UltimateBase : IUltimate
{
    protected float _ultimateTime;
    protected float _timer;
    protected bool _isActive;

    protected MachineEngineModule _engine;
    protected VehicleController Owner => _engine?.Owner;

    public virtual void Activate(MachineEngineModule engine)
    {
        _engine = engine;
        _timer = _ultimateTime;
        _isActive = true;
    }

    public virtual void Update()
    {
        if (!_isActive) return;
        _timer -= Time.deltaTime;
    }

    public virtual void End()
    {
        _isActive = false;
    }

    public bool IsActive() => _isActive;
    public bool IsEnd() => _isActive && _timer <= 0f;
}
