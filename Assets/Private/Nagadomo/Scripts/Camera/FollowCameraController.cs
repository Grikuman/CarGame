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
    public float maxZDistance = -5f;

    [Header("FOV設定")]
    public float defaultFOV = 60f;
    public float boostFOV = 75f;
    public float ultimateFOV = 85f;
    public float fovSmoothSpeed = 5f;

    [Header("ズームアウト設定")]
    public float defaultZOffset = -8f;
    public float boostZOffset = -9.5f;
    public float ultimateZOffset = -10f;
    public float zoomSmoothSpeed = 4f;

    [Header("シェイク設定")]
    public float baseShakeIntensity = 0.03f;   // 通常時の基礎揺れ
    public float maxShakeIntensity = 0.1f;     // 速度最大時の揺れ
    public float boostShakeBonus = 0.05f;      // ブースト時の追加
    public float ultimateShakeBonus = 0.1f;    // アルティメット時の追加
    public float shakeDamping = 6f;            // 減衰スピード

    private Vector3 _velocity;
    private float _currentRoll;
    private Camera _cam;

    private MachineBoostModule _machineBoostModule;
    private MachineUltimateModule _machineUltimateModule;
    private MachineEngineModule _machineEngineModule; // ← 速度参照用

    private Vector3 _shakeOffset = Vector3.zero;

    private void Start()
    {
        _cam = GetComponent<Camera>();
        if (_cam == null)
            _cam = GetComponentInChildren<Camera>();

        var vehicleController = target.GetComponent<VehicleController>();
        if (vehicleController != null)
        {
            _machineBoostModule = vehicleController.Find<MachineBoostModule>();
            _machineUltimateModule = vehicleController.Find<MachineUltimateModule>();
            _machineEngineModule = vehicleController.Find<MachineEngineModule>(); // ← 速度を取る
        }

        if (_cam) _cam.fieldOfView = defaultFOV;
    }

    private void Update()
    {
        FOVControl();
        ZoomControl();
        UpdateShake();
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector3 desiredWorldPos = target.TransformPoint(offset);
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, desiredWorldPos, ref _velocity, followSmoothness);

        Vector3 localPos = target.InverseTransformPoint(smoothedPos);
        if (localPos.z < maxZDistance)
        {
            localPos.z = maxZDistance;
            smoothedPos = target.TransformPoint(localPos);
        }

        transform.position = smoothedPos + _shakeOffset;

        Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

        float targetRoll = target.eulerAngles.z;
        if (targetRoll > 180f) targetRoll -= 360f;
        _currentRoll = Mathf.Lerp(_currentRoll, targetRoll * rollFollowStrength, Time.deltaTime * rollSmoothness);

        Quaternion rollQuat = Quaternion.Euler(0f, 0f, _currentRoll);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot * rollQuat, Time.deltaTime * rotationSmoothness);
    }

    // --- FOV制御 ---
    private void FOVControl()
    {
        if (!_cam) return;

        float targetFOV = defaultFOV;

        if (_machineUltimateModule != null && _machineUltimateModule.IsActiveUltimate())
            targetFOV = ultimateFOV;
        else if (_machineBoostModule != null && _machineBoostModule.IsActiveBoost())
            targetFOV = boostFOV;

        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, targetFOV, Time.deltaTime * fovSmoothSpeed);
    }

    // --- ズーム制御 ---
    private void ZoomControl()
    {
        float targetZ = defaultZOffset;

        if (_machineUltimateModule != null && _machineUltimateModule.IsActiveUltimate())
            targetZ = ultimateZOffset;
        else if (_machineBoostModule != null && _machineBoostModule.IsActiveBoost())
            targetZ = boostZOffset;

        offset.z = Mathf.Lerp(offset.z, targetZ, Time.deltaTime * zoomSmoothSpeed);
    }

    // --- シェイク更新 ---
    private void UpdateShake()
    {
        float currentSpeed = 0.0f;

        // 速度取得（EngineModuleなどから）
        if (_machineEngineModule != null)
            currentSpeed = _machineEngineModule.CurrentSpeed; // ← 実際の速度取得メソッドに合わせて変更

        // 速度を0〜1に正規化
        float speedFactor = Mathf.InverseLerp(0f, 200f, currentSpeed); // 200を最高速に合わせて調整

        // 基本強度
        float targetIntensity = Mathf.Lerp(baseShakeIntensity, maxShakeIntensity, speedFactor);

        // 状態補正
        if (_machineBoostModule != null && _machineBoostModule.IsActiveBoost())
            targetIntensity += boostShakeBonus;

        if (_machineUltimateModule != null && _machineUltimateModule.IsActiveUltimate())
            targetIntensity += ultimateShakeBonus;

        // ランダムノイズ生成
        Vector3 randomShake = Random.insideUnitSphere * targetIntensity;
        randomShake.z = 0f; // 前後方向は抑制

        // 減衰補間
        _shakeOffset = Vector3.Lerp(_shakeOffset, randomShake, Time.deltaTime * shakeDamping);
    }
}
