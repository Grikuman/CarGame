using UnityEngine;

public class BoostSystem : MonoBehaviour
{
    public float boostMultiplier = 2.0f;   // 倍率
    public float boostDuration = 2.0f;     // 継続時間
    public float boostCooldown = 5.0f;     // クールダウン

    private float currentCooldown = 0.0f;
    private float boostTimer = 0.0f;
    private bool isBoosting = false;

    private HoverCarController car;

    void Start()
    {
        car = GetComponent<HoverCarController>();
    }

    void Update()
    {
        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0.0f)
            {
                EndBoost();
            }
        }

        if (currentCooldown > 0.0f)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    // 外部から呼ばれる
    public void TryStartBoost()
    {
        if (currentCooldown <= 0.0f && !isBoosting)
        {
            StartBoost();
        }
    }

    private void StartBoost()
    {
        car.boostMultiplier = boostMultiplier;
        boostTimer = boostDuration;
        isBoosting = true;
        currentCooldown = boostCooldown;
    }

    private void EndBoost()
    {
        car.boostMultiplier = 1.0f;
        isBoosting = false;
    }
}
