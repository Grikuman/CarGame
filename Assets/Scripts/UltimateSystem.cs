using UnityEngine;

public class UltimateSystem : MonoBehaviour
{
    public float cooldown = 10f;

    private float cooldownTimer = 0f;
    private IUltimate currentUltimate;
    private HoverCarController car;

    void Start()
    {
        car = GetComponent<HoverCarController>();

        // �Ƃ肠���� Boost Ultimate ���Z�b�g
        currentUltimate = new Ultimate_Boost();
    }

    void Update()
    {
        // �A���e�B���b�g�������Ȃ�X�V
        if (currentUltimate != null && currentUltimate.IsActive)
        {
            currentUltimate.Update();
        }

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public void TryActivateUltimate()
    {
        if (cooldownTimer <= 0f && currentUltimate != null && !currentUltimate.IsActive)
        {
            currentUltimate.Activate(car);
            cooldownTimer = cooldown;
        }
    }

    public void SetUltimate(IUltimate ultimate)
    {
        currentUltimate = ultimate;
    }
}
