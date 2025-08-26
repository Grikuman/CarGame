using UnityEngine;

public class BoostSystem : MonoBehaviour
{
    [Header("�u�[�X�g�ݒ�")]
    public float boostMultiplier = 2.0f;   // �{��
    public float maxBoostGauge = 100f;     // �ő�Q�[�W��
    public float gaugeConsumptionRate = 30f; // 1�b����������
    public float gaugeRecoveryRate = 20f;    // 1�b������񕜗�
    public float boostCooldown = 3.0f;       // �u�[�X�g��̃N�[���_�E��

    [SerializeField ]private float currentGauge;            // ���݂̃Q�[�W
    private float cooldownTimer = 0.0f;    // �N�[���_�E���c�莞��
    private bool isBoosting = false;

    // �R���|�[�l���g
    private HoverCarController car;
    private UltimateSystem m_ultimateSystem;

    void Start()
    {
        car = GetComponent<HoverCarController>();
        m_ultimateSystem = GetComponent<UltimateSystem>();
        currentGauge = maxBoostGauge; // �����͖��^��
    }

    void Update()
    {
        if (isBoosting)
        {
            // �Q�[�W������
            currentGauge -= gaugeConsumptionRate * Time.deltaTime;
            // �A���e�B���b�g�Q�[�W�𒙂߂�
            m_ultimateSystem.AddUltimateGauge();

            if (currentGauge <= 0)
            {
                currentGauge = 0;
                EndBoost();
            }
        }
        else
        {
            // �N�[���_�E�����Ȃ�񕜂��Ȃ�
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                // �Q�[�W����
                if (currentGauge < maxBoostGauge)
                {
                    currentGauge += gaugeRecoveryRate * Time.deltaTime;
                    currentGauge = Mathf.Clamp(currentGauge, 0, maxBoostGauge);
                }
            }
        }
    }

    // �O������Ăԁi���͂Ȃǁj
    public void TryStartBoost()
    {
        if (!isBoosting && currentGauge > 0 && cooldownTimer <= 0)
        {
            StartBoost();
        }
    }

    private void StartBoost()
    {
        car.boostMultiplier = boostMultiplier;
        isBoosting = true;
    }

    private void EndBoost()
    {
        car.boostMultiplier = 1.0f;
        isBoosting = false;
        cooldownTimer = boostCooldown; // �I�����ɃN�[���_�E��
    }

    // UI�p�F���݂̃Q�[�W�������ŕԂ�
    public float GetBoostGaugeNormalized()
    {
        return currentGauge / maxBoostGauge;
    }
}
