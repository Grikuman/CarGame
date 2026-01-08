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
    /// ƒJƒƒ‰‚ğ—h‚ç‚·
    /// </summary>
    /// <param name="velocity">—h‚ê‚éŒü‚«‘¬‚³</param>
    /// <param name="t">ŠÔ</param>
    public void Shake(Vector3 velocity , float t = 0.2f)
    {
        if (_impulseSource == null) return;

        // ’l‚Ìİ’è
        _impulseSource.m_ImpulseDefinition.m_ImpulseDuration = t;
        _impulseSource.m_DefaultVelocity = velocity;


        // —h‚ê‚éˆ—‚ğÀs
        _impulseSource.GenerateImpulse();
    }
}
