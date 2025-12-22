using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vehicle/Data/LocalData")]
public class LoaclVehicleData : ScriptableObject
{
    //ID
    [SerializeField]int m_vehicleID;
    //モデル(いずれ消すかも)
    [SerializeField] GameObject m_vehicleModel;
    //モジュールデータ
    [SerializeField]
    private List<VehicleModuleFactoryBase> m_moduleFactories = new List<VehicleModuleFactoryBase>();

    public int ID { get { return m_vehicleID; } }
    /// <summary>
    /// 見た目のモデルの取得　ViewModuleがあればそちらを優先
    /// </summary>
    public GameObject Model { 
        get 
        {
            var view = GetModuleFactory<MachineViewModuleData>();
            if(view == null)return m_vehicleModel;
            return view.VehcleModel;

        } 
    }


    public List<VehicleModuleFactoryBase> ModuleFactoryBases { get { return m_moduleFactories; } }

    /// <summary>
    /// 特定のモジュールデータを取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetModuleFactory<T>() where T : VehicleModuleFactoryBase
    {
        for (int i = 0; i < m_moduleFactories.Count; i++)
        {
            if (m_moduleFactories[i] is T factory)
            {
                return factory;
            }
        }
        return null;
    }
}
