using UnityEngine;

public class Ultimate_EMP : UltimateBase
{
    public Ultimate_EMP(float ultimateTime)
    {
        _ultimateTime = ultimateTime;
    }

    public override void Activate(MachineEngineModule engine)
    {
        // 共通のアクティブ化処理
        base.Activate(engine);
    }

    public override void End()
    {
        base.End();
    }
}
