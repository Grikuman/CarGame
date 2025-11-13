using UnityEngine;
using Cinemachine;

[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("")]
public class CameraCornerOffsetExtension : CinemachineExtension
{
    [Tooltip("車体などのTransform")]
    public Transform target;

    [Tooltip("コーナリング時の横オフセット量（メートル単位）")]
    public float maxOffset = 1.5f;

    [Tooltip("ロールに対する反応強度")]
    public float sensitivity = 0.02f;

    [Tooltip("スムージング速度")]
    public float smooth = 5f;

    private Vector3 currentOffset = Vector3.zero;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (target == null || stage != CinemachineCore.Stage.Body)
            return;

        // 車のローカルZ角度（傾き）取得
        float rollZ = target.localEulerAngles.z;
        if (rollZ > 180f) rollZ -= 360f;

        // ロール角に応じて外側へオフセット
        float targetOffsetX = Mathf.Clamp(-rollZ * sensitivity, -maxOffset, maxOffset);

        // スムーズ補間
        currentOffset.x = Mathf.Lerp(currentOffset.x, targetOffsetX, deltaTime * smooth);

        // カメラのローカル位置に反映
        state.PositionCorrection += state.RawOrientation * currentOffset;
    }
}
