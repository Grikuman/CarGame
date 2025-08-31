using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // 車のTransform
    public Vector3 offset = new Vector3(0, 5, -10); // 車からの相対位置
    public float followSpeed = 5f; // 追従の滑らかさ

    void FixedUpdate()
    {
        // 目標位置を計算
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // 補間して滑らかに追う
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // 車の方向を見る
        transform.LookAt(target);
    }
}
