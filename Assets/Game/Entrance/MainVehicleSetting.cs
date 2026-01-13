using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使う車を変更
/// </summary>
public class MainVehicleSetting : MonoBehaviour
{
    //車のデータ
    [SerializeField] VehicleDataManager m_vehicleSetting;
    //現在選択の番号
    [SerializeField]int m_currentVehicleIndex = 0;
    //車を設置する場所
    [SerializeField] GameObject m_carPosition;
    //車
    GameObject m_vehicle;
    LoaclVehicleData m_vehicleData;
    //使う車のID
    public int VehicleID { get { return m_vehicleData.ID; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(m_vehicleSetting == null)
        {
            Debug.LogWarning("[MainVehicleSetting]VehicleDataManagerを設定してください",gameObject);
            return;
        }
    }

    /// <summary>
    /// 配列を次へ
    /// </summary>
    public void MoveUp()
    {
        VehicleChange(m_currentVehicleIndex + 1);
    }

    /// <summary>
    /// 配列を前に
    /// </summary>
    public void MoveDown()
    {
        VehicleChange(m_currentVehicleIndex - 1);
    }


    public void VehicleChnage()
    {
        VehicleChange(m_currentVehicleIndex);
    }

    /// <summary>
    /// 車を変更
    /// </summary>
    /// <param name="index"></param>
     public void VehicleChange(int index)
    {
        if (m_vehicleSetting == null) return;

        if (index >= m_vehicleSetting.MaxVehiicleNumber||index < 0)
        {
            Debug.LogWarning($"[MainVehicleSetting]{index}は範囲外です", gameObject);
            return;
        }
        
        m_currentVehicleIndex = index;
        //現在の車を削除
        if(m_vehicle)Destroy(m_vehicle);

        //データ取得
        m_vehicleData = m_vehicleSetting.GetDataToIndex(index);
        if (m_vehicleData == null) return;

        //車生成
        m_vehicle = Instantiate(m_vehicleData.Model,m_carPosition.transform.position,m_carPosition.transform.rotation);
    }
}
