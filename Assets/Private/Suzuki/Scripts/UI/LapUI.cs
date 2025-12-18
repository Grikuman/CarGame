using UnityEngine;
using TMPro;

public class LapUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lapText;

    public void UpdateLap(int currentLap, int totalLaps)
    {
        lapText.text = $"Lap {currentLap} / {totalLaps}";
    }

    public void Show(bool value)
    {
        lapText.gameObject.SetActive(value);
    }
}
