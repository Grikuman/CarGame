using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 車のデータを送る
/// </summary>
public class VehicleSender : MonoBehaviour
{
    //車(ローカル用)
    [SerializeField]GameObject m_Vehicle;
    //開始時に自動で送るか
    [SerializeField] bool m_autoSend;

    bool m_isSend = false;

    //車の受け取りて(インスペクター用)
    [SerializeField, Tooltip("入れ方\n1.インスペクター画面を増やす\n2.この画面のほうを固定する\n3.固定してないほうの画面で使うコンポーネント開きドラッグアンドドロップしてここに入れる")]
    [ComponentRestriction(typeof(IVehicleReceiver))]
    List<Component> m_vehicleUsersInspector;
    List<IVehicleReceiver> m_vehicleUsers = new List<IVehicleReceiver>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //変換処理
        foreach (var user in m_vehicleUsersInspector)
        {
            if (user is IVehicleReceiver receiver)
            {
                m_vehicleUsers.Add(receiver);
            }
        }

        
        if(m_Vehicle != null&&m_autoSend)
        {
            Send(m_Vehicle);
        }
    }

    /// <summary>
    /// 車のデータを送る
    /// </summary>
    /// <param name="vehicle"></param>
    public void Send(GameObject vehicle)
    {
        if (m_isSend) return;

        //最低限のコンポーネントチェック
        Rigidbody rigidbody = vehicle.GetComponent<Rigidbody>();
        if(rigidbody ==  null)
        {
            Debug.LogWarning($"[VehicleSender]{vehicle.gameObject}にRigidbodyがついていません");
        }
        VehicleController vehicleController = vehicle.GetComponent<VehicleController>();
        if(vehicleController == null)
        {
            Debug.LogWarning($"[VehicleSender]{vehicle.gameObject}にVehicleControllerがついていません");
        }

        //優先度順に並び替え
        m_vehicleUsers.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        //車のデータを送信
        foreach (var user in m_vehicleUsers)
        {
            user.Receipt(vehicle, rigidbody);
        }

        m_isSend = true;
    }
}
