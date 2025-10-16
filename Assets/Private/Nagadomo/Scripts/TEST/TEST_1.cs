using UnityEngine;

public class TEST_1 : MonoBehaviour
{
    [Header("エンジンの基本設定")]
    [SerializeField] private float _maxThrust = 3000.0f;
    [SerializeField] private float _maxSpeed = 120.0f;
    [SerializeField] private AnimationCurve _thrustCurve;

    [Header("抵抗の設定")]
    [SerializeField] private float _dragCoeff = 0.02f;
    [SerializeField] private float _brakingDrag = 2.0f;

    [Header("質量の設定")]
    [SerializeField] private float _mass = 200.0f;

    [Header("横方向の設定")]
    [SerializeField] private float _lateralGrip = 10.0f;

    [Header("見た目用設定")]
    [SerializeField] private Transform _visualModel;          // 車体モデルへの参照（子オブジェクト）
    [SerializeField] private float _visualRollAngle = 25.0f;  // ドリフト時の最大傾き角度
    [SerializeField] private float _visualRotateSpeed = 5.0f; // 回転補間速度

    public float CurrentSpeed { get; private set; }
    public float InputThrottle { get; set; } = 0.0f;
    public float InputBrake { get; set; } = 0.0f;
    public float InputSteer { get; set; } = 0.0f;

    private Rigidbody _rb;
    private Quaternion _defaultRotation; // 見た目用の初期姿勢

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = _mass;
        _rb.linearDamping = 0.0f;
        _rb.angularDamping = 0.5f;

        if (_visualModel == null)
        {
            Debug.LogWarning("Visual Modelが設定されていません。見た目回転は無効です。");
        }
        else
        {
            _defaultRotation = _visualModel.localRotation;
        }
    }

    private void FixedUpdate()
    {
        UpdateEngine();
        ApplyGrip();
        UpdateVisualRotation();
    }

    private void UpdateEngine()
    {
        CurrentSpeed = _rb.linearVelocity.magnitude;
        float speedFactor = Mathf.Clamp01(CurrentSpeed / _maxSpeed);
        float thrustFactor = _thrustCurve.Evaluate(speedFactor);

        float thrustForce = InputThrottle * _maxThrust * thrustFactor;
        float dragForce = _dragCoeff * CurrentSpeed * CurrentSpeed;
        float brakeForce = InputBrake * _brakingDrag * _mass;

        Vector3 forward = transform.forward;
        Vector3 force = (forward * thrustForce) - (forward * dragForce) - (forward * brakeForce);
        _rb.AddForce(force, ForceMode.Force);
    }

    private void ApplyGrip()
    {
        Vector3 velocity = _rb.linearVelocity;
        Vector3 forward = transform.forward;
        Vector3 forwardVel = Vector3.Project(velocity, forward);
        Vector3 lateralVel = velocity - forwardVel;

        _rb.AddForce(-lateralVel * _lateralGrip, ForceMode.Acceleration);
    }

    private void UpdateVisualRotation()
    {
        if (_visualModel == null) return;

        // 現在速度を 0〜1 の範囲に正規化
        float speedFactor = Mathf.Clamp01(CurrentSpeed / _maxSpeed);

        // 入力と速度に応じて傾きを決定（速いほど強く傾く）
        float targetRoll = InputSteer * _visualRollAngle * speedFactor;

        // 入力がない時はゆっくり元の角度に戻す
        Quaternion targetRot = _defaultRotation * Quaternion.Euler(0, 0, -targetRoll);

        // スムーズに補間
        _visualModel.localRotation = Quaternion.Slerp(
            _visualModel.localRotation,
            targetRot,
            Time.fixedDeltaTime * _visualRotateSpeed
        );
    }

}
