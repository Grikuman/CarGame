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

        // 距離ベースで最も近い位置の t を取得
        float t = _cinemachineDollyCart.m_Path.FindClosestPoint(targetPosition, 0, 5,1000);

        _cinemachineDollyCart.m_Position = t; 
    }
}
