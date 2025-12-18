using UnityEngine;

public abstract class UltimateBase : IUltimate
{
    protected float _ultimateTime;
    protected float _timer;
    protected bool _isActive;
    protected bool _isEnd;

    protected MachineEngineModule _engine;

    protected VehicleController Owner => _engine?.Owner;

    public virtual void Activate(MachineEngineModule engine)
    {
        _engine = engine;
        _timer = _ultimateTime;
        _isActive = true;
        _isEnd = false;
    }

    public virtual void Update()
    {
        if (!_isActive) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _isEnd = true;
        }
    }

    public virtual void End()
    {
        _isActive = false;
        _isEnd = false;
    }

    public bool IsEnd() => _isEnd;
    public bool IsActive() => _isActive;
}
