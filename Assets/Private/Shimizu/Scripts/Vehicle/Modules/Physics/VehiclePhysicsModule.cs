using UnityEngine;
using UnityEngine.UIElements;

public class VehiclePhysicsModule : IVehicleModule, IResettableVehicleModule<EngineSettings>
{

    public float RecoverPower { get; set; }
    public float RayLength { get; set; }

    public bool IsGrounded { get; set; }
    public Vector3 GroundNormal { get; set; }

    public bool IsHover { get; set; }
    public float HoverHeight { get; set; }
    public float HoverForce { get; set; }
    public float Damping { get; set; }

    public float RotationSpeed { get; set; }

    public LayerMask LayerMask { get; set; }

    // 重力制御
    public GravityAlignment _gravityAlignment { get; private set; } = null;
    // ホバー制御
    public HoverBoard _hoverBoard { get; private set; } = null;
    // 姿勢制御
    public OrientationStabilizer _orientationStabilizer { get; private set; } = null;

    private Transform _transform = null;


    private void OnDrawGizmos()
    {
        float length = 5f;

        if (_gravityAlignment == null) return;

        if (IsGrounded)
        {
            Debug.DrawRay(_transform.position, GroundNormal * length,Color.red);
        }
    }


    private bool _isActive = true;
    private VehicleController _vehicleController = null;

    /// <summary> アクティブ状態を設定 </summary>
    public void SetActive(bool value) => _isActive = value;
    /// <summary> アクティブ状態を取得 </summary>
    public bool GetIsActive() => _isActive;

    /// <summary> 初期化処理 </summary>
    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;

        _transform = vehicleController.gameObject.transform;

        // 各制御作成
        _gravityAlignment = new GravityAlignment(_vehicleController.gameObject.GetComponent<Rigidbody>());
        _hoverBoard = new HoverBoard(_transform , this);
        _orientationStabilizer = new OrientationStabilizer(_transform , this);

        // 重力の設定値を更新
        _gravityAlignment._rayLength = RayLength;
        _gravityAlignment._layerMask = LayerMask;

        // ホバーの設定値を更新
        _hoverBoard.hoverHeight = HoverHeight;
        _hoverBoard.hoverForce = HoverForce;
        _hoverBoard.isHover = IsHover;
        _hoverBoard.layerMask = LayerMask;

        // 姿勢制御の設定値を更新
        _orientationStabilizer.rotationSpeed = RotationSpeed;
    }
    /// <summary> 更新処理 </summary>
    public void UpdateModule()
    {
        Debug.Log("Update Engine Module");

        this.OnDrawGizmos();
    }
    /// <summary> 物理計算更新処理 </summary>
    public void FixedUpdateModule()
    {
        // 各制御の更新処理
        _gravityAlignment.UpdateGravity();
        _hoverBoard.UpdateHoverForce();
        _orientationStabilizer.UpdateStabilizer();

        // 地面に関する値を取得する
        GroundNormal = _gravityAlignment._groundNormal;
        IsGrounded   = _gravityAlignment._isGrounded;
    }

    // リセット時の処理
    public void ResetModule(EngineSettings settings)
    {
        Debug.Log("Reset Engine Settings");

        RecoverPower  = RecoverPower;
        RayLength     = RayLength;
        IsGrounded    = IsGrounded;
        GroundNormal  = GroundNormal;
        IsHover       = IsHover;
        HoverHeight   = HoverHeight;
        HoverForce    = HoverForce;
        Damping       = Damping;
        RotationSpeed = RotationSpeed;
        LayerMask     = LayerMask;

        // 重力の設定値を更新
        _gravityAlignment._rayLength = RayLength;
        _gravityAlignment._layerMask = LayerMask;

        // ホバーの設定値を更新
        _hoverBoard.hoverHeight = HoverHeight;
        _hoverBoard.hoverForce = HoverForce;
        _hoverBoard.isHover = IsHover;
        _hoverBoard.layerMask = LayerMask;

        // 姿勢制御の設定値を更新
        _orientationStabilizer.rotationSpeed = RotationSpeed;
    }

}
