using UnityEngine;
using UnityEngine.UIElements;

public class VehiclePhysicsModule : MonoBehaviour
{
    [Header("�d�͐ݒ�")]

    [SerializeField] private float _recoverPower;
    [SerializeField] private float _rayLength;

    [Space][Space]

    [SerializeField] private bool _isGrounded;
    [SerializeField] public Vector3 _groundNormal;

    [Header("�z�o�[�ݒ�")]
    [SerializeField] private bool _isHover;
    [SerializeField] private float _hoverHeight;
    [SerializeField] private float _hoverForce;
    [SerializeField] private float _damping;


    [Header("�p������ݒ�")]
    [SerializeField] private float _rotationSpeed;


    [Header("���ʐݒ�")]
    [SerializeField] private LayerMask _layerMask;

    // �d�͐���
    public GravityAlignment _gravityAlignment { get; private set; } = null;
    // �z�o�[����
    public HoverBoard _hoverBoard { get; private set; } = null;
    // �p������
    public OrientationStabilizer _orientationStabilizer { get; private set; } = null;

    public float _input { get; set; } = 0.0f;


    // ����������
    void Start()
    {
        // �e����쐬
        _gravityAlignment      = new GravityAlignment(GetComponent<Rigidbody>());
        _hoverBoard            = new HoverBoard(transform);
        _orientationStabilizer = new OrientationStabilizer(transform);

        // �d�͂̐ݒ�l���X�V
        _gravityAlignment._rayLength = _rayLength;
        _gravityAlignment._layerMask = _layerMask;

        // �z�o�[�̐ݒ�l���X�V
        _hoverBoard.hoverHeight = _hoverHeight;
        _hoverBoard.hoverForce  = _hoverForce;
        _hoverBoard.isHover     = _isHover;
        _hoverBoard.layerMask   = _layerMask;

        // �p������̐ݒ�l���X�V
        _orientationStabilizer.rotationSpeed = _rotationSpeed;
    }


    // �Œ�X�V����
    private void FixedUpdate()
    {
        // �C���X�y�N�^�[��ŕύX�����l���X�V
#if DEBUG
        // �d�͂̐ݒ�l���X�V
        _gravityAlignment._rayLength = _rayLength;
        _gravityAlignment._layerMask = _layerMask;

        // �z�o�[�̐ݒ�l���X�V
        _hoverBoard.hoverHeight = _hoverHeight;
        _hoverBoard.hoverForce  = _hoverForce;
        _hoverBoard.isHover     = _isHover;
        _hoverBoard.layerMask   = _layerMask;

        // �p������̐ݒ�l���X�V
        _orientationStabilizer.rotationSpeed = _rotationSpeed;
#endif

        // �e����̍X�V����
        _gravityAlignment.UpdateGravity();
        _hoverBoard.UpdateHoverForce();
        _orientationStabilizer.UpdateStabilizer();

        this.DebugHandle(_input);


        // �n�ʂɊւ���l���擾����
        _groundNormal = _gravityAlignment._groundNormal;
        _isGrounded   = _gravityAlignment._isGrounded;
    }

    public void DebugHandle(float input)
    {
        Vector3 groundUp = _groundNormal;

        // �n�ʖ@�������ɂ��ă��[��]�i�n���h�����́j
        Quaternion turnRot = Quaternion.AngleAxis(
            input * 100.0f * Time.fixedDeltaTime,
            groundUp
        );

        // ���݂̉�]�ɉ��Z
        transform.rotation = turnRot * transform.rotation;
    }

    private void OnDrawGizmos()
    {
        float length = 5f;

        if (_gravityAlignment == null) return;

        if (_isGrounded)
        {
            // �M�Y���̐F�ݒ�
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _groundNormal * length);
        }
    }

}
