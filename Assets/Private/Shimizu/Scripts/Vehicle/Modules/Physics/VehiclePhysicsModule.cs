using UnityEngine;

public class VehiclePhysicsModule : IVehicleModule, IResettableVehicleModule<VehiclePhysicsModuleData>
{

    public float RecoverPower { get; set; }
    public float RayLength { get; set; }

    public bool IsGrounded { get; private set; }
    public Vector3 GroundNormal { get; private set; }

    public bool IsHover { get; set; }
    public float HoverHeight { get; set; }
    public float HoverForce { get; set; }
    public float Damping { get; set; }

    public float RotationSpeed { get; set; }

    public LayerMask LayerMask { get; set; }

    // �d�͐���
    public GravityAlignment _gravityAlignment { get; private set; } = null;
    // �z�o�[����
    public HoverBoard _hoverBoard { get; private set; } = null;
    // �p������
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

    /// <summary> �A�N�e�B�u��Ԃ�ݒ� </summary>
    public void SetActive(bool value) => _isActive = value;
    /// <summary> �A�N�e�B�u��Ԃ��擾 </summary>
    public bool GetIsActive() => _isActive;

    /// <summary> ���������� </summary>
    public void Initialize(VehicleController vehicleController)
    {
        _vehicleController = vehicleController;

        _transform = vehicleController.gameObject.transform;

        // �e����쐬
        _gravityAlignment = new GravityAlignment(_vehicleController.gameObject.GetComponent<Rigidbody>());
        _hoverBoard = new HoverBoard(_transform , this);
        _orientationStabilizer = new OrientationStabilizer(_transform , this);

        // �d�͂̐ݒ�l���X�V
        _gravityAlignment._rayLength = RayLength;
        _gravityAlignment._layerMask = LayerMask;

        // �z�o�[�̐ݒ�l���X�V
        _hoverBoard.hoverHeight = HoverHeight;
        _hoverBoard.hoverForce = HoverForce;
        _hoverBoard.isHover = IsHover;
        _hoverBoard.layerMask = LayerMask;

        // �p������̐ݒ�l���X�V
        _orientationStabilizer.rotationSpeed = RotationSpeed;

     
    }
    /// <summary> �J�n���� </summary>
    public void Start()
    {
        Debug.Log("Start Vehicle Physics Module");
    }
    /// <summary> �X�V���� </summary>
    public void UpdateModule()
    {
        Debug.Log("Update Vehicle Physics Module");

        this.OnDrawGizmos();
    }
    /// <summary> �����v�Z�X�V���� </summary>
    public void FixedUpdateModule()
    {
        Debug.Log("Fixed Update Vehicle Physics Module");

        // �e����̍X�V����
        _gravityAlignment.UpdateGravity();
        _hoverBoard.UpdateHoverForce();
        _orientationStabilizer.UpdateStabilizer();

        // �n�ʂɊւ���l���擾����
        GroundNormal = _gravityAlignment._groundNormal;
        IsGrounded   = _gravityAlignment._isGrounded;
    }

    // ���Z�b�g���̏���
    public void ResetModule(VehiclePhysicsModuleData data)
    {
        Debug.Log("Reset Vehicle Physics Module Data");

        RecoverPower  = data.RecoverPower;
        RayLength     = data.RayLength;
        
        IsHover       = data.IsHover;
        HoverHeight   = data.HoverHeight;
        HoverForce    = data.HoverForce;
        Damping       = data.Damping;

        RotationSpeed = data.RotationSpeed;

        LayerMask     = data.LayerMask;

        // �d�͂̐ݒ�l���X�V
        _gravityAlignment._rayLength = RayLength;
        _gravityAlignment._layerMask = LayerMask;

        // �z�o�[�̐ݒ�l���X�V
        _hoverBoard.hoverHeight = HoverHeight;
        _hoverBoard.hoverForce = HoverForce;
        _hoverBoard.isHover = IsHover;
        _hoverBoard.layerMask = LayerMask;

        // �p������̐ݒ�l���X�V
        _orientationStabilizer.rotationSpeed = RotationSpeed;
    }

}
