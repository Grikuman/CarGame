using UnityEngine;
using System.Collections.Generic;

public class CarMove : MonoBehaviour
{
    // �Ԏ����i�O�ցE��ւ̐ݒ�Ȃǁj���܂Ƃ߂����X�g
    public List<AxleInfo> axleInfos;

    // ���[�^�[�ɂ��ő�g���N�i�O�i�E��ނ̋����j
    public float maxMotorTorque = 1500f;

    // �ő呀�Ǌp�i�n���h�����ǂꂾ���؂�邩�j
    public float maxSteeringAngle = 30f;

    // �h���t�g���̃��A�O���b�v�i0�ɋ߂��قǊ���₷���j
    public float rearGrip = 0.5f;

    // �h���t�g�����Shift�L�[���g����
    public bool useShiftForDrift = true;

    void Start()
    {
        // �Ԃ̏d�S�����������āA���]���ɂ�������
        GetComponent<Rigidbody>().centerOfMass += Vector3.down * 0.5f;
    }

    void FixedUpdate()
    {
        // �O��ړ��̓��́iW/S�L�[�Ȃǁj�ɉ����ăg���N���v�Z
        float motor = maxMotorTorque * Input.GetAxis("Vertical");

        // ���E�ړ��̓��́iA/D�L�[�Ȃǁj�ɉ����đ��Ǌp���v�Z
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        // Shift�L�[��������Ă���΃h���t�g��ԂƂ���i1.0�j�A������Ă��Ȃ���Βʏ�i0.0�j
        float driftInput = (useShiftForDrift && Input.GetKey(KeyCode.LeftShift)) ? 1f : 0f;

        // �h���t�g���͂ɉ����ă��A�O���b�v�̕␳�l���v�Z�i1���ʏ�O���b�v�ArearGrip���h���t�g�j
        float rearGripFactor = Mathf.Lerp(1f, rearGrip, driftInput);

        // �e�A�N�X���i�Ԏ��j���Ƃɏ���
        foreach (AxleInfo axleInfo in axleInfos)
        {
            // �n���h�����L���ȎԎ��Ȃ瑀�Ǌp��ݒ�
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            // �쓮���L���ȎԎ��Ȃ�g���N��ݒ�
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            // ��ւȂ�A�h���t�g�p�ɉ��O���b�v�𒲐�
            if (axleInfo.isRear)
            {
                SetRearGrip(axleInfo, rearGripFactor);
            }
        }
    }

    // ���A�^�C���̉��O���b�v�istiffness�j��ݒ�
    void SetRearGrip(AxleInfo axleInfo, float gripFactor)
    {
        WheelFrictionCurve friction;

        // ����ւ̉��O���b�v�𒲐�
        friction = axleInfo.leftWheel.sidewaysFriction;
        friction.stiffness = gripFactor;
        axleInfo.leftWheel.sidewaysFriction = friction;

        // �E��ւ̉��O���b�v�𒲐�
        friction = axleInfo.rightWheel.sidewaysFriction;
        friction.stiffness = gripFactor;
        axleInfo.rightWheel.sidewaysFriction = friction;
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;   // �����̃z�C�[��
    public WheelCollider rightWheel;  // �E���̃z�C�[��
    public bool motor;                // �쓮�͂����Ԏ����ǂ���
    public bool steering;             // �n���h�����삪�\�ȎԎ����ǂ���
    public bool isRear;               // ��ւ��ǂ����i�O���b�v�����Ɏg�p�j
}
