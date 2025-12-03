using UnityEngine;

public class TestReceiver : MonoBehaviour,IVehicleReceiver
{
    public void Receipt(GameObject vehicle, Rigidbody rigidbody)
    {
        Debug.Log("ƒf[ƒ^‚ğó‚¯æ‚è‚Ü‚µ‚½");
    }
}
