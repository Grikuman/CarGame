using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // �Ԃ�Transform
    public Vector3 offset = new Vector3(0, 5, -10); // �Ԃ���̑��Έʒu
    public float followSpeed = 5f; // �Ǐ]�̊��炩��

    void FixedUpdate()
    {
        // �ڕW�ʒu���v�Z
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // ��Ԃ��Ċ��炩�ɒǂ�
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // �Ԃ̕���������
        transform.LookAt(target);
    }
}
