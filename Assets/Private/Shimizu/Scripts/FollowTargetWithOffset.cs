using UnityEngine;

/// <summary>
/// ターゲットオブジェクトを追従するスクリプト（オフセット付き）
/// </summary>
public class FollowTargetWithOffset : MonoBehaviour
{
    [Header("追従対象")]
    public Transform target; // 追従したいオブジェクト（プレイヤーなど）

    [Header("追従オフセット")]
    public Vector3 offset = new Vector3(0f, 5f, -10f); // ワールド空間での位置ずれ

    [Header("補間設定")]
    public bool useLerp = true;        // 線形補間を使うか
    public float followSpeed = 5f;     // Lerp のスピード

    void LateUpdate()
    {
        if (target == null) return; // ターゲットが未設定なら何もしない

        // ターゲット位置 + オフセット
        Vector3 desiredPosition = target.position + offset;

        if (useLerp)
        {
            // 線形補間（スムーズに追従）
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            // 即座に追従
            transform.position = desiredPosition;
        }
    }
}
