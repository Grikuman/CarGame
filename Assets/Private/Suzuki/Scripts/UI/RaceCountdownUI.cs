using UnityEngine;
using TMPro;

public class RaceCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    public void ShowNumber(int number)
    {
        countdownText.text = number.ToString();
        countdownText.gameObject.SetActive(true);
    }

    public void ShowGo()
    {
        countdownText.text = "GO!";
        countdownText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        countdownText.gameObject.SetActive(false);
    }
}
