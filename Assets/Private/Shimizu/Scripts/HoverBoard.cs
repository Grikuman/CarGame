using UnityEngine;


public class HoverBoard 
{
    // �z�o�[�A�N�e�B�u�ݒ�
    public bool isHover { get; set; } = true;
    // �z�o�[�̍���
    public float hoverHeight { get; set; } = 2f;
    // �z�o�[�̗�
    public float hoverForce { get; set; } = 200.0f;
    // �_���s���O
    public float damping { get; set; } = 30.0f;

    // ���C���[�}�X�N
    public LayerMask layerMask { get; set; }



    private Transform _transform = null;
    private Rigidbody _rb = null;
    private VehiclePhysicsModule _vehiclePhysicsModule = null;

    // �R���X�g���N�^
    public HoverBoard(Transform transform)
    {
        _transform = transform;
        _rb = transform.GetComponent<Rigidbody>();
        _vehiclePhysicsModule = transform.GetComponent<VehiclePhysicsModule>();
    }


    // �z�o�[����X�V����
    public void UpdateHoverForce()
    {
        if (!isHover) return;

        RaycastHit hit;

        // �������Ƀ��C���΂�
        if (Physics.Raycast(_transform.position, -_transform.up, out hit, hoverHeight * 1.2f , layerMask))
        {
            // �d�͐�����I�t�ɂ���
            _vehiclePhysicsModule._gravityAlignment._isGravity = false;

            // �������擾����
            float distance = hit.distance;

            // ���V�����Ƃ̍����v�Z
            float hoverError = hoverHeight - distance;

            // �n�ʕ����ւ̑��x���擾
            float verticalSpeed = Vector3.Dot(_rb.linearVelocity, hit.normal);

            // �͂��v�Z�i�o�l�� - �����́j
            float force = hoverError * hoverForce - verticalSpeed * damping;

            // ������ɉ�����
            _rb.AddForce(_transform.up * force, ForceMode.Acceleration);
        }
        else
        {
            // �d�͐�����I���ɂ���
            _vehiclePhysicsModule._gravityAlignment._isGravity = true;
        }
    }
}
