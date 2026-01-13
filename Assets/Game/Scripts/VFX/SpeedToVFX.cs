using UnityEngine;
using UnityEngine.VFX;

public class SpeedToVFX : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float maxSpeed = 100f;

    void Update()
    {
        float speed = rb.linearVelocity.magnitude;

        // 0〜1 に正規化
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);

        // VFX に値を送る
        vfx.SetFloat("Speed", normalizedSpeed);
    }
}
