using UnityEngine;
using UnityEngine.Rendering; // Volume�֘A�ɕK�v
using UnityEngine.Rendering.Universal; // URP�̏ꍇ (�r���g�C���Ȃ�s�v)

public class MotionBlurController : MonoBehaviour,IVehicleReceiver
{
    private Rigidbody machineRigidbody;
    public Volume postProcessVolume;
    public float maxSpeed = 50f; // 50m/s (180 km/h)

    [Tooltip("�u���[�̍ő勭�x")]
    public float maxIntensity = 0.5f;

    private MotionBlur motionBlur;

    public void Receipt(GameObject vehicle, Rigidbody rigidbody)
    {
        machineRigidbody = rigidbody;
    }

    void Start()
    {
        // Volume�v���t�@�C������MotionBlur�̐ݒ���擾
        if (postProcessVolume.profile.TryGet(out motionBlur))
        {
            // �����l��0�ɐݒ�
            motionBlur.intensity.value = 0f;
        }
        else
        {
            Debug.LogError("Volume Profile��Motion Blur������܂���I");
            enabled = false;
        }
    }

    void Update()
    {
        if (motionBlur == null||machineRigidbody == null) return;

        float currentSpeed = machineRigidbody.linearVelocity.magnitude;
        float speedRatio = Mathf.Clamp01(currentSpeed / maxSpeed);

        // ���x�̊����ɉ����ău���[���x��0����maxIntensity�̊ԂŕύX
        motionBlur.intensity.value = Mathf.Lerp(0f, maxIntensity, speedRatio);
    }
}