using Cinemachine;
using UnityEngine;

public class FollowTargetWithOffset : MonoBehaviour
{
    private CinemachineDollyCart _cinemachineDollyCart = null;

    [SerializeField] private Transform _Target = null;
    [SerializeField] private float _offset;

    private void Start()
    {
        Application.targetFrameRate = 60;
        _cinemachineDollyCart = this.GetComponent<CinemachineDollyCart>();
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = _Target.position;

        Vector3 direction = -_Target.forward.normalized;

        targetPosition = targetPosition + (direction * _offset);

        // �����x�[�X�ōł��߂��ʒu�� t ���擾
        float t = _cinemachineDollyCart.m_Path.FindClosestPoint(targetPosition, 0, 5,1000);

        _cinemachineDollyCart.m_Position = t; 
    }
}
