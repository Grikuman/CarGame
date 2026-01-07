using Fusion;
using UnityEngine;

public class NetworkChecker : NetworkBehaviour
{


    public bool IsStateAuthority()
    {
        if(Runner == null||!Runner.IsRunning)return false;
        return Object.HasStateAuthority;
    }
}
