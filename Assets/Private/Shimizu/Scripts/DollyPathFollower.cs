using Cinemachine;
using UnityEngine;

public class DollyPathFollower : MonoBehaviour
{
   

    [SerializeField] private Transform _Target;
    [SerializeField] private float _offset;

    [SerializeField] private int _stepsPerSegment;

    private CinemachineDollyCart _cinemachineDollyCart = null;
    public int _startSegment;
    public int _endSegment;


    private void Start()
    {
        _cinemachineDollyCart = this.GetComponent<CinemachineDollyCart>();
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = _Target.position;

        Vector3 direction = -_Target.forward.normalized;

        targetPosition = targetPosition + (direction * _offset);

        // ���݂̃J�����̈ʒu���擾
        int currentT = Mathf.FloorToInt(_cinemachineDollyCart.m_Position);

        // �J�n�|�C���g�ԍ�
        _startSegment = currentT - 3;
        // �I���|�C���g�ԍ�
        _endSegment = currentT + 3;

        // �����x�[�X�ōł��߂��ʒu�� t ���擾
        float t = _cinemachineDollyCart.m_Path.FindClosestPoint(targetPosition, _startSegment, _endSegment, _stepsPerSegment);

        _cinemachineDollyCart.m_Position = t;
    }

    
}
