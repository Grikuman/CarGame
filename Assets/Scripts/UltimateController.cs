using UnityEngine;

public class UltimateController : MonoBehaviour
{
    // �A���e�B���b�g�̎��
    public enum UltimateType
    {
        Blitzray,
        Parasite,
        Empress
    }

    [Header("�A���e�B���b�g�̎�ނ�ݒ�")]
    [SerializeField] private UltimateType m_ultType = UltimateType.Blitzray;

    [Header("�A���e�B���b�g�Q�[�W�֘A")]
    [SerializeField] private float m_ultGauge = 0.0f; �@�@�@// ���݂̃Q�[�W
    [SerializeField] private float m_maxGauge = 100.0f;     // �Q�[�W�ő�l
    [SerializeField] private float m_gaugeGainRate = 10.0f; // �u�[�X�g���̃Q�[�W���܂�₷��

    [Header("���")]
    [SerializeField] private bool m_canActivateUltimate = false; // �A���e�B���b�g�𔭓��\��
    [SerializeField] private bool m_activateUltimate = false;  // �A���e�B���b�g�g�p��
    // �u�[�X�g�X�N���v�g�ւ̎Q��
    private KartBoostController m_boost;

    void Start()
    {
        // Boost�N���X���擾
        m_boost = GetComponent<KartBoostController>();
    }

    void Update()
    {
        // Boost��GetUseBoost()��true�̂Ƃ��Q�[�W�����߂�
        if (m_boost != null && m_boost.GetIsBoost() && m_ultGauge < m_maxGauge)
        {
            // �A���e�B���b�g�Q�[�W�ɉ��Z����
            m_ultGauge += m_gaugeGainRate * Time.deltaTime;

            // �A���e�B���b�g�Q�[�W��100�𒴂��Ȃ��悤�ɐ�������
            // �������i�K�ŃA���e�B���b�g�����\��Ԃɂ���
            if (m_ultGauge >= m_maxGauge)
            {
                m_ultGauge = m_maxGauge;
                m_canActivateUltimate = true;
            }
        }

        // �A���e�B���b�g�����\��Ԃ̂Ƃ��ɔ������͂������
        if (m_canActivateUltimate && Input.GetKeyDown(KeyCode.E))
        {
            // �A���e�B���b�g����
            ActivateUltimate();
        }
        // �A���e�B���b�g�������Ȃ�Ίe�������s��
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
        // �ݒ肳��Ă����ނ̃A���e�B���b�g����������
        switch (m_ultType)
        {
            case UltimateType.Blitzray:
                Debug.Log("Blitzray �����F������");
                // TODO: ��������

                break;

            case UltimateType.Parasite:
                Debug.Log("Parasite �����F�u�[�X�g�z��");
                // TODO: ���͂̓G�̃u�[�X�g�Q�[�W�����炵�Ď����ɉ��Z

                break;

            case UltimateType.Empress:
                Debug.Log("EMPRESS �����F�X�^��");
                // TODO: ���͈͓��̓G���X�^����Ԃɂ���

                break;
        }

        // �A���e�B���b�g�g�p��̓��Z�b�g����
        //m_ultGauge = 0.0f;
        //m_canUseUltimate = false;
    }


    // ���݂̃A���e�B���b�g�Q�[�W�������擾����
    public float GetGaugePercent()
    {
        return m_ultGauge / m_maxGauge;
    }
}
