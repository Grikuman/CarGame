using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RacingChaseCamera : MonoBehaviour
{
    public Transform target; // 車のTransform
    public Vector3 baseOffset = new Vector3(0f, 3.5f, -7f);
    public float smoothTime = 0.15f;
    public float lookAheadDistance = 6f;
    public float lookAheadSpeedFactor = 0.5f; // 速度に対する先読みの強さ
    public float fovMin = 60f;
    public float fovMax = 85f;
    public float maxSpeedForFov = 80f;
    public float fovLerpSpeed = 2f;
    public bool useLocalOffset = true; // 車の向きに追従するオフセット

    private Vector3 currentVelocity;
    private Camera cam;
    private Vector3 lastTargetPos;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (target != null) lastTargetPos = target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 速度（簡易）
        Vector3 vel = (target.position - lastTargetPos) / Time.deltaTime;
        float speed = vel.magnitude;
        lastTargetPos = target.position;

        // 先読み（進行方向にオフセット）
        Vector3 forward = useLocalOffset ? target.forward : Vector3.forward;
        Vector3 lookAhead = forward * (lookAheadDistance * Mathf.Clamp01(speed / (maxSpeedForFov * lookAheadSpeedFactor)));

        // 目標位置（ターゲット位置 + baseOffset in target space）
        Vector3 desiredPos = target.position + (useLocalOffset ? target.TransformVector(baseOffset) : baseOffset) + lookAhead;

        // スムーズ移動
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref currentVelocity, smoothTime);

        // 追跡方向をターゲットに向ける（少し先を向く）
        Vector3 lookPoint = target.position + lookAhead * 0.5f;
        Quaternion desiredRot = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, 1f - Mathf.Exp(-10f * Time.deltaTime));

        // FOV制御
        float targetFov = Mathf.Lerp(fovMin, fovMax, Mathf.Clamp01(speed / maxSpeedForFov));
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * fovLerpSpeed);
    }
}
