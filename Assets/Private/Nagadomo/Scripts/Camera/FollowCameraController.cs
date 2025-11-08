using UnityEngine;

public class FollowCameraController : MonoBehaviour
{
    [Header("ターゲット設定")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 3.5f, -8f);

    [Header("スムーズ設定")]
    [Range(0.01f, 1f)]
    public float followSmoothness = 0.08f;
    public float rotationSmoothness = 5f;

    [Header("ロール追従設定")]
    public float rollFollowStrength = 1.0f;
    public float rollSmoothness = 5f;

    [Header("Z距離制限設定")]
    public float maxZDistance = -5f; // カメラがターゲットよりこれ以上後ろに行かない（ローカルZ基準）

    private Vector3 _velocity;
    private float _currentRoll;

    // ブーストモジュール
    MachineBoostModule _machineBoostModule;
    // アルティメットモジュール
    MachineUltimateModule _machineUltimateModule;

    void FixedUpdate()
    {
        if (!target) return;

        // --- 理想位置 ---
        Vector3 desiredWorldPos = target.TransformPoint(offset);

        // --- 通常スムーズ追従 ---
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, desiredWorldPos, ref _velocity, followSmoothness);

        // --- ターゲット座標系でのカメラ位置を取得 ---
        Vector3 localPos = target.InverseTransformPoint(smoothedPos);

        // --- Z軸制限 ---
        if (localPos.z < maxZDistance)
        {
            localPos.z = maxZDistance;
            smoothedPos = target.TransformPoint(localPos);
        }

        // --- カメラ位置確定 ---
        transform.position = smoothedPos;

        // --- 向き ---
        Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

        // --- ロール追従 ---
        float targetRoll = target.eulerAngles.z;
        if (targetRoll > 180f) targetRoll -= 360f;
        _currentRoll = Mathf.Lerp(_currentRoll, targetRoll * rollFollowStrength, Time.deltaTime * rollSmoothness);

        Quaternion rollQuat = Quaternion.Euler(0f, 0f, _currentRoll);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot * rollQuat, Time.deltaTime * rotationSmoothness);
    }

    private void DistanceControl()
    {

    }
}
