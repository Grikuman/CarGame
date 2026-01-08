using UnityEngine;

public class Ultimate_Boost : UltimateBase
{
    private float _boostMultiplier;   // ブースト倍率
    private float _originalMultiplier; // 元のブースト倍率

    public Ultimate_Boost(float boostMultiplier, float ultimateTime)
    {
        _boostMultiplier = boostMultiplier;
        _ultimateTime = ultimateTime;
    }

    public override void Activate(MachineEngineModule engine)
    {
        base.Activate(engine);

        // 元のブースト倍率を保存
        _originalMultiplier = engine.BoostMultiplier;

        // ブースト適用
        engine.BoostMultiplier *= _boostMultiplier;
    }

    public override void End()
    {
        if (_engine != null)
        {
            // 元のブースト倍率に戻す
            _engine.BoostMultiplier = _originalMultiplier;
        }

        base.End();
    }
}
