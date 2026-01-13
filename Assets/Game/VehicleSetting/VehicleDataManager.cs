using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Vehicle/Data/DataManager")]
public class VehicleDataManager : ScriptableObject
{
    //保存されている車のデータ
    [SerializeField]List<LoaclVehicleData> m_vehicles = new List<LoaclVehicleData>();
    //配列の保存番号と車のIDの結びつき(基本同じ値)
    Dictionary<int,int> m_idtoIndex = new Dictionary<int,int>();

    //最大車の台数
    public int MaxVehiicleNumber {  get { return m_vehicles.Count; } }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        //配列番号と車IDを紐づけ
        m_idtoIndex.Clear();
        for (int i = 0; i < m_vehicles.Count; i++)
        {
            m_idtoIndex[m_vehicles[i].ID] = i;
        }
    }

    /// <summary>
    /// IDに対応したインデックス番号を返す
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetIndexToID(int id)
    {
        if (m_idtoIndex.ContainsKey(id))return m_idtoIndex[id];
        return -1;
    }

    /// <summary>
    /// インデックス番号でデータ取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public LoaclVehicleData GetDataToIndex(int index)
    {
        if(index >= MaxVehiicleNumber||index < 0)
        {
            Debug.LogWarning($"[VehicleDataManager]配列番号{index}がありません");
            return null;
        }
        return m_vehicles[index];
    }

    /// <summary>
    /// IDでデータ取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public LoaclVehicleData GetDataToID(int id)
    {
        // IDが存在するか確認
        if (!m_idtoIndex.ContainsKey(id))
        {
            Debug.LogWarning($"[VehicleDataManager]ID{id}がありません");
            return null;
        }

        int index = m_idtoIndex[id];

        return GetDataToIndex(index);
    }
}
