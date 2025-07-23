using UnityEngine;
using System.Collections.Generic;

public class CarMove : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque = 3000f;
    public float maxSteeringAngle = 45f;
    public float boostMultiplier = 2.5f;
    public KeyCode boostKey = KeyCode.LeftShift;
    public float brakeTorqueOnRelease = 4000f;   
    public float targetSpeed = 100f;             
    public float accelerationRate = 100f;        

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += Vector3.down * 1.0f;
        rb.linearDamping = 3.5f;          
        rb.angularDamping = 6.0f;         
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        foreach (var axle in axleInfos)
        {
            SetTightFriction(axle.leftWheel);
            SetTightFriction(axle.rightWheel);
        }
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        float finalTargetSpeed = targetSpeed;
        if (Input.GetKey(boostKey))
        {
            finalTargetSpeed *= boostMultiplier;
        }

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 forwardDirection = transform.forward;
        Vector3 desiredVelocity = forwardDirection * moveInput * finalTargetSpeed;

        // 
        rb.linearVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, accelerationRate * Time.fixedDeltaTime);

        float steering = maxSteeringAngle * steerInput;

        foreach (AxleInfo axle in axleInfos)
        {
            if (axle.steering)
            {
                axle.leftWheel.steerAngle = steering;
                axle.rightWheel.steerAngle = steering;
            }

            if (axle.motor)
            {
                // 
                axle.leftWheel.motorTorque = 0f;
                axle.rightWheel.motorTorque = 0f;

                // 
                float brake = Mathf.Approximately(moveInput, 0f) ? brakeTorqueOnRelease : 0f;
                axle.leftWheel.brakeTorque = brake;
                axle.rightWheel.brakeTorque = brake;
            }
        }
    }

    void SetTightFriction(WheelCollider wheel)
    {
        WheelFrictionCurve f = wheel.forwardFriction;
        f.stiffness = 5f;
        wheel.forwardFriction = f;

        f = wheel.sidewaysFriction;
        f.stiffness = 5f;
        wheel.sidewaysFriction = f;
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
