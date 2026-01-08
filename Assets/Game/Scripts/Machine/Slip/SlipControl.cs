using UnityEngine;

public class SlipControl
{
    private readonly Rigidbody _rb;

    // 横滑りの抑制する強さ
    private readonly float _lateralGrip;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="rb">制御対象のRigidbody</param>
    /// <param name="lateralGrip">横のグリップ係数</param>
    public SlipControl(Rigidbody rb, float lateralGrip)
    {
        _rb = rb;
        _lateralGrip = lateralGrip;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public void UpdateSlip()
    {
        // 横滑りを抑える処理
        this.ApplyGrip();
    }

    /// <summary>
    /// 横滑りを抑える処理
    /// </summary>
    public void ApplyGrip()
    {
        // 現在の速度を取得
        Vector3 velocity = _rb.linearVelocity;
        // 前方方向ベクトル
        Vector3 forward = _rb.transform.forward;

        // 前方成分と横方向成分を分解
        Vector3 forwardVel = Vector3.Project(velocity, forward);
        Vector3 lateralVel = velocity - forwardVel;

        // 横方向速度を打ち消す力を加える
        _rb.AddForce(-lateralVel * _lateralGrip, ForceMode.Acceleration);
    }
}
