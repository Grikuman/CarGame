using UnityEngine;

public class VehicleController : MonoBehaviour
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        this.DrawRay();
    }

    private void FixedUpdate()
    {
        Vector3 forward = transform.forward;

        _rb.linearVelocity = forward;
    }

    private void DrawRay()
    {
        Vector3 velocity = _rb.linearVelocity;

        // 各方向のベクトル
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 steeringDirection = Quaternion.Euler(0, 10, 0) * forward;

        // 各方向への速度成分（Dot積）
        float forwardMag  = Vector3.Dot(velocity, forward);
        float rightMag    = Vector3.Dot(velocity, right);
        float steeringMag = Vector3.Dot(velocity, steeringDirection);

        // スケーリング（視覚上の調整）
        float scale = 0.5f;

        // Debug表示（長さは各方向の成分に比例）
        Debug.DrawRay(transform.position, forward * forwardMag * scale, Color.green);    // 前方向
        Debug.DrawRay(transform.position, right * rightMag * scale, Color.red);          // 横方向
        Debug.DrawRay(transform.position, steeringDirection * steeringMag * scale, Color.magenta); // ステア方向
        Debug.DrawRay(transform.position, velocity * scale, Color.blue);                 // 実際の速度ベクトル
    }


}
