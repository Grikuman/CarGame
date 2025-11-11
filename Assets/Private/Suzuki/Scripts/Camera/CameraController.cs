using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
    [Header("ターゲット設定")]
    public Transform target;
    public Rigidbody targetRb;

    [Header("基本オフセット")]
    public Vector3 baseOffset = new Vector3(0, 3.5f, -5f);

    [Header("スピード連動ズーム")]
    public float maxBack = -3f;
    public float speedToMax = 200f;
    public float zoomSmooth = 3f;

    [Header("コーナースライド")]
    public float slideAmount = 2f;
    public float slideSmooth = 3f;

    [Header("カメラバンク（ロール）")]
    public float bankAmount = 5f;
    public float bankSmooth = 3f;

    private CinemachineVirtualCamera vcam;
    private CinemachineTransposer transposer;
    private CinemachineComposer composer;
    private Vector3 currentOffset;
    private float currentBank;

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        composer = vcam.GetCinemachineComponent<CinemachineComposer>();
        currentOffset = baseOffset;
    }

    void LateUpdate()
    {
        if (target == null || targetRb == null || transposer == null) return;

        // 速度によるズームアウト
        float speed = targetRb.linearVelocity.magnitude;
        float t = Mathf.Clamp01(speed / speedToMax);
        float targetZ = Mathf.Lerp(baseOffset.z, baseOffset.z + maxBack, t);

        // 横滑り量を計算
        Vector3 localVel = target.InverseTransformDirection(targetRb.linearVelocity);
        float lateral = Mathf.Clamp(localVel.x / 10f, -1f, 1f);
        float targetX = baseOffset.x - lateral * slideAmount;

        // オフセットをスムーズ更新
        currentOffset.x = Mathf.Lerp(currentOffset.x, targetX, Time.deltaTime * slideSmooth);
        currentOffset.z = Mathf.Lerp(currentOffset.z, targetZ, Time.deltaTime * zoomSmooth);
        currentOffset.y = Mathf.Lerp(currentOffset.y, baseOffset.y, Time.deltaTime * zoomSmooth);

        transposer.m_FollowOffset = currentOffset;

        // カメラバンクをCinemachineの"Roll"に適用
#if UNITY_6000_OR_NEWER
        float targetBank = -lateral * bankAmount;
        currentBank = Mathf.Lerp(currentBank, targetBank, Time.deltaTime * bankSmooth);
        composer.m_Dutch = currentBank; // Cinemachine 6.x以降はm_Dutchを直接操作
#endif
    }
}
