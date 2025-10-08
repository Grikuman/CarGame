using Cinemachine;
using Fusion.Editor;
using UnityEngine;

public class FollowTargetWithOffset : MonoBehaviour
{
    private CinemachineDollyCart _cinemachineDollyCart = null;

    [SerializeField] private Rigidbody _TargetRigidbody = null;
    [SerializeField] private float _reductionRate = 1.0f;

    private void Start()
    {
        _cinemachineDollyCart = this.GetComponent<CinemachineDollyCart>();
    }

    private void FixedUpdate()
    {
        float speed = _TargetRigidbody.linearVelocity.magnitude;


        _cinemachineDollyCart.m_Speed = speed;
    }
}
