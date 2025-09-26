using UnityEngine;

public class OrientationStabilizer
{
    // ��ԑ��x
    public float rotationSpeed { get; set; } = 5.0f;

    private Transform _transform = null;
    private VehiclePhysicsModule _vehiclePhysicsModule = null;
   
    // �R���X�g���N�^
    public OrientationStabilizer(Transform transform)
    {
        _transform = transform;
        _vehiclePhysicsModule = transform.GetComponent<VehiclePhysicsModule>();
    }


    // �p������X�V����
    public void UpdateStabilizer()
    {
        Vector3 groundUp = -_vehiclePhysicsModule._gravityAlignment._groundNormal;

        // �n�ʖ@���ɍ��킹�ď������␳
        Quaternion rotationToGround = Quaternion.FromToRotation(_transform.up, groundUp);
        Quaternion targetRotation = rotationToGround * _transform.rotation;

        _transform.rotation = Quaternion.Slerp(
            _transform.rotation,
            targetRotation,
            Time.fixedDeltaTime * rotationSpeed
        );
    }
}
