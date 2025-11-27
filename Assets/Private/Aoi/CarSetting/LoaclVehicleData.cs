using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Data/LocalData")]
public class LoaclVehicleData : ScriptableObject
{
    [SerializeField]int m_vehicleID;
    [SerializeField] GameObject m_vehicleModel;

    public int ID { get { return m_vehicleID; } }
    public GameObject Model { get { return m_vehicleModel;} }
}
