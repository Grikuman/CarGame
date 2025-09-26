using UnityEngine;
using UnityEngine.UIElements;

public class VehiclePhysicsModule : MonoBehaviour
{
    [Header("重力設定")]

    [SerializeField] private float _recoverPower;
    [SerializeField] private float _rayLength;

    [Space][Space]

    [SerializeField] private bool _isGrounded;
    [SerializeField] public Vector3 _groundNormal;

    [Header("ホバー設定")]
    [SerializeField] private bool _isHover;
    [SerializeField] private float _hoverHeight;
    [SerializeField] private float _hoverForce;
    [SerializeField] private float _damping;


    [Header("姿勢制御設定")]
    [SerializeField] private float _rotationSpeed;


    [Header("共通設定")]
    [SerializeField] private LayerMask _layerMask;

    // 重力制御
    public GravityAlignment _gravityAlignment { get; private set; } = null;
    // ホバー制御
    public HoverBoard _hoverBoard { get; private set; } = null;
    // 姿勢制御
    public OrientationStabilizer _orientationStabilizer { get; private set; } = null;

    public float _input { get; set; } = 0.0f;


    // 初期化処理
    void Start()
    {
        // 各制御作成
        _gravityAlignment      = new GravityAlignment(GetComponent<Rigidbody>());
        _hoverBoard            = new HoverBoard(transform);
        _orientationStabilizer = new OrientationStabilizer(transform);

        // 重力の設定値を更新
        _gravityAlignment._rayLength = _rayLength;
        _gravityAlignment._layerMask = _layerMask;

        // ホバーの設定値を更新
        _hoverBoard.hoverHeight = _hoverHeight;
        _hoverBoard.hoverForce  = _hoverForce;
        _hoverBoard.isHover     = _isHover;
        _hoverBoard.layerMask   = _layerMask;

        // 姿勢制御の設定値を更新
        _orientationStabilizer.rotationSpeed = _rotationSpeed;
    }


    // 固定更新処理
    private void FixedUpdate()
    {
        // インスペクター上で変更した値を更新
#if DEBUG
        // 重力の設定値を更新
        _gravityAlignment._rayLength = _rayLength;
        _gravityAlignment._layerMask = _layerMask;

        // ホバーの設定値を更新
        _hoverBoard.hoverHeight = _hoverHeight;
        _hoverBoard.hoverForce  = _hoverForce;
        _hoverBoard.isHover     = _isHover;
        _hoverBoard.layerMask   = _layerMask;

        // 姿勢制御の設定値を更新
        _orientationStabilizer.rotationSpeed = _rotationSpeed;
#endif

        // 各制御の更新処理
        _gravityAlignment.UpdateGravity();
        _hoverBoard.UpdateHoverForce();
        _orientationStabilizer.UpdateStabilizer();

        this.DebugHandle(_input);


        // 地面に関する値を取得する
        _groundNormal = _gravityAlignment._groundNormal;
        _isGrounded   = _gravityAlignment._isGrounded;
    }

    public void DebugHandle(float input)
    {
        Vector3 groundUp = _groundNormal;

        // 地面法線を軸にしてヨー回転（ハンドル入力）
        Quaternion turnRot = Quaternion.AngleAxis(
            input * 100.0f * Time.fixedDeltaTime,
            groundUp
        );

        // 現在の回転に加算
        transform.rotation = turnRot * transform.rotation;
    }

    private void OnDrawGizmos()
    {
        float length = 5f;

        if (_gravityAlignment == null) return;

        if (_isGrounded)
        {
            // ギズモの色設定
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _groundNormal * length);
        }
    }

}
