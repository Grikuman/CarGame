using UnityEngine;

public class OrientationStabilizer
{
    // 補間速度
    public float rotationSpeed { get; set; } = 5.0f;

    private Transform _transform = null;
    private VehiclePhysicsModule _vehiclePhysicsModule = null;
   
    // コンストラクタ
    public OrientationStabilizer(Transform transform)
    {
        _transform = transform;
        _vehiclePhysicsModule = transform.GetComponent<VehiclePhysicsModule>();
    }


    // 姿勢制御更新処理
    public void UpdateStabilizer()
    {
        Vector3 groundUp = -_vehiclePhysicsModule._gravityAlignment._groundNormal;

        // 地面法線に合わせて上方向を補正
        Quaternion rotationToGround = Quaternion.FromToRotation(_transform.up, groundUp);
        Quaternion targetRotation = rotationToGround * _transform.rotation;

        _transform.rotation = Quaternion.Slerp(
            _transform.rotation,
            targetRotation,
            Time.fixedDeltaTime * rotationSpeed
        );
    }
}
