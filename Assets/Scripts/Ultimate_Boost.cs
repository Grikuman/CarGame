using UnityEngine;

public class Ultimate_Boost : IUltimate
{
    private float duration = 3.0f;
    private float multiplier = 3.0f;
    private float timer;
    private HoverCarController car;
    private bool active;

    public bool IsActive => active;

    public void Activate(HoverCarController car)
    {
        this.car = car;
        car.boostMultiplier = multiplier;
        timer = duration;
        active = true;
    }

    public void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            End();
        }
    }

    public void End()
    {
        if (car != null) car.boostMultiplier = 1f;
        active = false;
    }
}
