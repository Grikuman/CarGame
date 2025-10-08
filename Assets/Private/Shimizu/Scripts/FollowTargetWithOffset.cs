using UnityEngine;

/// <summary>
/// �^�[�Q�b�g�I�u�W�F�N�g��Ǐ]����X�N���v�g�i�I�t�Z�b�g�t���j
/// </summary>
public class FollowTargetWithOffset : MonoBehaviour
{
    [Header("�Ǐ]�Ώ�")]
    public Transform target; // �Ǐ]�������I�u�W�F�N�g�i�v���C���[�Ȃǁj

    [Header("�Ǐ]�I�t�Z�b�g")]
    public Vector3 offset = new Vector3(0f, 5f, -10f); // ���[���h��Ԃł̈ʒu����

    [Header("��Ԑݒ�")]
    public bool useLerp = true;        // ���`��Ԃ��g����
    public float followSpeed = 5f;     // Lerp �̃X�s�[�h

    void LateUpdate()
    {
        if (target == null) return; // �^�[�Q�b�g�����ݒ�Ȃ牽�����Ȃ�

        // �^�[�Q�b�g�ʒu + �I�t�Z�b�g
        Vector3 desiredPosition = target.position + offset;

        if (useLerp)
        {
            // ���`��ԁi�X���[�Y�ɒǏ]�j
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            // �����ɒǏ]
            transform.position = desiredPosition;
        }
    }
}
