using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectView : MonoBehaviour
{
    [System.Serializable]
    public struct DataSet
    {
        [SerializeField] int m_machineID;
        [SerializeField] Sprite m_select;
        [SerializeField] Sprite m_machineData;

        public int MachineID => m_machineID;
        public Sprite Select => m_select;
        public Sprite MachineData => m_machineData;
    }

    [SerializeField] List<DataSet> m_dataSets;
    [SerializeField] Image m_selectImage;
    [SerializeField] Image m_dataImage;

    public void DataChange(int machineID)
    {
        DataSet data = default;
        bool search = false;
        foreach (var dataset in m_dataSets)
        {
            if (dataset.MachineID == machineID)
            {
                data = dataset;
                search = true;
                break;
            }
        }
        if (!search) return;

        m_selectImage.sprite = data.Select;
        m_dataImage.sprite = data.MachineData;
    }
}
