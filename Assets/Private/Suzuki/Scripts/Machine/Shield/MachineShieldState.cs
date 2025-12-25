using UnityEngine;

public class MachineShieldState : MonoBehaviour
{
    public bool IsShieldActive { get; private set; }

    public void Enable()
    {
        IsShieldActive = true;
    }

    public void Disable()
    {
        IsShieldActive = false;
    }
}
