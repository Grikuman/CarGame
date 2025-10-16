using UnityEngine;

public class UltimateSystem : MonoBehaviour
{
    [Header("�Q�[�W�ݒ�")] 
    [SerializeField] private float m_currentGauge = 0.0f; // ���݂̃A���e�B���b�g�Q�[�W
    [SerializeField] private float m_maxGauge = 100.0f;   // �ő�A���e�B���b�g�Q�[�W
    [SerializeField] private float m_gaugeIncrease = 0.01f; // �Q�[�W������

    private IUltimate currentUltimate;
    private HoverCarController car;

    void Start()
    {
        // �R���|�[�l���g���擾����
        car = GetComponent<HoverCarController>();

        // �Ƃ肠����Boost Ultimate��ݒ肵�Ă���
        currentUltimate = new Ultimate_Boost();
    }

    void Update()
    {
        // �A���e�B���b�g�������Ȃ�X�V
        if (currentUltimate.IsActive())
        {
            currentUltimate.Update();
            // �A���e�B���b�g���I��������
            if (currentUltimate.IsEnd())
            {
                // �A���e�B���b�g�I���������s��
                currentUltimate.End();
                // �Q�[�W�����Z�b�g����
                m_currentGauge = 0.0f;
            }
        }

        // �Q�[�W�l��␳����
        if(m_currentGauge >= m_maxGauge)
        {
            m_currentGauge = m_maxGauge;
        }
    }

    public void TryActivateUltimate()
    {
        // �Q�[�W�����܂��Ă���@���@�A���e�B���b�g����������Ă��Ȃ��ꍇ
        if (m_currentGauge >= m_maxGauge && !currentUltimate.IsActive())
        {
            //currentUltimate.Activate(car);
        }
    }

    public void AddUltimateGauge()
    {
        m_currentGauge += m_gaugeIncrease;
    }

    public void SetUltimate(IUltimate ultimate)
    {
        currentUltimate = ultimate;
    }

    public bool IsActiveUltimate()
    {
        if (currentUltimate.IsActive())
        {  
            return true;
        }
        else
        {
            return false;
        }
    }

    // UI�p�F���݂̃Q�[�W�������ŕԂ�
    public float GetUltimateGaugeNormalized()
    {
        return m_currentGauge / m_maxGauge;
    }
}
