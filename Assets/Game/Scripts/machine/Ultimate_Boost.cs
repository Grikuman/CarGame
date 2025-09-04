using UnityEngine;

public class Ultimate_Boost : IUltimate
{
    private float m_ultimateTime = 3.0f;
    private float m_boostMultiplier = 3.0f;
    private float m_timer;
    private bool m_isActive;
    private bool m_end = false;
    private HoverCarController car;
    public void Activate(HoverCarController car)
    {
        // �R���g���[���[��ݒ肷��
        this.car = car;
        // �����{����ݒ肷��
        car.boostMultiplier = m_boostMultiplier;
        // �A���e�B���b�g�̌��ʎ��Ԃ�ݒ肷��
        m_timer = m_ultimateTime;
        // �A���e�B���b�g�������
        m_isActive = true;
    }

    public void Update()
    {
        if (!m_isActive) return;

        m_timer -= Time.deltaTime;
        // �A���e�B���b�g���Ԃ��I��������
        if (m_timer <= 0.0f)
        {
            // �I��������
            m_end = true;
        }
    }

    public void End()
    {
        // �����{����߂�
        car.boostMultiplier = 1.0f;
        // �A���e�B���b�g��Ԃ���������
        m_isActive = false;
        // �I����Ԃ���������
        m_end = false;
    }

    public bool IsEnd() { return m_end; }

    public bool IsActive() { return m_isActive; }
}
