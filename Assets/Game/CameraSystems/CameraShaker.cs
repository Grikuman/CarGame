using Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;

    private void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary>
    /// �J������h�炷
    /// </summary>
    /// <param name="velocity">�h����������</param>
    /// <param name="t">����</param>
    public void Shake(Vector3 velocity , float t = 0.2f)
    {
        if (_impulseSource == null) return;

        // �l�̐ݒ�
        _impulseSource.m_ImpulseDefinition.m_ImpulseDuration = t;
        _impulseSource.m_DefaultVelocity = velocity;


        // �h��鏈�������s
        _impulseSource.GenerateImpulse();
    }
}
