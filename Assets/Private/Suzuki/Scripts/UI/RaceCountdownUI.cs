using UnityEngine;
using UnityEngine.UI;

public class RaceCountdownUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image countdownImage;

    [Header("Sprites")]
    [SerializeField] private Sprite[] numberSprites; // index 0 = 1, 1 = 2, 2 = 3
    [SerializeField] private Sprite goSprite;

    private void Awake()
    {
        countdownImage.gameObject.SetActive(false);
    }

    public void ShowNumber(int number)
    {
        int index = number - 1;

        if (index < 0 || index >= numberSprites.Length) return;

        countdownImage.sprite = numberSprites[index];
        countdownImage.gameObject.SetActive(true);
    }

    public void ShowGo()
    {
        countdownImage.sprite = goSprite;
        countdownImage.gameObject.SetActive(true);
    }

    public void Hide()
    {
        countdownImage.gameObject.SetActive(false);
    }
}
