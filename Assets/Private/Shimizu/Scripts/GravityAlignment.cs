using UnityEngine;

public class GravityAlignment
{
    private static Vector3 RECOVER_RIGHT_RAY_DIRECTION = Quaternion.Euler(0, 0, 135) * Vector3.up;
    private static Vector3 RECOVER_LEFT_RAY_DIRECTION  = Quaternion.Euler(0, 0, -135) * Vector3.up;


    // �n�ʂ�T������
    public float _rayLength { get; set; } = 20.0f;
    // �R�[�X�ɖ߂���
    public float _recoverPower { get; set; } = 2.0f;
    // �n�ʃ��C���[�w��i�K�v�ɉ����āj
    public LayerMask _layerMask { get; set; }


    // �n�ʂɐڂ��Ă��邩
    public bool _isGrounded { get; private set; } = false;
    // �n�ʂ̖@���i�����l�͏�����j
    public Vector3 _groundNormal { get;  set; } = Vector3.up;

    // �d�͂̃A�N�e�B�u�ݒ�
    public bool _isGravity { get; set; } = true;


    private Rigidbody _rb        = null;
    private Transform _transform = null;
    private Vector3 _currentDirection = Vector3.zero;

    public GravityAlignment(Rigidbody rb)
    {
        _rb = rb;
        _transform = rb.transform;
    }

    // �n�ʂ����m
    public void UpdateGravity()
    {
        RaycastHit hit;

        // �������Ƀ��C���΂�
        if (Physics.Raycast(_transform.position, -_transform.up, out hit, _rayLength, _layerMask))
        {
            _isGrounded = true;
            // �n�ʂ̌������擾
            _groundNormal = -hit.normal;

            // �O�t���[���Ƃ̒n�ʂƂ̌������r
            if(_currentDirection != _groundNormal)
            {
                // this.ResetVerticalVelocity(-_groundNormal);
            }

            if(_isGravity)
            _rb.linearVelocity += _groundNormal * 30.81f * Time.fixedDeltaTime;

            // ���݂̒n�ʂ̌�����ۑ�
            _currentDirection = _groundNormal;
        }
        else
        {
            // �R�[�X�ɖ߂�����
           this.RecoverOnTrack();
        }
    }


    private void ResetVerticalVelocity(Vector3 newGravityDirection)
    {
        // ���݂̑��x
        Vector3 velocity = _rb.linearVelocity;

        // �V�����d�͕����i���K���j
        Vector3 gravityDir = newGravityDirection.normalized;

        // �d�͕����̑��x����
        float verticalSpeed = Vector3.Dot(velocity, gravityDir);

        // �d�͕����̑��x����������
        Vector3 adjustedVelocity = velocity - gravityDir * verticalSpeed;

        // �C����̑��x����
        _rb.linearVelocity = adjustedVelocity;

        Debug.Log("�O�̏d�͕����̑��x���������܂����B");
    }

    private void RecoverOnTrack()
    {
        Debug.Log("���J�o�[��");

        RaycastHit hit;
        Vector3 direction = Vector3.down;

        // ���΂߉��Ƀ��C���΂�
        if (Physics.Raycast(_transform.position, RECOVER_LEFT_RAY_DIRECTION, out hit, _rayLength))
        {
            Vector3 origin = _transform.position;
            // �q�b�g�n�_�ւ̕����x�N�g��
            direction = (hit.point - origin).normalized;

            direction = new Vector3(direction.x * _recoverPower, direction.y , direction.z * _recoverPower);
        }
        else
        {
            // �E�΂߉��Ƀ��C���΂�
            if(Physics.Raycast(_transform.position, RECOVER_RIGHT_RAY_DIRECTION, out hit, _rayLength))
            {
                Vector3 origin = _transform.position;
                // �q�b�g�n�_�ւ̕����x�N�g��
                direction = (hit.point - origin).normalized;

                direction = new Vector3(direction.x * _recoverPower, direction.y, direction.z * _recoverPower);
            }
        }

        _rb.linearVelocity += direction * 9.81f * Time.fixedDeltaTime;
    }
}
