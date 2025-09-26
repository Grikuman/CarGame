using UnityEngine;


public class HoverBoard 
{
    // ホバーアクティブ設定
    public bool isHover { get; set; } = true;
    // ホバーの高さ
    public float hoverHeight { get; set; } = 2f;
    // ホバーの力
    public float hoverForce { get; set; } = 200.0f;
    // ダンピング
    public float damping { get; set; } = 30.0f;

    // レイヤーマスク
    public LayerMask layerMask { get; set; }



    private Transform _transform = null;
    private Rigidbody _rb = null;
    private VehiclePhysicsModule _vehiclePhysicsModule = null;

    // コンストラクタ
    public HoverBoard(Transform transform)
    {
        _transform = transform;
        _rb = transform.GetComponent<Rigidbody>();
        _vehiclePhysicsModule = transform.GetComponent<VehiclePhysicsModule>();
    }


    // ホバー制御更新処理
    public void UpdateHoverForce()
    {
        if (!isHover) return;

        RaycastHit hit;

        // 下方向にレイを飛ばす
        if (Physics.Raycast(_transform.position, -_transform.up, out hit, hoverHeight * 1.2f , layerMask))
        {
            // 重力制御をオフにする
            _vehiclePhysicsModule._gravityAlignment._isGravity = false;

            // 向きを取得する
            float distance = hit.distance;

            // 浮遊距離との差を計算
            float hoverError = hoverHeight - distance;

            // 地面方向への速度を取得
            float verticalSpeed = Vector3.Dot(_rb.linearVelocity, hit.normal);

            // 力を計算（バネ力 - 減衰力）
            float force = hoverError * hoverForce - verticalSpeed * damping;

            // 上方向に加える
            _rb.AddForce(_transform.up * force, ForceMode.Acceleration);
        }
        else
        {
            // 重力制御をオンにする
            _vehiclePhysicsModule._gravityAlignment._isGravity = true;
        }
    }
}
