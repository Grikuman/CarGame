
using UnityEngine;

public class UltimateController : MonoBehaviour
{
    // �A���e�B���b�g�̎��
    public enum UltimateType
    {
        Blitzray,
        Parasite
    }

    [Header("�A���e�B���b�g�̎�ނ�ݒ�")]
    [SerializeField] private UltimateType m_ultType = UltimateType.Blitzray;

    [Header("�A���e�B���b�g�Q�[�W�֘A")]
    [SerializeField] private float m_ultGauge = 0.0f; �@�@�@ // ���݂̃Q�[�W
    [SerializeField] private float m_maxUltGauge = 100.0f;   // �Q�[�W�ő�l
    [SerializeField] private float m_gaugeGainRate = 10.0f;  // �u�[�X�g���̃Q�[�W���܂�₷��

    [Header("���")]
    [SerializeField] private bool m_canActivateUltimate = false; // �A���e�B���b�g�𔭓��\��
    [SerializeField] private bool m_activateUltimate = false;    // �A���e�B���b�g�g�p��

    [Header("�������A���e�B���b�g�ݒ�")]
    [SerializeField] private float m_boostSpeed = 70.0f;
    [SerializeField] private float m_boostAcceleration = 25.0f;

    //-----------------------------------------------------------------------------------------------

    // �u�[�X�g�R���g���[���[�R���|�[�l���g
    private KartBoostController m_boost;
    // �A�[�P�[�h�J�[�g�R���|�[�l���g

    // �A���e�B���b�g�̉�����ɖ߂��p
    private float originalTopSpeed;     // ���̍ő呬�x(�߂��p�j
    private float originalAcceleration; // ���̉����x(�߂��p�j

    void Start()
    {
        // KartBoostController�R���|�[�l���g���擾
        m_boost = GetComponent<KartBoostController>();
        // ArcadeKart�R���|�[�l���g���擾����
     
    }

    void Update()
    {
        // �A���e�B���b�g�𔭓����Ă��Ȃ��Ƃ��̏���
        if (!m_activateUltimate)
        {
            // Boost��GetUseBoost()��true�̂Ƃ��Q�[�W�����߂�
            if (m_boost != null && m_boost.GetIsBoost() && m_ultGauge < m_maxUltGauge)
            {
                // �A���e�B���b�g�Q�[�W�ɉ��Z����
                m_ultGauge += m_gaugeGainRate * Time.deltaTime;
                // �A���e�B���b�g�Q�[�W���ő�l�𒴂��Ȃ��悤�ɐ�������
                if (m_ultGauge >= m_maxUltGauge)
                {
                    // �ő�l�ɕ␳����
                    m_ultGauge = m_maxUltGauge;
                    // �A���e�B���b�g�����\��Ԃɂ���
                    m_canActivateUltimate = true;
                }
            }

            // �A���e�B���b�g�����\��Ԃ̂Ƃ��ɓ��͂������
            if (m_canActivateUltimate && Input.GetKeyDown(KeyCode.E))
            {
                // �A���e�B���b�g����
                ActivateUltimate();
            }
        }
        
        // �A���e�B���b�g�𔭓����Ă���Ƃ��̏���
        if(m_activateUltimate)
        {
            // �A���e�B���b�g�̏������s��
            Ultimate();
        }
    }

    // �A���e�B���b�g�𔭓�����
    void ActivateUltimate()
    {
        // �A���e�B���b�g�g�p��
        m_activateUltimate = true;
    }

    // �A���e�B���b�g�̏������s��
    void Ultimate()
    {
        // �Q�[�W�������
        m_ultGauge -= m_gaugeGainRate *Time.deltaTime * 5;

        // �ݒ肳��Ă����ނ̃A���e�B���b�g����������
        switch (m_ultType)
        {
            case UltimateType.Blitzray:
                Debug.Log("�����F������");
                // TODO: ��������
                Ultimate_Blitzray();
                break;

            case UltimateType.Parasite:
                Debug.Log("�����F�u�[�X�g�z��");
                // TODO: ���͂̓G�̃u�[�X�g�Q�[�W�����炵�Ď����ɉ��Z

                break;
        }
    }

    // �������A���e�B���b�g�̏���
    private void Ultimate_Blitzray()
    {
     
        // �E���g�Q�[�W�������Ȃ����ꍇ
        if (m_ultGauge <= 0)
        {
            // �X�e�[�^�X��߂�
           
            // �A���e�B���b�g���l��␳����
            m_ultGauge = 0.0f;
            // �󋵂����Z�b�g����
            m_canActivateUltimate = false;
            m_activateUltimate = false;
        }
    }

    // ���݂̃A���e�B���b�g�Q�[�W�������擾����
    public float GetGaugePercent()
    {
        return m_ultGauge / m_maxUltGauge;
    }
}
